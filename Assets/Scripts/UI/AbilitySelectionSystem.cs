using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using InputNamespace;
using Management;
using UnityEngine.UI;

public class AbilitySelectionSystem : MonoBehaviour
{
    [Header("配置")] [SerializeField] private List<AbilityData> abilities; // 拖入你的数据
    [SerializeField] private GameObject cardPrefab; // 拖入制作好的Prefab
    [SerializeField] private Transform cardContainer; // 放置卡片的父物体

    [Header("UI 引用")] [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private CanvasGroup mainCanvasGroup; // 整个面板的CanvasGroup
    [SerializeField] private HorizontalLayoutGroup layoutGroup;

    private List<AbilityCard> _spawnedCards = new List<AbilityCard>();
    private int _currentSelectedIndex = -1;
    private bool _isLocked = true; // 锁定操作防止连点

    private void Start()
    {
        descriptionText.alpha = 0; // 初始隐藏描述
        SpawnCards();

        LayoutRebuilder.ForceRebuildLayoutImmediate(cardContainer as RectTransform);

        // 播放入场动画
        float delay = 0.1f;
        for (int i = 0; i < _spawnedCards.Count; i++)
        {
            _spawnedCards[i].PlayEntrance(i * delay);
        }

        // 动画播完后解锁输入
        DOVirtual.DelayedCall(0.8f, () => _isLocked = false);
    }

    private void SpawnCards()
    {
        if (layoutGroup)
            layoutGroup.enabled = true;

        // 清理旧的
        foreach (Transform child in cardContainer) Destroy(child.gameObject);
        _spawnedCards.Clear();

        // 生成新的
        for (int i = 0; i < abilities.Count; i++)
        {
            AbilityCard card = Instantiate(cardPrefab, cardContainer).GetComponent<AbilityCard>();
            card.Setup(abilities[i], i, OnCardClicked);
            _spawnedCards.Add(card);
        }
    }

    private void OnCardClicked(int index)
    {
        if (_isLocked) return;

        // 逻辑：如果点击的是当前已经选中的 -> 确认
        if (_currentSelectedIndex == index)
        {
            ConfirmSelection(index);
        }
        // 逻辑：点击了别的 -> 切换选中
        else
        {
            ChangeSelection(index);
        }
    }

    private void ChangeSelection(int index)
    {
        _currentSelectedIndex = index;

        // 1. 更新所有卡片状态
        for (int i = 0; i < _spawnedCards.Count; i++)
        {
            _spawnedCards[i].UpdateVisualState(i == index, true);
        }

        // 2. 更新描述文本 (先淡出旧的，改字，再淡入新的)
        descriptionText.DOKill();
        descriptionText.DOFade(0, 0.1f).OnComplete(() =>
        {
            descriptionText.text = abilities[index].description;
            descriptionText.DOFade(1, 0.2f);
        });
    }

    private void ConfirmSelection(int index)
    {
        _isLocked = true; // 锁定输入

        // 1. 播放卡片确认动画
        _spawnedCards[index].PlayConfirmEffect(() =>
        {
            // 2. 动画结束，通知 GameManager
            //GameManager.Instance.StartGameWithAbility(abilities[index].id);

            GameManager.Instance.playerAbilityIndex = index + 1;
            GameEventManager.Instance.onLevelStart.Invoke();
            InputManager.SetGameInputState(true);

            // 3. 整个 UI 淡出消失
            mainCanvasGroup.DOFade(0, 0.5f).OnComplete(() => { gameObject.SetActive(false); });
        });
    }
}