using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Levels
{
    [CreateAssetMenu(fileName = "SO_LevelData", menuName = "LD58/Levels/Level data")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private List<GrowSpot> _growSpots = new();

        public IReadOnlyList<GrowSpot> GrowSpots => _growSpots;

        [Serializable]
        public sealed class GrowSpot
        {
            [SerializeField] private Vector2 _position;

            public Vector2 Position => _position;
        }
    }
}
