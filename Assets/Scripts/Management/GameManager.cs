using System.Collections.Generic;
using Management.SceneManage;
using UnityEngine;
using Utilities;

namespace Management
{
    public class GameManager : GenericSingleton<GameManager>
    {
        [Header("Possession")] [SerializeField]
        private float possessionEnergy;

        [SerializeField] private float possessionMaxEnergy;
        [Range(1, 10)] [SerializeField] private float possessionReduceRate;

        private bool _possessionReduceAllowed = true;

        [Space(20)] [Header("Scenes")] [SerializeField]
        private int sceneIndex;

        [SerializeField] [TextArea(2, 10)] [Tooltip("Write scene names in sequence.")]
        private List<string> scenes;
        
        public HashSet<GameObject> EnemyList = new();

        public GameObject player;

        private void OnDisable()
        {
            GameEventManager.Instance?.onPossessionEnd.RemoveListener(PossessionDone);
            GameEventManager.Instance?.onEnemyKilled.RemoveListener(PossessionEnergyGain);
            GameEventManager.Instance?.onLevelClear.RemoveListener(LevelClear);
            GameEventManager.Instance?.onLevelFail.RemoveListener(LevelFail);
            GameEventManager.Instance?.onLevelFinish.RemoveListener(LevelFinish);
        }

        private void FixedUpdate()
        {
            // Would tell possession manager that possession is allowed. 
            if (possessionEnergy >= possessionMaxEnergy)
            {
                GameEventManager.Instance.onEnergyFilled.Invoke();
                possessionEnergy = possessionMaxEnergy;
                _possessionReduceAllowed = false;
            }

            // Reduce possession energy per frame
            if (_possessionReduceAllowed && possessionEnergy > 0)
            {
                possessionEnergy -= possessionReduceRate;
            }

            if (_possessionReduceAllowed && possessionEnergy < 0)
            {
                possessionEnergy = 0;
            }
        }

        public void StartGame()
        {
            // TODO: 
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
        /// Event func of event: onLevelClear
        /// </summary>
        public void LevelClear()
        {
            // TODO: 
        }

        /// <summary>
        /// Event func of event: onLevelFail
        /// </summary>
        public void LevelFail()
        {
            // TODO: 
        }

        /// <summary>
        /// Event func of event: onLevelFinish
        /// </summary>
        public void LevelFinish()
        {
            EnemyList.Clear();
            ++sceneIndex;
            SceneController.Instance.LoadScene(scenes[sceneIndex]);
        }

        /// <summary>
        /// Event func of event: onPossessionEnd
        /// </summary>
        public void PossessionDone()
        {
            possessionEnergy = 0;
            _possessionReduceAllowed = true;
        }

        /// <summary>
        /// Event func of event: onEnemyKilled
        /// </summary>
        /// <param name="killed">Enemy that be killed</param>
        public void PossessionEnergyGain(GameObject killed)
        {
            // TODO: 
        }

        /// <summary>
        /// Event func of event: onEnemyKilled
        /// </summary>
        /// <param name="killed">Enemy that be killed</param>
        public void EnemyKilled(GameObject killed)
        {
            EnemyList.Remove(killed);
        }
    }
}