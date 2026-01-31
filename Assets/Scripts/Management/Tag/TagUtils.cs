using UnityEngine;

namespace Management.Tag
{
    public static class TagUtils
    {
        public static GameTag Type_Enemy => TagManager.GetTag("Enemy");
        public static GameTag Type_Player => TagManager.GetTag("Player");
        public static GameTag Type_AttckEntity => TagManager.GetTag("AttckEntity");
        public static GameTag Type_MaskBullet => TagManager.GetTag("MaskBullet");
    }
}
