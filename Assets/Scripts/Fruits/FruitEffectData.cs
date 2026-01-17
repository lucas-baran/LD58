using UnityEngine;

namespace LD58.Fruits
{
    public abstract class FruitEffectData : ScriptableObject
    {
        public abstract void Execute(Fruit fruit);
    }
}
