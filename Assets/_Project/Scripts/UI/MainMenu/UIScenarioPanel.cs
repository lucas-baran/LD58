using Cysharp.Threading.Tasks;
using LD58.Game;
using LucasBaran.Bootstrap;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UIScenarioPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Image _picture;

        private Scenario _scenario;
        private CancellationTokenSource _loadLevelDescriptionCTS;

        public void SetScenario(Scenario scenario)
        {
            CancelLoadLevelDescription();
            UnloadPreviousLevelDescription();
            _scenario = scenario;
            _loadLevelDescriptionCTS = new CancellationTokenSource();
            LoadLevelDescriptionAsync(scenario, _loadLevelDescriptionCTS.Token).Forget();
        }

        private void UnloadPreviousLevelDescription()
        {
            if (_scenario != null && _scenario.TryGetModule(ScenarioModules.LEVEL_DESCRIPTION, out AssetReference level_description_reference) && level_description_reference.IsValid())
            {
                level_description_reference.ReleaseAsset();
            }
        }

        private async UniTaskVoid LoadLevelDescriptionAsync(Scenario scenario, CancellationToken cancellation_token)
        {
            LevelDescription level_description = await scenario.LoadModuleAsync<LevelDescription>(ScenarioModules.LEVEL_DESCRIPTION, cancellation_token);
            _nameText.text = level_description.Name;
            _picture.sprite = level_description.Picture;
        }

        private void CancelLoadLevelDescription()
        {
            if (_loadLevelDescriptionCTS != null)
            {
                _loadLevelDescriptionCTS.Cancel();
                _loadLevelDescriptionCTS.Dispose();
                _loadLevelDescriptionCTS = null;
            }
        }

        private void OnDestroy()
        {
            CancelLoadLevelDescription();
            UnloadPreviousLevelDescription();
        }
    }
}
