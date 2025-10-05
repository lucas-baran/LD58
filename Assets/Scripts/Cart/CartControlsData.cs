using UnityEngine;

namespace LD58.Cart
{
    [CreateAssetMenu(fileName = "SO_CartControlsData", menuName = "LD58/Cart/Controls Data")]
    public class CartControlsData : ScriptableObject
    {
        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private float _levelBoundsPadding = 2f;
        [SerializeField] private float _aimSpeed = 50f;
        [SerializeField] private float _maxAimAngle = 50f;
        [SerializeField] private float _switchFruitTime = 0.5f;

        public float MovementSpeed => _movementSpeed;
        public float LevelBoundsPadding => _levelBoundsPadding;
        public float AimSpeed => _aimSpeed;
        public float MaxAimAngle => _maxAimAngle;
        public float SwitchFruitTime => _switchFruitTime;
    }
}
