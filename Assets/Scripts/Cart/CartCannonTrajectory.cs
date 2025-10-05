using System.Runtime.CompilerServices;
using UnityEngine;

namespace LD58.Cart
{
    public class CartCannonTrajectory : MonoBehaviour
    {
        [SerializeField] private float _totalTime = 1f;
        [SerializeField] private int _segmentCount = 10;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private CartCannon _cannon;

        private Vector3[] _positions;

        private void RegeneratePositions()
        {
            Vector3 initial_position = _cannon.ShootPosition;
            float shoot_angle = Mathf.Deg2Rad * Vector3.SignedAngle(Vector3.right, _cannon.ShootVelocity, Vector3.forward);
            float gravity = Physics.gravity.magnitude;
            float gravity_scale = gravity * _cannon.CurrentFruit.GravityScale;
            float raw_velocity = _cannon.ShootVelocity.magnitude;

            for (int i = 0; i < _positions.Length; i++)
            {
                float time = _totalTime * i / _positions.Length;
                _positions[i] = initial_position + GetPositionAt(time, raw_velocity, shoot_angle, gravity_scale, transform.position.z);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector3 GetPositionAt(float time, float initial_velocity, float shoot_angle, float gravity, float z)
        {
            float velocity_time = time * initial_velocity;

            return new Vector3(
                velocity_time * Mathf.Cos(shoot_angle),
                velocity_time * Mathf.Sin(shoot_angle) - 0.5f * gravity * time * time,
                z
            );
        }

        private void RefreshLineRenderer()
        {
            if (_cannon.CurrentFruit == null)
            {
                _lineRenderer.enabled = false;

                return;
            }

            RegeneratePositions();
            _lineRenderer.enabled = true;
            _lineRenderer.widthMultiplier = _cannon.CurrentFruit.Data.ColliderSize;
            _lineRenderer.SetPositions(_positions);
        }

        private void Update()
        {
            RefreshLineRenderer();
        }

        private void Awake()
        {
            _positions = new Vector3[_segmentCount + 1];
            _lineRenderer.positionCount = _positions.Length;
        }
    }
}
