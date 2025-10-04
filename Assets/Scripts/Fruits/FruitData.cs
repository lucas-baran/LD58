using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitData", menuName = "LD58/Fruits/Fruit data")]
    public class FruitData : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private int _value = 1;
        [SerializeField] private Sprite _sprite;

        public string Name => _name;
        public int Value => _value;
        public Sprite Sprite => _sprite;
    }
}
