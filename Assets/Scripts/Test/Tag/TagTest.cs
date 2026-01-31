using Management.Tag;
using UnityEngine;

namespace Test.Tag
{
    public class TagTest : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Taggable taggable = GetComponent<Taggable>();
            taggable.TryRemoveTag(TagManager.GetTag("Enemy"));
            taggable.TryAddTag(TagManager.GetTag("Player"));
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
