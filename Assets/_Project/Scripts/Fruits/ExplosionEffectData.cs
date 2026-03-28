using LD58.Common;
using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitEffectData_Explosion", menuName = CreateAssetMenuItems.FRUIT_EFFECTS + "Explosion")]
    public sealed class ExplosionEffectData : ScriptableObject
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private int _damage = 10_000;

        public float Radius => _radius;
        public int Damage => _damage;
    }
}
