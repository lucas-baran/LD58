using LD58.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UITaxPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingShotsText;
        [SerializeField] private Button _payTaxesButton;

        public void SetRemainingShots(int remaining_shots)
        {
            _remainingShotsText.text = $"{remaining_shots} remaining shots";
        }

        public void SetButtonEnabled(bool enabled)
        {
            _payTaxesButton.interactable = enabled;
        }

        private void PayTaxesButton_OnClick()
        {
            Level.Instance.PayTaxes();
        }

        private void Start()
        {
            _payTaxesButton.onClick.AddListener(PayTaxesButton_OnClick);
        }

        private void OnDestroy()
        {
            _payTaxesButton.onClick.RemoveListener(PayTaxesButton_OnClick);
        }
    }
}
