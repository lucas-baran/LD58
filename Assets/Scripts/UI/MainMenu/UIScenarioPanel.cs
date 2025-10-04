using LD58.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UIScenarioPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Image _picture;

        public void SetScenarioData(ScenarioData scenario_data)
        {
            _nameText.text = scenario_data.Name;
            _picture.sprite = scenario_data.Picture;
        }
    }
}
