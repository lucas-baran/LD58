using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LucasBaran.Bootstrap
{
    public sealed class ScenarioLoader : MonoBehaviour
    {
        private readonly LoadedScenarios _loadedScenarios = new();
        private readonly Stack<Scenario> _pushInLoadingQueue = new();
        private readonly List<Scenario> _loadingQueue = new();

        private Scenario _loadingScenario;

        public static bool HasInstance => Instance != null;
        public static ScenarioLoader Instance { get; private set; }

        public async UniTask LoadAsync(Scenario scenario)
        {
            try
            {
                if (scenario == null)
                {
                    throw new ArgumentNullException(nameof(scenario));
                }

                if (_loadedScenarios.IsLoaded(scenario))
                {
                    return;
                }

                PushInLoadingQueue(scenario);
                await WaitForScenarioToLoadAsync(scenario);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public async UniTask LoadFromGroupAsync(ScenarioGroup scenario_group)
        {
            try
            {
                if (scenario_group == null)
                {
                    throw new ArgumentNullException(nameof(scenario_group));
                }

                await UniTask.WhenAll(LoadFromGroup(scenario_group));
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public async UniTask LoadFromGroupsAsync(IEnumerable<ScenarioGroup> scenario_groups)
        {
            try
            {
                if (scenario_groups == null)
                {
                    throw new ArgumentNullException(nameof(scenario_groups));
                }

                IEnumerable<UniTask> tasks = scenario_groups.Select(scenario_group => LoadFromGroupAsync(scenario_group));
                await UniTask.WhenAll(tasks);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private IEnumerable<UniTask> LoadFromGroup(ScenarioGroup scenario_group)
        {
            IReadOnlyList<Scenario> scenarios = scenario_group.Scenarios;
            int scenario_count = scenarios.Count;

            for (int scenario_index = 0; scenario_index < scenario_count; scenario_index++)
            {
                Scenario scenario = scenarios[scenario_index];
                yield return LoadAsync(scenario);
            }
        }

        private void PushInLoadingQueue(Scenario scenario)
        {
            _pushInLoadingQueue.Push(scenario);

            while (_pushInLoadingQueue.TryPop(out Scenario scenario_to_push))
            {
                _loadingQueue.Add(scenario_to_push);

                IReadOnlyList<ScenarioGroup> dependencies = scenario_to_push.Dependencies;
                int dependency_count = dependencies.Count;

                for (int dependency_index = 0; dependency_index < dependency_count; dependency_index++)
                {
                    ScenarioGroup dependency = dependencies[dependency_index];
                    IReadOnlyList<Scenario> scenario_dependencies = dependency.Scenarios;
                    int scenario_dependency_count = scenario_dependencies.Count;

                    for (int scenario_dependency_index = 0; scenario_dependency_index < scenario_dependency_count; scenario_dependency_index++)
                    {
                        Scenario scenario_dependency = scenario_dependencies[scenario_dependency_index];
                        _pushInLoadingQueue.Push(scenario_dependency);
                    }
                }
            }
        }

        private async UniTask LoadNextScenarioAsync()
        {
            try
            {
                while (_loadingQueue.Count > 0)
                {
                    _loadingScenario = _loadingQueue[^1];
                    _loadingQueue.RemoveAt(_loadingQueue.Count - 1);
                    _loadedScenarios.AddScenario(_loadingScenario);

                    await LoadScenesAsync(_loadingScenario);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                _loadingScenario = null;
            }
        }

        private async UniTask LoadScenesAsync(Scenario scenario)
        {
            IReadOnlyList<Scenario.SceneReference> scene_references = scenario.SceneReferences;
            int scene_reference_count = scene_references.Count;
            UniTask[] load_tasks = new UniTask[scene_reference_count];
            AsyncOperationHandle<SceneInstance> active_scene_handle = default;

            for (int scene_reference_index = 0; scene_reference_index < scene_reference_count; scene_reference_index++)
            {
                Scenario.SceneReference scene_reference = scenario.SceneReferences[scene_reference_index];
                SceneAssetReference scene_asset_reference = scene_reference.SceneAssetReference;
                _loadedScenarios.AddSceneReference(scene_asset_reference);

                if (scene_asset_reference.IsValid())
                {
                    load_tasks[scene_reference_index] = UniTask.CompletedTask;
                }
                else
                {
                    AsyncOperationHandle<SceneInstance> handle = scene_asset_reference.LoadSceneAsync(LoadSceneMode.Additive, priority: scene_reference.Priority);
                    load_tasks[scene_reference_index] = handle.ToUniTask();

                    if (scene_reference.ActiveScene)
                    {
                        active_scene_handle = handle;
                    }
                }
            }

            await UniTask.WhenAll(load_tasks);

            if (active_scene_handle.IsValid())
            {
                SceneManager.SetActiveScene(active_scene_handle.Result.Scene);
            }
        }

        private async UniTask WaitForScenarioToLoadAsync(Scenario scenario)
        {
            await UniTask.WaitUntil((_loadedScenarios, scenario), static ((LoadedScenarios LoadedScenarios, Scenario Scenario) args) => args.LoadedScenarios.IsLoaded(args.Scenario));
        }

        public async UniTask UnloadAsync(Scenario scenario)
        {
            try
            {
                if (scenario == null)
                {
                    throw new ArgumentNullException(nameof(scenario));
                }

                if (scenario == _loadingScenario)
                {
                    await WaitForScenarioToLoadAsync(scenario);
                }

                if (_loadedScenarios.RemoveScenario(scenario))
                {
                    await UnloadScenesAsync(scenario);

                    return;
                }

                RemoveScenarioFromLoadingQueue(scenario);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public async UniTask UnloadAsync(IEnumerable<Scenario> scenarios)
        {
            try
            {
                if (scenarios == null)
                {
                    throw new ArgumentNullException(nameof(scenarios));
                }

                await UniTask.WhenAll(Unload(scenarios));
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private IEnumerable<UniTask> Unload(IEnumerable<Scenario> scenarios)
        {
            foreach (Scenario scenario in scenarios)
            {
                yield return UnloadAsync(scenario);
            }
        }

        private void RemoveScenarioFromLoadingQueue(Scenario scenario)
        {
            _loadingQueue.Remove(scenario);
        }

        private async UniTask UnloadScenesAsync(Scenario scenario)
        {
            IReadOnlyList<Scenario.SceneReference> scene_references = scenario.SceneReferences;
            int scene_reference_count = scene_references.Count;
            UniTask[] unload_tasks = new UniTask[scene_reference_count];

            for (int scene_reference_index = 0; scene_reference_index < scene_reference_count; scene_reference_index++)
            {
                Scenario.SceneReference scene_reference = scenario.SceneReferences[scene_reference_index];
                SceneAssetReference scene_asset_reference = scene_reference.SceneAssetReference;
                _loadedScenarios.RemoveSceneReference(scene_asset_reference, out bool is_referenced);
                unload_tasks[scene_reference_index] = is_referenced ? UniTask.CompletedTask : scene_asset_reference.UnLoadScene().ToUniTask();
            }

            await UniTask.WhenAll(unload_tasks);
        }

        public async UniTask UnloadAllFromGroupAsync(ScenarioGroup scenario_group)
        {
            try
            {
                if (scenario_group == null)
                {
                    throw new ArgumentNullException(nameof(scenario_group));
                }

                await UnloadAsync(scenario_group.Scenarios);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public async UniTask UnloadAllFromGroupsAsync(IReadOnlyList<ScenarioGroup> scenario_groups)
        {
            try
            {
                if (scenario_groups == null)
                {
                    throw new ArgumentNullException(nameof(scenario_groups));
                }

                IEnumerable<UniTask> unload_tasks = UnloadAllFromGroups(scenario_groups);
                await UniTask.WhenAll(unload_tasks);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private IEnumerable<UniTask> UnloadAllFromGroups(IReadOnlyList<ScenarioGroup> scenario_groups)
        {
            int scenario_group_count = scenario_groups.Count;

            for (int scenario_group_index = 0; scenario_group_index < scenario_group_count; scenario_group_index++)
            {
                ScenarioGroup scenario_group = scenario_groups[scenario_group_index];
                IReadOnlyList<Scenario> scenarios = scenario_group.Scenarios;
                int scenario_count = scenarios.Count;

                for (int scenario_index = 0; scenario_index < scenario_count; scenario_index++)
                {
                    Scenario scenario = scenarios[scenario_index];

                    yield return UnloadAsync(scenario);
                }
            }
        }

        public async UniTask UnloadAllAsync()
        {
            List<Scenario> scenarios_to_unload = null;

            try
            {
                scenarios_to_unload = ListPool<Scenario>.Get();
                scenarios_to_unload.AddRange(_loadedScenarios);

                await UnloadAsync(scenarios_to_unload);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                if (scenarios_to_unload != null)
                {
                    ListPool<Scenario>.Release(scenarios_to_unload);
                }
            }
        }

        public async UniTask UnloadAllAsync(Predicate<Scenario> predicate)
        {
            List<Scenario> scenarios_to_unload = null;

            try
            {
                if (predicate == null)
                {
                    throw new ArgumentNullException(nameof(predicate));
                }

                scenarios_to_unload = ListPool<Scenario>.Get();

                foreach (Scenario scenario in _loadedScenarios)
                {
                    if (predicate.Invoke(scenario))
                    {
                        scenarios_to_unload.Add(scenario);
                    }
                }

                await UnloadAsync(scenarios_to_unload);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                if (scenarios_to_unload != null)
                {
                    ListPool<Scenario>.Release(scenarios_to_unload);
                }
            }
        }

        private void Update()
        {
            if (_loadingScenario == null && _loadingQueue.Count > 0)
            {
                LoadNextScenarioAsync().Forget();
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
