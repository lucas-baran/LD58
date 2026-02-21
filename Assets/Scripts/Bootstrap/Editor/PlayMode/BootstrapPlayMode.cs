using UnityEditor;
using UnityEditor.SceneManagement;

namespace LucasBaran.Bootstrap
{
    [InitializeOnLoad]
    internal static class BootstrapPlayMode
    {
        static BootstrapPlayMode()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange state_change)
        {
            switch (state_change)
            {
                case PlayModeStateChange.ExitingEditMode:
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    SetupPlayModeScene();
                    break;
                }
                default:
                    break;
            }
        }

        private static void SetupPlayModeScene()
        {
            if (!BootstrapEditorPrefs.EnableBootstrapAtPlayMode)
            {
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            AssetUtils.TryGetBootstrapSceneAsset(out SceneAsset scene_asset);
            EditorSceneManager.playModeStartScene = scene_asset;
        }
    }
}
