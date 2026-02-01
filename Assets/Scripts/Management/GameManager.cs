using System.Collections.Generic;
using Management.SceneManage;
using Management.Tag;
using Unity.Collections;
using UnityEngine;
using Utilities;

namespace Management
{
    public class GameManager : GenericSingleton<GameManager>
    {
        [Header("UI")] public GameObject abilityCanvas;

        [Header("Possession")] [SerializeField]
        private float possessionEnergy;

        [SerializeField] private float possessionMaxEnergy;
        [Range(1, 10)] [SerializeField] private float possessionReduceRate;
        [Range(1, 2000)] [SerializeField] private float possessionGain;
        [ReadOnly] [SerializeField] private bool isEnergyFilled;

        [Space(20)] [Header("Scenes")] [SerializeField]
        private int sceneIndex = 0;

        [SerializeField] [SceneName]
        [Tooltip("Write scene names in sequence.")]
        private List<string> scenes;

        public HashSet<GameObject> EnemyList = new();

        [Space(20)] [Header("Player")] [SerializeField]
        public GameObject player;

        public int playerAbilityIndex;

        [SerializeField] public int playerHp;
        public float PossessionEnergy => possessionEnergy;
        public float PossessionMaxEnergy => possessionMaxEnergy;

        private void OnDisable()
        {
            GameEventManager.Instance?.onPossessionEnd.RemoveListener(PossessionDone);
            GameEventManager.Instance?.onLevelStart.RemoveListener(LevelEnter);
            GameEventManager.Instance?.onLevelStart.RemoveListener(LevelStart);
            GameEventManager.Instance?.onLevelFail.RemoveListener(LevelFail);
            GameEventManager.Instance?.onLevelClear.RemoveListener(LevelClear);
            GameEventManager.Instance?.onLevelFinish.RemoveListener(LevelFinish);
            GameEventManager.Instance?.onEnemyKilled.RemoveListener(PossessionEnergyGain);
            GameEventManager.Instance?.onEnemyKilled.RemoveListener(EnemyKilled);
        }

        private void FixedUpdate()
        {
            // Would tell possession manager that possession is allowed. 
            isEnergyFilled = possessionEnergy >= possessionMaxEnergy;

            // Reduce possession energy per frame
            if (possessionEnergy > 0)
            {
                possessionEnergy -= possessionReduceRate;
            }

            if (possessionEnergy < 0)
            {
                possessionEnergy = 0;
            }
        }

        /// <summary>
        /// Public func for check if energy is filled to allow possession.
        /// </summary>
        /// <returns>True for filled, false for unfilled</returns>
        public bool IsEnergyFilled()
        {
            return isEnergyFilled;
        }

        public void StartGame()
        {
            EnemyList.Clear();
            SceneController.Instance.LoadScene(scenes[sceneIndex]);
        }

        /// <summary>
        /// Public func for making enemy list
        /// </summary>
        /// <param name="enemy">Enemy need to be added</param>
        public void RegisterEnemy(GameObject enemy)
        {
            EnemyList.Add(enemy);
        }

        /// <summary>
        /// Public func for change player
        /// </summary>
        /// <param name="newPlayer"></param>
        public void RegisterPlayer(GameObject newPlayer, bool first = false)
        {
            if (player == newPlayer) return;
            Taggable taggable = newPlayer.GetComponent<Taggable>();
            if (!taggable) return;
            // Special check for init register
            if (first)
            {
                player = newPlayer;
                taggable.TryAddTag(TagUtils.Type_Player);
                taggable.TryRemoveTag(TagUtils.Type_Enemy);
            }

            if (taggable.HasTag(TagUtils.Type_Player) || !taggable.HasTag(TagUtils.Type_Enemy))
            {
                Debug.LogError($"Unexpected Register to {newPlayer.name}");
                return;
            }

            player = newPlayer;
            taggable.TryAddTag(TagUtils.Type_Player);
            taggable.TryRemoveTag(TagUtils.Type_Enemy);
        }

        /// <summary>
        /// Event func of event: onLevelEnter
        /// </summary>
        public void LevelEnter()
        {
            // TODO: choose ability
            abilityCanvas.SetActive(true);
            Debug.Log("Level Enter");
        }

        /// <summary>
        /// Event func of event: onLevelStart
        /// </summary>
        public void LevelStart()
        {
            // May Optimize: better enemy list clear logic
            EnemyList.Clear();
            SceneController.Instance.LoadScene(scenes[sceneIndex]);
        }

        /// <summary>
        /// Event func of event: onLevelFail
        /// </summary>
        public void LevelFail()
        {
            // May change: reload after input
            // restart level
            GameEventManager.Instance.onLevelEnter.Invoke();
        }

        /// <summary>
        /// Event func of event: onLevelClear
        /// </summary>
        public void LevelClear()
        {
            // TODO: 
            Debug.Log("Level Clear");
        }

        /// <summary>
        /// Event func of event: onLevelFinish
        /// </summary>
        public void LevelFinish()
        {
            // add index and it will be used when start new level
            ++sceneIndex;
            GameEventManager.Instance.onLevelEnter.Invoke();
        }

        /// <summary>
        /// Event func of event: onPossessionEnd
        /// </summary>
        public void PossessionDone()
        {
            possessionEnergy = 0;
        }

        /// <summary>
        /// Event func of event: onEnemyKilled
        /// </summary>
        /// <param name="killed">Enemy that be killed</param>
        public void PossessionEnergyGain(GameObject killed)
        {
            possessionEnergy += possessionGain;
            Debug.Log("Possession Energy Gain: " + possessionEnergy);
        }

        /// <summary>
        /// Event func of event: onEnemyKilled
        /// </summary>
        /// <param name="killed">Enemy that be killed</param>
        public void EnemyKilled(GameObject killed)
        {
            EnemyList.Remove(killed);
            if (EnemyList.Count == 0)
            {
                GameEventManager.Instance.onLevelClear.Invoke();
            }

            Debug.Log("Enemy killed: " + killed.name);
        }
    }
}