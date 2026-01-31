using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Management
{
    public class GameEventManager : GenericSingleton<GameEventManager>
    {
        public PossessionEvent onPossessionTrigger = new();
        public PossessionEndEvent onPossessionEnd = new();
        public LevelFailEvent onLevelFail = new();
        public LevelClearEvent onLevelClear = new();
        public EnemyKilled onEnemyKilled = new();
        public EnergyFilled onEnergyFilled = new();
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
    public class PossessionEndEvent : UnityEvent
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

    /// <summary>
    /// 
    /// </summary>
    /// <para>
    /// first: be killed enemy
    /// </para>
    [System.Serializable]
    public class EnemyKilled : UnityEvent<GameObject>
    {
    }

    [System.Serializable]
    public class EnergyFilled : UnityEvent
    {
    }
}