using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace LD58
{
    internal sealed class GenerateAddressableGroupsWindow : EditorWindow
    {
        private const string GENERATE_BUTTON = "Regenerate groups";

        private AddressableAssetGroupTemplate _groupTemplate = null;

        private IEnumerable<SelectedAsset> GetSelectedAssets()
        {
            return Selection.assetGUIDs
                .Select(guid => new SelectedAsset(guid, AssetDatabase.LoadAssetByGUID<Object>(new GUID(guid))));
        }

        private void RegenerateGroup(SelectedAsset selected_asset)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetEntry entry = settings.FindAssetEntry(selected_asset.Guid);
            AddressableAssetGroup group = null;

            if (entry == null
                || entry.parentGroup.entries.Count > 1
                )
            {
                System.Type[] schemas = _groupTemplate.SchemaObjects.Select(schema => schema.GetType()).ToArray();
                group = settings.CreateGroup(selected_asset.Asset.name, setAsDefaultGroup: false, readOnly: false, postEvent: true, schemasToCopy: null, schemas);
                _groupTemplate.ApplyToAddressableAssetGroup(group);

                entry = settings.CreateOrMoveEntry(selected_asset.Guid, group, readOnly: false, postEvent: false);
                List<AddressableAssetEntry> new_entries = new() { entry };

                group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, new_entries, postEvent: false, groupModified: true);
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, new_entries, postEvent: true, settingsModified: false);

                return;
            }

            group = entry.parentGroup;
            group.Name = selected_asset.Asset.name;

            group.SetDirty(AddressableAssetSettings.ModificationEvent.GroupRenamed, group.Name, postEvent: false, groupModified: true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupRenamed, group.Name, postEvent: true, settingsModified: false);
        }

        private void OnGUI()
        {
            IEnumerable<SelectedAsset> selected_assets = GetSelectedAssets();

            foreach (SelectedAsset selected_asset in selected_assets)
            {
                EditorGUILayout.LabelField(selected_asset.Asset.name);
            }

            _groupTemplate = (AddressableAssetGroupTemplate)EditorGUILayout.ObjectField(_groupTemplate, typeof(AddressableAssetGroupTemplate), allowSceneObjects: false);

            if (_groupTemplate != null
                && GUILayout.Button(GENERATE_BUTTON)
                )
            {
                foreach (SelectedAsset selected_asset in selected_assets)
                {
                    RegenerateGroup(selected_asset);
                }
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        [MenuItem("LD58/Generate addressable groups")]
        private static void ShowWindow()
        {
            GetWindow<GenerateAddressableGroupsWindow>();
        }

        private record SelectedAsset(string Guid, Object Asset);
    }
}
