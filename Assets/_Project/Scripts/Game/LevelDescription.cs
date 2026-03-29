using LD58.Common;
using UnityEngine;

namespace LD58.Game
{
    [CreateAssetMenu(fileName = "SO_LevelDescription", menuName = CreateAssetMenuItems.LEVELS + "Level description")]
    public sealed class LevelDescription : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _picture;

        public string Name => _name;
        public Sprite Picture => _picture;
    }
}
