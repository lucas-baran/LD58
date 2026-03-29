using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LucasBaran.Bootstrap
{
    [CreateAssetMenu(fileName = "SO_Scenario", menuName = "Lucas Baran/Bootstrap/Scenario")]
    public sealed class Scenario : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _showInDebugGui = true;
        [SerializeField] private ScenarioGroup[] _dependencies;
        [SerializeField] private SceneReference[] _sceneReferences;
        [SerializeField] private Module[] _modules;

        public string Name => string.IsNullOrEmpty(_name) ? name : _name;
        public bool ShowInDebugGui => _showInDebugGui;
        public IReadOnlyList<ScenarioGroup> Dependencies => _dependencies;
        public IReadOnlyList<SceneReference> SceneReferences => _sceneReferences;

        public bool TryGetModule(int id, out AssetReference asset_reference)
        {
            int module_count = _modules.Length;
            bool found_module = false;
            asset_reference = null;

            for (int module_index = 0; !found_module && module_index < module_count; module_index++)
            {
                Module module = _modules[module_index];
                found_module = module.Id == id;
                asset_reference = module.AssetReference;
            }

            return found_module;
        }

        public async UniTask<T> LoadModuleAsync<T>(int id, CancellationToken cancellation_token)
        {
            if (!TryGetModule(id, out AssetReference module_reference))
            {
                return default;
            }

            return await module_reference.LoadAssetAsync<T>().ToUniTask(cancellationToken: cancellation_token);
        }

        [Serializable]
        public sealed class SceneReference
        {
            [SerializeField] private bool _activeScene;
            [SerializeField] private int _priority;
            [SerializeField] private SceneAssetReference _sceneAssetReference;

            public bool ActiveScene => _activeScene;
            public int Priority => _priority;
            public SceneAssetReference SceneAssetReference => _sceneAssetReference;
        }

        [Serializable]
        public sealed class Module
        {
            [SerializeField] private int _id;
            [SerializeField] private AssetReference _assetReference;

            public int Id => _id;
            public AssetReference AssetReference => _assetReference;
        }
    }
}
