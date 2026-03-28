using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace LucasBaran.Bootstrap.Toolbar
{
    public static class OpenToolbarActions
    {
        public static void OpenBootstrapScene(bool additive)
        {
            OpenBootstrapScene();

            if (!additive)
            {
                CloseAllScenesExceptBootstrap();
            }
        }

        public static void CloseBootstrapScene()
        {
            if (AssetUtils.TryGetBootstrapScenePath(out string scene_path))
            {
                Scene scene = EditorSceneManager.GetSceneByPath(scene_path);

                if (scene.IsValid())
                {
                    EditorSceneManager.CloseScene(scene, removeScene: true);
                }
            }
        }

        public static void LocateBootstrapScene()
        {
            if (AssetUtils.TryGetBootstrapSceneAsset(out SceneAsset scene_asset))
            {
                EditorGUIUtility.PingObject(scene_asset);
            }
        }

        public static void OpenScenario(Scenario scenario, bool additive)
        {
            bool close_bootstrap_when_finished = !additive || !IsBootstrapSceneOpened();

            if (!additive)
            {
                OpenBootstrapScene();
                CloseAllScenesExceptBootstrap();
            }

            OpenScenarioScenes(scenario);

            if (close_bootstrap_when_finished)
            {
                CloseBootstrapScene();
            }
        }

        public static void CloseScenario(Scenario scenario)
        {
            CloseScenarioScenes(scenario);
        }

        public static void LocateScenario(Scenario scenario)
        {
            EditorGUIUtility.PingObject(scenario);
        }

        private static void OpenBootstrapScene()
        {
            if (AssetUtils.TryGetBootstrapScenePath(out string scene_path))
            {
                EditorSceneManager.OpenScene(scene_path, OpenSceneMode.Additive);
            }
        }

        private static void CloseAllScenesExceptBootstrap()
        {
            int scene_count = EditorSceneManager.sceneCount;
            AssetUtils.TryGetBootstrapScenePath(out string bootstrap_scene_path);

            for (int scene_index = scene_count - 1; scene_index >= 0; scene_index--)
            {
                Scene scene = EditorSceneManager.GetSceneAt(scene_index);

                if (scene.path != bootstrap_scene_path)
                {
                    EditorSceneManager.CloseScene(scene, removeScene: true);
                }
            }
        }

        private static void OpenScenarioScenes(Scenario scenario)
        {
            IReadOnlyList<Scenario.SceneReference> scene_references = scenario.SceneReferences;
            int scene_count = scene_references.Count;

            for (int scene_index = 0; scene_index < scene_count; scene_index++)
            {
                Scenario.SceneReference scene_reference = scene_references[scene_index];
                SceneAsset scene_asset = scene_reference.SceneAssetReference.editorAsset;
                string scene_path = AssetDatabase.GetAssetPath(scene_asset);
                EditorSceneManager.OpenScene(scene_path, OpenSceneMode.Additive);
            }
        }

        private static void CloseScenarioScenes(Scenario scenario)
        {
            IReadOnlyList<Scenario.SceneReference> scene_references = scenario.SceneReferences;
            int scene_count = scene_references.Count;

            for (int scene_index = 0; scene_index < scene_count; scene_index++)
            {
                Scenario.SceneReference scene_reference = scene_references[scene_index];
                SceneAsset scene_asset = scene_reference.SceneAssetReference.editorAsset;
                string scene_path = AssetDatabase.GetAssetPath(scene_asset);
                Scene scene = EditorSceneManager.GetSceneByPath(scene_path);

                if (scene.IsValid())
                {
                    if (EditorSceneManager.sceneCount == 1)
                    {
                        OpenBootstrapScene();
                    }

                    EditorSceneManager.CloseScene(scene, removeScene: true);
                }
            }
        }

        private static bool IsBootstrapSceneOpened()
        {
            bool is_loaded = false;
            int scene_count = EditorSceneManager.sceneCount;
            AssetUtils.TryGetBootstrapScenePath(out string bootstrap_scene_path);

            for (int scene_index = 0; !is_loaded && scene_index < scene_count; scene_index++)
            {
                Scene scene = EditorSceneManager.GetSceneAt(scene_index);
                is_loaded = scene.path == bootstrap_scene_path;
            }

            return is_loaded;
        }
    }
}
