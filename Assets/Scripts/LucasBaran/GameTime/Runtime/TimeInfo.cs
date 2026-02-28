using System.Runtime.CompilerServices;

namespace LucasBaran.GameTime
{
    public sealed class TimeInfo
    {
        public float TimeScale { get; internal set; }
        public float Time { get; internal set; }
        public float DeltaTime { get; internal set; }
        public float SmoothDeltaTime { get; internal set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Update()
        {
            DeltaTime = TimeScale * UnityEngine.Time.deltaTime;
            SmoothDeltaTime = TimeScale * UnityEngine.Time.smoothDeltaTime;
            Time += DeltaTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UpdateWithParent(TimeInfo parent)
        {
            DeltaTime = TimeScale * parent.DeltaTime;
            SmoothDeltaTime = TimeScale * parent.SmoothDeltaTime;
            Time += DeltaTime;
        }
    }
}
