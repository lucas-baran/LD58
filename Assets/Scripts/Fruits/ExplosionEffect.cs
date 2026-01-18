using UnityEngine;

namespace LD58.Fruits
{
    public sealed class ExplosionEffect : FruitEffect
    {
        [SerializeField] private ExplosionEffectData _data;

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

                if (distance < _data.Radius)
                {
                    active_fruit.Damage(_data.Damage);
                }
            }
        }
    }
}
