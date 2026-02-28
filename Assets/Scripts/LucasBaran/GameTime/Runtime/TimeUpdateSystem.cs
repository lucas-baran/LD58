using LD58;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace LucasBaran.GameTime
{
    internal static class TimeUpdateSystem
    {
        [GeneratedTime] internal static readonly TimeInfo MasterTime = new();
        [GeneratedTime] internal static readonly TimeInfo UITime = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RegisterSubsystem()
        {
            PlayerLoopSystem system = new()
            {
                type = typeof(TimeUpdateSystem),
                updateDelegate = UpdateTimeInfos,
                subSystemList = null,
            };

            PlayerLoopUtilities.InsertSystemAfter<TimeUpdate.WaitForLastPresentationAndUpdateTime>(ref system);
        }

        private static void UpdateTimeInfos()
        {
            MasterTime.Update();
            UITime.UpdateWithParent(MasterTime);
        }
    }
}
