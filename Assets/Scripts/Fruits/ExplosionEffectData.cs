using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitEffectData_Explosion", menuName = "LD58/Fruits/Effects/Explosion")]
    public sealed class ExplosionEffectData : FruitEffectData
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private int _damage = 10_000;

        public override void Execute(Fruit fruit)
        {
            var active_fruits = FruitGrower.Instance.ActiveFruits;
            Vector3 center = fruit.transform.position;

            for (int fruit_index = active_fruits.Count - 1; fruit_index >= 0; fruit_index--)
            {
                Fruit active_fruit = active_fruits[fruit_index];

                if (active_fruit == fruit
                    || !active_fruit.Data.HasCollisions
                    )
                {
                    continue;
                }

                float distance = Vector3.Distance(active_fruit.transform.position, center) + active_fruit.Data.ColliderSize / 2f;

                if (distance < _radius)
                {
                    active_fruit.Damage(_damage);
                }
            }
        }
    }
}
