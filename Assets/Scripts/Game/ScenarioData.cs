using UnityEngine;

namespace LD58.Game
{
    [CreateAssetMenu(fileName = "SO_ScenarioData", menuName = "LD58/Game/Scenario data")]
    public sealed class ScenarioData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _picture;
        [SerializeField] private SceneReference _sceneReference;

        public string Name => _name;
        public Sprite Picture => _picture;
        public SceneReference SceneReference => _sceneReference;
    }
}
