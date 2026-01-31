using UnityEngine;
using UnityEngine.VFX;

namespace LD58.Fruits
{
    public sealed class ExplosionEffect : FruitEffect
    {
        [SerializeField] private ExplosionEffectData _data;
        [SerializeField] private VisualEffect _vfx;
        [SerializeField] private VFXGraphicsBufferProperties _vfxProperties;

        private VFXGraphicsBuffer<Vector4> _positionBuffer;

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

            _positionBuffer.AddData(new Vector4(center.x, center.y, center.z, _data.Radius));
        }

        private void Awake()
        {
            _positionBuffer = new VFXGraphicsBuffer<Vector4>(_vfx, capacity: 32, stride: 16, _vfxProperties, auto_resize: true, subsystem_type: typeof(ExplosionEffect));
        }

        private void OnDestroy()
        {
            _positionBuffer.Dispose();
        }
    }
}
