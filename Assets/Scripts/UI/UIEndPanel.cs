using Cysharp.Threading.Tasks;
using LD58.Game;
using LD58.Levels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UIEndPanel : MonoBehaviour
    {
        [SerializeField] private EndText _winText;
        [SerializeField] private EndText _loseText;
        [SerializeField] private TMP_Text _mainText;
        [SerializeField] private TMP_Text _quoteText;
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _quitToMenuButton;

        private void Show(EndText end_text)
        {
            gameObject.SetActive(true);
            _mainText.text = end_text.Main;
            _quoteText.text = end_text.Quote;
        }

        private void Level_OnLose()
        {
            Show(_loseText);
        }

        private void Level_OnWin()
        {
            Show(_winText);
        }

        private void ReplayButton_OnClick()
        {
            GameManager.Instance.ReloadCurrentSceneAsync().Forget();
        }

        private void QuitToMenuButton_OnClick()
        {
            GameManager.Instance.LoadMainMenuSceneAsync().Forget();
        }

        private void Start()
        {
            Level.Instance.OnLose += Level_OnLose;
            Level.Instance.OnWin += Level_OnWin;
            _replayButton.onClick.AddListener(ReplayButton_OnClick);
            _quitToMenuButton.onClick.AddListener(QuitToMenuButton_OnClick);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _replayButton.onClick.RemoveListener(ReplayButton_OnClick);
            _quitToMenuButton.onClick.RemoveListener(QuitToMenuButton_OnClick);
        }

        [Serializable]
        private sealed class EndText
        {
            [SerializeField] private string _main = string.Empty;
            [SerializeField] private string _quote = string.Empty;

            public string Main => _main;
            public string Quote => _quote;
        }
    }
}
