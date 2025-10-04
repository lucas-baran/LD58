using System;
using UnityEngine;

namespace LD58
{
    [Serializable]
    public sealed class SceneReference
#if UNITY_EDITOR
        : ISerializationCallbackReceiver
#endif
    {
        [SerializeField, HideInInspector] private string _sceneName;

        public string SceneName => _sceneName;

#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset _sceneAsset;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (_sceneAsset != null)
            {
                _sceneName = _sceneAsset.name;
            }
        }
#endif
    }
}
