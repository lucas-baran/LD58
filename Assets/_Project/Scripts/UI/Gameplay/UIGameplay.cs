using Cysharp.Threading.Tasks;
using LD58.Game;
using LD58.Inputs;
using LD58.Levels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UIGameplay : MonoBehaviour
    {
        [SerializeField] private Button _quitToMenuButton;

        private void QuitToMenuButton_OnClick()
        {
            if (Level.HasInstance && Level.Instance.IsPlaying)
            {
                GameManager.Instance.LoadMainMenuSceneAsync().Forget();
            }
        }

        private void Cancel_performed(InputAction.CallbackContext context)
        {
            QuitToMenuButton_OnClick();
        }

        private void Start()
        {
            InputManager.Instance.UI.Cancel.performed += Cancel_performed;

            _quitToMenuButton.onClick.AddListener(QuitToMenuButton_OnClick);
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.UI.Cancel.performed -= Cancel_performed;
            }

            _quitToMenuButton.onClick.RemoveListener(QuitToMenuButton_OnClick);
        }
    }
}
