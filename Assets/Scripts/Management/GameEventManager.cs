using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Management
{
    public class GameEventManager : GenericSingleton<GameEventManager>
    {
        public PossessionEvent onPossessionTrigger = new PossessionEvent();
        public LevelFailEvent onLevelFail = new LevelFailEvent();
        public LevelClearEvent onLevelClear = new LevelClearEvent();
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