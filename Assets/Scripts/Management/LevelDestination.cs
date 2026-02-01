using Management.Tag;
using UnityEngine;

namespace Management
{
    public class LevelDestination : MonoBehaviour
    {
        public bool isLevelCleared;

        private void Awake()
        {
            GameEventManager.Instance.onLevelClear.AddListener(LevelClear);
        }

        /// <summary>
        /// Event func of event: onLevelClear
        /// </summary>
        public void LevelClear()
        {
            isLevelCleared = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Entered " + other.name);
            if (isLevelCleared && other.gameObject.GetComponent<Taggable>().HasTag(TagUtils.Type_Player))
            {
                Debug.Log("Level cleared and Player in");
                GameEventManager.Instance.onLevelFinish.Invoke();
            }
        }
    }
}