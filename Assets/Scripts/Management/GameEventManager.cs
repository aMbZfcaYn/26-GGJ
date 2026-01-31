using UnityEngine;
using UnityEngine.Events;

namespace Management
{
    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance { get; private set; }

        public PossessionEvent onPossessionTrigger = new PossessionEvent();
        public LevelFailEvent onLevelFail = new LevelFailEvent();
        public LevelClearEvent onLevelClear = new LevelClearEvent();

        private void Awake()
        {
            if (Instance is not null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <para>
    /// first: Target
    /// second: Player
    /// </para>
    [System.Serializable]
    public class PossessionEvent : UnityEvent<GameObject, GameObject>
    {
    }

    [System.Serializable]
    public class LevelFailEvent : UnityEvent
    {
    }

    [System.Serializable]
    public class LevelClearEvent : UnityEvent
    {
    }
}