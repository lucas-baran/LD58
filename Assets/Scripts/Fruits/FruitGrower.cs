using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LD58.Fruits
{
    public class FruitGrower : MonoBehaviour
    {
        [SerializeField] private float _cameraPadding = 1f;

        private readonly List<Fruit> _fruits = new();
        private Camera _camera;

        public void GrowFruits()
        {
            foreach (Fruit fruit in _fruits)
            {
                fruit.Grow();
            }
        }

        public bool IsAnyFruitMoving()
        {
            foreach (Fruit fruit in _fruits)
            {
                if (fruit.IsMoving)
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsOutOfBound(Fruit fruit)
        {
            Vector3 fruit_position = fruit.transform.position;
            Vector3 camera_position = _camera.transform.position;
            float camera_half_height = _camera.orthographicSize;
            float camera_half_width = _camera.aspect * camera_half_height;

            return fruit_position.x > camera_position.x + camera_half_width + _cameraPadding
                || fruit_position.x < camera_position.x - camera_half_width - _cameraPadding
                || fruit_position.y > camera_position.y + camera_half_height + _cameraPadding
                || fruit_position.y < camera_position.y - camera_half_height - _cameraPadding;
        }

        private void Update()
        {
            foreach (Fruit fruit in _fruits)
            {
                if (IsOutOfBound(fruit))
                {
                    fruit.Destroy();
                }
            }
        }

        private void Start()
        {
            _camera = Camera.main;
            _fruits.AddRange(FindObjectsByType<Fruit>(FindObjectsSortMode.None));
        }
    }
}
