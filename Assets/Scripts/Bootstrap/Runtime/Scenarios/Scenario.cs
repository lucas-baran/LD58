using System;
using System.Collections.Generic;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    [CreateAssetMenu(fileName = "SO_Scenario", menuName = "Lucas Baran/Bootstrap/Scenario")]
    public sealed class Scenario : ScriptableObject
    {
        [SerializeField] private ScenarioGroup[] _dependencies;
        [SerializeField] private SceneReference[] _sceneReferences;

        public IReadOnlyList<ScenarioGroup> Dependencies => _dependencies;
        public IReadOnlyList<SceneReference> SceneReferences => _sceneReferences;

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
    }
}
