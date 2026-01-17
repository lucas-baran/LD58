using LD58.Levels;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LD58.Fruits
{
    public class FruitGrower : Singleton<FruitGrower>
    {
        [SerializeField] private List<FruitData> _startingFruits = new();
        [SerializeField] private float _cameraPadding = 1f;
        [SerializeField] private Fruit _fruitPrefab;

        private readonly Queue<Fruit> _fruitQueue = new();
        private readonly List<Fruit> _activeFruits = new();
        private readonly List<GrowSpot> _growSpots = new();

        private Camera _camera;

        public IReadOnlyList<Fruit> ActiveFruits => _activeFruits;

        public Fruit GetFruit(FruitData fruit_data)
        {
            if (!_fruitQueue.TryDequeue(out Fruit fruit))
            {
                fruit = Instantiate(_fruitPrefab);
            }

            _activeFruits.Add(fruit);
            fruit.gameObject.SetActive(true);
            fruit.Initialize(fruit_data);

            return fruit;
        }

        public void Destroy(Fruit fruit)
        {
            fruit.gameObject.SetActive(false);
            _activeFruits.Remove(fruit);
            _fruitQueue.Enqueue(fruit);
        }

        public void GrowFruits()
        {
            foreach (GrowSpot grow_spot in _growSpots)
            {
                if (grow_spot.Fruit == null)
                {
                    grow_spot.Fruit = GetFruit(GetRandomStartingFruit());
                }
                else
                {
                    grow_spot.Fruit.Grow();
                }

                grow_spot.Fruit.transform.SetPositionAndRotation(grow_spot.Position, Quaternion.identity);
            }
        }

        public bool IsAnyFruitMoving()
        {
            foreach (Fruit fruit in _activeFruits)
            {
                if (fruit.IsMoving)
                {
                    return true;
                }
            }

            return false;
        }

        public void SpawnFruits(LevelData level_data)
        {
            for (int i = 0; i < level_data.StartingFruits.Count; i++)
            {
                LevelData.FruitPosition fruit_position = level_data.StartingFruits[i];
                Fruit fruit = GetFruit(fruit_position.FruitData);
                fruit.transform.position = new Vector3(fruit_position.Position.x, fruit_position.Position.y, fruit.transform.position.z);
                _growSpots.Add(new GrowSpot(fruit));
            }
        }

        private FruitData GetRandomStartingFruit()
        {
            return _startingFruits[Random.Range(0, _startingFruits.Count)];
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
            for (int fruit_index = _activeFruits.Count - 1; fruit_index >= 0; fruit_index--)
            {
                Fruit fruit = _activeFruits[fruit_index];

                if (IsOutOfBound(fruit))
                {
                    Destroy(fruit);
                }
            }
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private sealed class GrowSpot
        {
            private Fruit _fruit;

            public readonly Vector3 Position;
            public Fruit Fruit
            {
                get => _fruit;
                set
                {
                    if (value == _fruit)
                    {
                        return;
                    }

                    if (_fruit != null)
                    {
                        _fruit.OnFall -= Fruit_OnFall;
                    }

                    _fruit = value;

                    if (_fruit != null)
                    {
                        _fruit.OnFall += Fruit_OnFall;
                    }
                }
            }

            public GrowSpot(Fruit fruit)
            {
                Position = fruit.transform.position;
                Fruit = fruit;
            }

            private void Fruit_OnFall()
            {
                Fruit = null;
            }
        }
    }
}
