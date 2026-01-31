using UnityEngine;

namespace Utilities
{
    public class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance is not null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this as T;
                // DontDestroyOnLoad(this.gameObject);
                Initialize();
            }
        }
        
        protected virtual void Initialize() { }
    }
}
