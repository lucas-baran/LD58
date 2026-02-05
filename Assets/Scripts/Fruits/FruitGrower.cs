using LD58.Levels;
using LD58.Players;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace LD58.Fruits
{
    public class FruitGrower : Singleton<FruitGrower>
    {
        [SerializeField] private List<FruitData> _startingFruits = new();
        [SerializeField] private float _cameraPadding = 1f;
        [SerializeField] private Fruit _fruitPrefab;
        [SerializeField] private FruitEffectManager _fruitEffectManager;

        private readonly Queue<Fruit> _fruitQueue = new();
        private readonly List<Fruit> _activeFruits = new();
        private readonly List<GrowSpot> _growSpots = new();

        private LevelFruitDeck _fruitDeck;
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
            GrowFilledSpots(out NativeList<int> free_spots);
            PlantFruitsFromDeck(ref free_spots);
            PlantDefaultFruits(ref free_spots);
            free_spots.Dispose();
        }

        private void GrowFilledSpots(out NativeList<int> free_spots)
        {
            int spot_count = _growSpots.Count;
            free_spots = new(initialCapacity: spot_count, Allocator.Temp);

            for (int spot_index = 0; spot_index < spot_count; spot_index++)
            {
                GrowSpot grow_spot = _growSpots[spot_index];

                if (grow_spot.IsFree)
                {
                    free_spots.AddNoResize(spot_index);
                }
                else
                {
                    grow_spot.Fruit.Grow();
                }
            }
        }

        private void PlantFruitsFromDeck(ref NativeList<int> free_spots)
        {
            while (free_spots.Length > 0 && _fruitDeck.TryGetRandomFruit(out FruitData fruit_data))
            {
                int random_index = Random.Range(0, free_spots.Length);
                GrowSpot grow_spot = _growSpots[free_spots[random_index]];
                grow_spot.Fruit = GetFruit(fruit_data);
                grow_spot.ReturnToDeck = true;
                free_spots.RemoveAt(random_index);
            }
        }

        private void PlantDefaultFruits(ref NativeList<int> free_spots)
        {
            foreach (int spot_index in free_spots)
            {
                GrowSpot grow_spot = _growSpots[spot_index];
                grow_spot.Fruit = GetFruit(GetRandomStartingFruit());
                grow_spot.ReturnToDeck = false;
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

        public void Initialize(LevelData level_data)
        {
            _fruitDeck = new LevelFruitDeck(Player.Instance.FruitDeck.Fruits);
            CreateGrowSpots(level_data);
            GrowFruits();
            GrowFruitsUntilTheyCanCollide();
        }

        private void GrowFruitsUntilTheyCanCollide()
        {
            foreach (GrowSpot grow_spot in _growSpots)
            {
                if (!grow_spot.Fruit.Data.HasCollisions)
                {
                    grow_spot.Fruit.Grow();
                }
            }
        }

        private void CreateGrowSpots(LevelData level_data)
        {
            for (int spot_index = 0; spot_index < level_data.GrowSpots.Count; spot_index++)
            {
                LevelData.GrowSpot spot_info = level_data.GrowSpots[spot_index];
                Vector3 position = new(spot_info.Position.x, spot_info.Position.y, 0f);
                _growSpots.Add(new GrowSpot(position, _fruitEffectManager, _fruitDeck));
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
            private readonly FruitEffectManager _fruitEffectManager;
            private readonly LevelFruitDeck _fruitDeck;
            private readonly Vector3 _position;
            private Fruit _fruit;
            private FruitData _initialFruitData;

            public bool ReturnToDeck { get; set; }
            public bool IsFree => _fruit == null;
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
                        _fruit.OnDetach -= Fruit_OnDetach;
                    }

                    _fruit = value;

                    if (_fruit != null)
                    {
                        _initialFruitData = _fruit.Data;
                        SetFruitPosition(value);
                        _fruit.OnDetach += Fruit_OnDetach;
                    }
                }
            }

            public GrowSpot(Vector3 position, FruitEffectManager fruit_effect_manager, LevelFruitDeck fruit_deck)
            {
                _position = position;
                _fruitEffectManager = fruit_effect_manager;
                _fruitDeck = fruit_deck;
            }

            private void SetFruitPosition(Fruit fruit)
            {
                Vector3 position = new(_position.x, _position.y, fruit.transform.position.z);
                fruit.transform.SetPositionAndRotation(position, Quaternion.identity);
            }

            private void Fruit_OnDetach()
            {
                if (ReturnToDeck)
                {
                    _fruitDeck.Return(_initialFruitData);
                    _initialFruitData = null;
                    ReturnToDeck = false;
                }

                if (_fruit.Data.OnDetachEffectPrefab != null)
                {
                    _fruitEffectManager.ExecuteEffect(_fruit, _fruit.Data.OnDetachEffectPrefab);
                }

                Fruit = null;
            }
        }
    }
}
