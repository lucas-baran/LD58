using Cysharp.Threading.Tasks;
using LD58.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LD58.Levels
{
    public sealed class TutorialIntro : LevelIntro
    {
        [SerializeField] private List<UITutorialPanel> _panels = new();

        public override async UniTask PlayAsync(CancellationToken cancellation_token)
        {
            for (int i = 0; i < _panels.Count; i++)
            {
                await _panels[i].WaitForSubmitAsync(cancellation_token);
            }

            gameObject.SetActive(false);
        }
    }
}
