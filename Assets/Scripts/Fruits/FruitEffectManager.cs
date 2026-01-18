using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    public class FruitEffectManager : MonoBehaviour
    {
        private readonly Dictionary<Type, FruitEffect> _fruitEffects = new();

        public void ExecuteEffect(
            Fruit fruit,
            FruitEffect fruit_effect_prefab
            )
        {
            FruitEffect fruit_effect = GetFruitEffectInstance(fruit_effect_prefab);
            fruit_effect.Execute(fruit);
        }

        private FruitEffect GetFruitEffectInstance(
            FruitEffect fruit_effect_prefab
            )
        {
            Type type = fruit_effect_prefab.GetType();

            if (!_fruitEffects.TryGetValue(type, out FruitEffect effect_instance))
            {
                effect_instance = Instantiate(fruit_effect_prefab);
                _fruitEffects[type] = effect_instance;
            }

            return effect_instance;
        }
    }
}
