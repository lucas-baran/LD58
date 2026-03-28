using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace LucasBaran.Bootstrap.Toolbar
{
    internal static class PlayButtonFactory
    {
        public static MainToolbarDropdown Create()
        {
            Texture2D texture = EditorGUIUtility.IconContent("d_PlayButton@2x").image as Texture2D;
            MainToolbarContent content = new(texture, "Play scenario");

            return new MainToolbarDropdown(content, OnButtonClicked);
        }

        private static void OnButtonClicked(Rect rect)
        {
            using var editor_skin_scope = GUISkinUtils.GetEditorScope();
            PlayToolbarDropdown dropdown = new();
            dropdown.Show(rect);
        }
    }
}
