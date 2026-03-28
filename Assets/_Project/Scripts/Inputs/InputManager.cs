using UnityEngine;

namespace LD58.Inputs
{
    public static class InputManager
    {
        private static Inputs_LD58 _instance;

        public static bool HasInstance => _instance != null;
        public static Inputs_LD58 Instance => _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _instance = new Inputs_LD58();
            _instance.Enable();

            Application.quitting += Application_quitting;
        }

        private static void Application_quitting()
        {
            Application.quitting -= Application_quitting;

            if (_instance != null)
            {
                _instance.Disable();
                _instance.Dispose();
                _instance = null;
            }
        }
    }
}
