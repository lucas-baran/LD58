using UnityEngine;

namespace LD58
{
    public abstract class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        private static T _instance;

        public static bool HasInstance => _instance != null;
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            _instance = (T)this;
        }
    }
}
