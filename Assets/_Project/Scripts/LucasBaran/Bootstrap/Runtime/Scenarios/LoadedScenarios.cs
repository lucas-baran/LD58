using System.Collections;
using System.Collections.Generic;

namespace LucasBaran.Bootstrap
{
    public sealed class LoadedScenarios : IEnumerable<Scenario>
    {
        private readonly LoadedSceneFactory _loadedSceneFactory = new();
        private readonly Dictionary<SceneAssetReference, LoadedScene> _loadedScenes = new();
        private readonly HashSet<Scenario> _loadedScenarios = new();

        public bool IsLoaded(Scenario scenario)
        {
            return _loadedScenarios.Contains(scenario);
        }

        public void AddScenario(Scenario scenario)
        {
            _loadedScenarios.Add(scenario);
        }

        public bool RemoveScenario(Scenario scenario)
        {
            return _loadedScenarios.Remove(scenario);
        }

        public void AddSceneReference(SceneAssetReference scene_reference)
        {
            if (!_loadedScenes.TryGetValue(scene_reference, out LoadedScene loaded_scene))
            {
                loaded_scene = _loadedSceneFactory.Get();
                _loadedScenes[scene_reference] = loaded_scene;
            }

            loaded_scene.ReferenceCount++;
        }

        public void RemoveSceneReference(SceneAssetReference scene_reference, out bool is_referenced)
        {
            if (!_loadedScenes.TryGetValue(scene_reference, out LoadedScene loaded_scene))
            {
                is_referenced = false;

                return;
            }

            loaded_scene.ReferenceCount--;
            is_referenced = loaded_scene.ReferenceCount > 0;

            if (!is_referenced)
            {
                _loadedScenes.Remove(scene_reference);
                _loadedSceneFactory.Return(loaded_scene);
            }
        }

        public HashSet<Scenario>.Enumerator GetEnumerator()
        {
            return _loadedScenarios.GetEnumerator();
        }

        IEnumerator<Scenario> IEnumerable<Scenario>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
