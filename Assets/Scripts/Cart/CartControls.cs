using LD58.Inputs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD58.Cart
{
    public class CartControls : MonoBehaviour
    {
        [SerializeField] private CartControlsData _data;
        [SerializeField] private Transform _cannonPivot;

        private Inputs_LD58.PlayerActions _playerActions;
        private Camera _camera;

        public event UnityAction OnShot = null;

        private void UpdatePosition()
        {
            float movement = _playerActions.Move.ReadValue<float>();
            transform.Translate(new Vector3(Time.fixedDeltaTime * movement * _data.MovementSpeed, 0f, 0f));

            float level_width = _camera.orthographicSize * _camera.aspect;
            float x = transform.position.x;

            if (Mathf.Abs(x) > level_width - _data.LevelBoundsPadding)
            {
                transform.position = new Vector3(Mathf.Sign(x) * (level_width - _data.LevelBoundsPadding), transform.position.y, transform.position.z);
            }
        }

        private void UpdateAim()
        {
            float aim = _playerActions.Aim.ReadValue<float>();
            _cannonPivot.Rotate(Vector3.forward, Time.fixedDeltaTime * aim * _data.AimSpeed);

            float angle = Vector3.SignedAngle(Vector3.down, -_cannonPivot.up, Vector3.forward);

            if (Mathf.Abs(angle) > _data.MaxAimAngle)
            {
                _cannonPivot.rotation = Quaternion.AngleAxis(Mathf.Sign(angle) * _data.MaxAimAngle, Vector3.forward);
            }
        }

        private void Shoot_performed(InputAction.CallbackContext context)
        {
            OnShot?.Invoke();
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateAim();
        }

        private void Awake()
        {
            _playerActions = InputManager.Instance.Player;
            _camera = Camera.main;

            _playerActions.Shoot.performed += Shoot_performed;
        }
    }
}
