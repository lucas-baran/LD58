using UnityEditor;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    internal static class DropdownUtils
    {
        public static Texture2D GetSelectionIcon(bool is_selected)
        {
            return is_selected ? EditorGUIUtility.IconContent("d_FilterSelectedOnly").image as Texture2D : null;
        }
    }
}
