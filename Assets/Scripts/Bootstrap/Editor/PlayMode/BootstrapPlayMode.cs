using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LucasBaran.Bootstrap
{
    [InitializeOnLoad]
    internal static class BootstrapPlayMode
    {
        private const string BOOTSTRAP_SCENE_NAME = "Scene_Bootstrap";

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

            string bootstrap_scene_path = AssetDatabase.FindAssets("t:SceneAsset " + BOOTSTRAP_SCENE_NAME).FirstOrDefault();
            SceneAsset bootstrap_scene_asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(bootstrap_scene_path);
            EditorSceneManager.playModeStartScene = bootstrap_scene_asset;
        }
    }
}
