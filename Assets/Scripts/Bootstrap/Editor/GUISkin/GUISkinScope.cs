using System;
using UnityEditor;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    public static class GUISkinUtils
    {
        public static GUISkin GetEditorSkin()
        {
            return EditorGUIUtility.isProSkin
                ? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene)
                : EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
        }

        public static Scope GetEditorScope()
        {
            return new Scope(GetEditorSkin());
        }

        public readonly struct Scope : IDisposable
        {
            private readonly GUISkin _oldSkin;

            public Scope(
                GUISkin skin
                )
            {
                _oldSkin = GUI.skin;
                GUI.skin = skin;
            }

            public void Dispose()
            {
                GUI.skin = _oldSkin;
            }
        }
    }
}
