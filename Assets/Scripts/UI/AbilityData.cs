using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Game/Ability Data")]
public class AbilityData : ScriptableObject
{
    public int id; // 传递给 GameManager 的 ID
    public string abilityName; // 名字
    public Sprite icon; // 图标图片
    [TextArea] public string description; // 描述文本
}