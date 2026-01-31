using System.Collections.Generic;
using UnityEngine;

namespace Management.Tag
{
    public class Taggable : MonoBehaviour
    {
        [SerializeField] private List<GameTag> tags;

        public bool HasTag(GameTag tagToCompare)
        {
            return tags.Contains(tagToCompare);
        }

        public bool TryAddTag(GameTag tagToAdd)
        {
            if (tagToAdd is null || HasTag(tagToAdd)) return false;
            tags.Add(tagToAdd);
            return true;
        }

        public bool TryRemoveTag(GameTag tagToRemove)
        {
            if (tagToRemove is null || !HasTag(tagToRemove)) return false;
            tags.Remove(tagToRemove);
            return true;
        }

        public static bool HasTag(GameObject obj, GameTag tagToCompare)
        {
            if (!obj) return false;
            if (!obj.TryGetComponent<Taggable>(out var taggable))
            {
                Debug.LogError($"Error: Querying Untaggable {obj.name} for {tagToCompare.name}");
                return false;
            }

            return taggable.HasTag(tagToCompare);
        }
    }
}