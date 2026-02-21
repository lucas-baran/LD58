using System.Linq;
using UnityEditor;

namespace LucasBaran.Bootstrap
{
    public static class AssetUtils
    {
        private const string BOOTSTRAP_SCENE_NAME = "Scene_Bootstrap";

        public static bool TryGetBootstrapSceneAsset(out SceneAsset scene_asset)
        {
            GUID guid = AssetDatabase.FindAssetGUIDs("t:SceneAsset " + BOOTSTRAP_SCENE_NAME).FirstOrDefault();
            scene_asset = AssetDatabase.LoadAssetByGUID<SceneAsset>(guid);
            return scene_asset != null;
        }
    }
}
