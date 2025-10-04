using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitData", menuName = "LD58/Fruits/Fruit data")]
    public class FruitData : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private List<GrowStep> _growSteps;

        public string Name => _name;
        public int MaxGrow => _growSteps.Count - 1;
        public IReadOnlyList<GrowStep> GrowSteps => _growSteps;

        public sealed class GrowStep
        {
            [SerializeField] private int _value = 1;
            [SerializeField] private Sprite _sprite;
            [SerializeField] private bool _hasCollisions;

            public int Value => _value;
            public Sprite Sprite => _sprite;
            public bool HasCollisions => _hasCollisions;
        }
    }
}
