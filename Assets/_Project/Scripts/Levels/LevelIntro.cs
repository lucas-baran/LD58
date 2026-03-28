using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace LD58.Levels
{
    public abstract class LevelIntro : MonoBehaviour
    {
        public abstract UniTask PlayAsync(CancellationToken cancellation_token);
    }
}
