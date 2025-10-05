using Cysharp.Threading.Tasks;
using LD58.Inputs;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UITutorialPanel : MonoBehaviour
    {
        [SerializeField] private float _delayBeforeSubmit = 0.5f;
        [SerializeField] private Button _submitButton;
        [SerializeField] private List<GameObject> _maskObjects = new();

        private bool _submitPerformed = false;

        public async UniTask WaitForSubmitAsync(CancellationToken cancellation_token)
        {
            try
            {
                _submitPerformed = false;
                ShowVisuals(true);

                await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeSubmit), cancellationToken: cancellation_token);

                InputManager.Instance.UI.Submit.performed += Submit_performed;

                await UniTask.WaitUntil(this, tutorial_panel => tutorial_panel._submitPerformed, cancellationToken: cancellation_token);

                ShowVisuals(false);
            }
            finally
            {
                if (InputManager.HasInstance)
                {
                    InputManager.Instance.UI.Submit.performed -= Submit_performed;
                }
            }
        }

        private void ShowVisuals(bool enabled)
        {
            gameObject.SetActive(enabled);
            _submitButton.gameObject.SetActive(enabled);

            foreach (GameObject mask_object in _maskObjects)
            {
                mask_object.SetActive(enabled);
            }
        }

        private void SubmitButton_OnClick()
        {
            _submitPerformed = true;
        }

        private void Submit_performed(InputAction.CallbackContext context)
        {
            SubmitButton_OnClick();
        }

        private void Awake()
        {
            _submitButton.onClick.AddListener(SubmitButton_OnClick);
            ShowVisuals(false);
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.UI.Submit.performed -= Submit_performed;
            }

            _submitButton.onClick.RemoveListener(SubmitButton_OnClick);
        }
    }
}
