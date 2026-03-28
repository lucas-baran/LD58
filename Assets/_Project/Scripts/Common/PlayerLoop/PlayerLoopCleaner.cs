namespace LD58
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal static class PlayerLoopCleaner
    {
        private static readonly List<Type> _registeredSystems = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            Application.quitting -= Application_quitting;
            Application.quitting += Application_quitting;
        }

        public static void RegisterForCleanUp(Type system_type)
        {
            _registeredSystems.Add(system_type);
        }

        private static void Application_quitting()
        {
            foreach (Type system_type in _registeredSystems)
            {
                PlayerLoopUtilities.RemoveSystem(system_type);
            }

            _registeredSystems.Clear();
        }
    }
}
