using UnityEngine;

namespace Management.Tag
{
    public class TagManager : MonoBehaviour
    {
        public static TagManager Instance { get; private set; }

        public TagLibrary tagLibrary;

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

        public static GameTag GetTag(string tagName)
        {
            if (Instance?.tagLibrary is not null) return Instance.tagLibrary.GetTag(tagName);
            Debug.LogError("TagManager or TagLibrary is not initialized!");
            return null;
        }
    }
}