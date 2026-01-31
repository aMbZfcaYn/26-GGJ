using UnityEngine;

namespace Management.Tag
{
    [CreateAssetMenu(fileName = "New GameTag", menuName = "Tag System/GameTag")]
    public class GameTag : ScriptableObject
    {
        public string TagName = "Tag";
    }
}
