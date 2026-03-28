using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

namespace LD58
{
    internal sealed class CreateAssetFoldersWindow : EditorWindow
    {
        private const string CREATE_BUTTON = "Create";
        private static readonly Folder[] FOLDERS = new Folder[]
        {
             Folder.Create<AnimationClip>( "Animations" ),
             Folder.Create<AudioClip>( "Audio" ),
             Folder.Create<Material>( "Materials" ),
             Folder.Create<PhysicsMaterial>( "PhysicsMaterials" ),
             Folder.Create<GameObject>( "Prefabs" ),
             Folder.Create<MonoScript>( "ScriptableObjects" ),
             Folder.Create<Shader>( "Shaders" ),
             Folder.Create<Texture>( "Textures" ),
             Folder.Create<VisualEffectAsset>( "VFX" ),
        };

        private void CreateFolder(string folder_name)
        {
            Type project_window_util_type = typeof(ProjectWindowUtil);
            MethodInfo get_active_folder_path = project_window_util_type.GetMethod("TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object[] parameters = new object[1];
            bool has_project_window = (bool)get_active_folder_path.Invoke(obj: null, parameters);

            if (!has_project_window)
            {
                return;
            }

            string path = (string)parameters[0];

            if (!AssetDatabase.IsValidFolder(Path.Combine(path, folder_name)))
            {
                AssetDatabase.CreateFolder(path, folder_name);
            }
        }

        private void DrawCreateButton(Folder folder)
        {
            using EditorGUILayout.HorizontalScope horizontal_scope = new();
            GUIContent content = new(folder.Name, AssetPreview.GetMiniTypeThumbnail(folder.ThumbnailType));
            EditorGUILayout.PrefixLabel(content);

            if (GUILayout.Button(CREATE_BUTTON))
            {
                CreateFolder(folder.Name);
            }
        }

        private void OnGUI()
        {
            for (int folder_index = 0; folder_index < FOLDERS.Length; folder_index++)
            {
                DrawCreateButton(FOLDERS[folder_index]);
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        [MenuItem("LD58/Create asset folders")]
        private static void ShowWindow()
        {
            GetWindow<CreateAssetFoldersWindow>();
        }

        private record Folder(string Name, Type ThumbnailType)
        {
            public static Folder Create<T>(string name)
                where T : UnityEngine.Object
            {
                return new Folder(name, typeof(T));
            }
        }
    }
}
