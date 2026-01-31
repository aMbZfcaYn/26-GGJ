using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Management.Tag
{
    [CreateAssetMenu(fileName = "TagLibrary", menuName = "Tag System/Tag Library")]
    public class TagLibrary : ScriptableObject
    {
        public List<GameTag> allTags = new();
        [System.NonSerialized] private Dictionary<string, GameTag> _tagLookupMap;

        protected void OnEnable()
        {
            BuildLookupMap();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            BuildLookupMap();
        }
#endif

        private void BuildLookupMap()
        {
            if (allTags is null || allTags.Count == 0)
            {
                _tagLookupMap = new Dictionary<string, GameTag>();
                return;
            }

            try
            {
                _tagLookupMap = allTags.ToDictionary(
                    tag => GetNormalizedTagKey(tag.TagName),
                    tag => tag
                );
            }
            catch (System.ArgumentException ex)
            {
                Debug.LogError($"[TagLibrary] 存在重复的 Tag 定义，无法创建查找字典: {ex.Message}");
                _tagLookupMap = new Dictionary<string, GameTag>();
            }
        }

        private static string GetNormalizedTagKey(string tagName)
        {
            return $"{tagName.Trim().ToUpperInvariant()}";
        }

        public GameTag GetTag(string tagName)
        {
            string lookupKey = GetNormalizedTagKey(tagName);
            if (_tagLookupMap is not null) return _tagLookupMap.GetValueOrDefault(lookupKey);
            BuildLookupMap();
            return _tagLookupMap?.GetValueOrDefault(lookupKey);
        }
    }
}