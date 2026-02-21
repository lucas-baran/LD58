using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace LucasBaran.Bootstrap.Toolbar
{
    internal static class OpenButtonFactory
    {
        public static MainToolbarDropdown Create()
        {
            Texture2D texture = EditorGUIUtility.IconContent("d_MoreOptions@2x").image as Texture2D;
            MainToolbarContent content = new(texture, "Scenario selection");

            return new MainToolbarDropdown(content, OnButtonClicked);
        }

        private static void OnButtonClicked(Rect rect)
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                using var editor_skin_scope = GUISkinUtils.GetEditorScope();
                OpenToolbarDropdown dropdown = new();
                dropdown.Show(rect);
            }
        }
    }
}
