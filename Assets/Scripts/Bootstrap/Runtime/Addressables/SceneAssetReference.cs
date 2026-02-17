using System;
using UnityEngine.AddressableAssets;

namespace LucasBaran.Bootstrap
{
    [Serializable]
    public sealed class SceneAssetReference
#if UNITY_EDITOR
        : AssetReferenceT<UnityEditor.SceneAsset>
#else
        : AssetReference
#endif
    {
        public SceneAssetReference(string guid) : base(guid) { }
    }
}
