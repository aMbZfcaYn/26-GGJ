using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using Management;

public class EnergyStateController : MonoBehaviour
{
    [System.Serializable]
    public class EnergyType
    {
        public string typeName; // 方便在编辑器查看
        public List<Sprite> states; // 该类型下的所有状态图（按能量从低到高排列）
    }

    [Header("配置数据")]
    [SerializeField] private List<EnergyType> energyLibrary; // 存储所有类型的图像库
    [SerializeField] private Image displayImage; // 显示图像的 UI 组件

    [Header("动画设置")]
    [SerializeField] private float transitionDuration = 0.2f;
    [SerializeField] private Vector3 punchScale = new Vector3(1.2f, 1.2f, 1.2f);

    private int currentTypeIndex = 0;
    private int lastSpriteIndex = -1;

    private void Start()
    {
        GameEventManager.Instance.onLevelStart.AddListener(InitLevelType);
    }

    /// <summary>
    /// 进入关卡时初始化类型
    /// </summary>
    /// <param name="typeIndex">关卡指定的图像类型索引</param>
    public void InitLevelType()
    {
        int typeIndex = GameManager.Instance.playerAbilityIndex - 1;
        
        if (typeIndex < 0 || typeIndex >= energyLibrary.Count)
        {
            Debug.LogError("Invalid Type Index!");
            return;
        }
        currentTypeIndex = typeIndex;
        lastSpriteIndex = -1; // 重置索引以便强制更新第一张图
    }

    /// <summary>
    /// 实时更新能量（建议在属性变化时调用，或在 Update 中调用）
    /// </summary>
    /// <param name="current">当前能量</param>
    /// <param name="max">最大能量</param>
    public void UpdateEnergy(float current, float max)
    {
        if (energyLibrary.Count == 0) return;

        float percentage = Mathf.Clamp01(current / max);
        var currentSprites = energyLibrary[currentTypeIndex].states;
        int stateCount = currentSprites.Count;

        if (stateCount == 0) return;

        // 计算当前应该显示哪一张图片
        // 例如：4张图，百分比0.5，index = floor(0.5 * 3.99) = 1
        int spriteIndex = Mathf.FloorToInt(percentage * (stateCount - 0.001f));

        // 只有当图片索引发生变化时，才执行切换逻辑和动画
        if (spriteIndex != lastSpriteIndex)
        {
            ChangeSprite(currentSprites[spriteIndex]);
            lastSpriteIndex = spriteIndex;
        }
    }

    private void ChangeSprite(Sprite newSprite)
    {
        // 使用 DOTween 制作切换效果
        // 1. 瞬间切换图片
        displayImage.sprite = newSprite;

        // 2. 播放一个简单的缩放效果，让玩家感知到变化
        displayImage.transform.DOKill(); // 停止之前的动画
        displayImage.transform.localScale = Vector3.one; // 重置缩放
        displayImage.transform.DOPunchScale(punchScale, transitionDuration, 5, 1);

        // 3. 可选：做一个颜色闪烁效果
        displayImage.DOColor(Color.white, 0.1f).From(Color.gray);
    }

    // --- 测试用代码 (按键模拟) ---
    [Header("测试模拟")]
    public float testEnergy = 100;
    void Update()
    {
        // 实时调用更新
        UpdateEnergy(GameManager.Instance.PossessionEnergy, GameManager.Instance.PossessionMaxEnergy);

        // 模拟加减能量
        if (Input.GetKey(KeyCode.UpArrow)) testEnergy += Time.deltaTime * 50;
        if (Input.GetKey(KeyCode.DownArrow)) testEnergy -= Time.deltaTime * 50;
        testEnergy = Mathf.Clamp(testEnergy, 0, 100);
    }
}