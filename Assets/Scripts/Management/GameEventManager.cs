using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Management
{
    public class GameEventManager : GenericSingleton<GameEventManager>
    {
        public PossessionEvent onPossessionTrigger = new();
        public PossessionEndEvent onPossessionEnd = new();
        public LevelEnterEvent onLevelEnter = new();
        public LevelStartEvent onLevelStart = new();
        public LevelFailEvent onLevelFail = new();
        public LevelClearEvent onLevelClear = new();
        public LevelFinishEvent onLevelFinish = new();
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

    /// <summary>
    /// When first enter level.
    /// Will do ability choose and other things.
    /// Only execute once for one level.
    /// </summary>
    [System.Serializable]
    public class LevelEnterEvent : UnityEvent
    {
    }

    /// <summary>
    /// Execute everytime when start a level.
    /// May execute multiple times.
    /// </summary>
    [System.Serializable]
    public class LevelStartEvent : UnityEvent
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

    [System.Serializable]
    public class LevelFinishEvent : UnityEvent
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