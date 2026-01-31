using UnityEngine;
using Utilities;

namespace Management.Tag
{
    public class TagManager : GenericSingleton<TagManager>
    {
        public TagLibrary tagLibrary;

        public static GameTag GetTag(string tagName)
        {
            if (Instance?.tagLibrary) return Instance.tagLibrary.GetTag(tagName);
            Debug.LogError("TagManager or TagLibrary is not initialized!");
            return null;
        }
    }
}