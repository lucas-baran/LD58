using UnityEngine;
using UnityEngine.UI;

namespace LD58.Enemies
{
    public sealed class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Image _healthBarForeground;

        private void RefreshHealthBar()
        {
            _healthBarForeground.fillAmount = _enemy.Health.HealthPercent;
        }

        private void Start()
        {
            _enemy.Health.OnHealthChanged.AddListener(RefreshHealthBar);
            RefreshHealthBar();
        }
    }
}
