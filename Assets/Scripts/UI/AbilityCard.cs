using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class AbilityCard : MonoBehaviour
{
    [Header("UI 绑定")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject highlightBorder; // 选中时显示的高亮框

    [Header("动画参数")]
    [SerializeField] private float selectScale = 1.1f; // 选中放大的倍数
    [SerializeField] private float normalScale = 1.0f;
    [SerializeField] private float dimScale = 0.9f;    // 未选中缩小的倍数
    [SerializeField] private float dimAlpha = 0.5f;    // 未选中变暗的透明度

    private int _index;
    private Action<int> _onClickCallback;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(AbilityData data, int index, Action<int> onClick)
    {
        _index = index;
        _onClickCallback = onClick;

        if (data != null)
        {
            //iconImage.sprite = data.icon;
            nameText.text = data.abilityName;
        }

        // 初始化状态：隐藏边框，透明度设为0（为了入场动画）
        //highlightBorder.SetActive(false);
        _canvasGroup.alpha = 0f;
    }

    // 点击事件绑定到按钮上
    public void OnClick()
    {
        _onClickCallback?.Invoke(_index);
    }

    // --- 动画方法 ---

    // 1. 入场动画
    public void PlayEntrance(float delay)
    {
        // 此时卡片已经在正确的位置了（因为我们在System里强制刷新了布局）
        // 先记录这个“终点位置”
        Vector2 finalPos = _rectTransform.anchoredPosition;

        // 手动把卡片往下移 150 (作为起点)
        _rectTransform.anchoredPosition = finalPos + Vector2.down * 150f;

        // 动画：移动回 finalPos
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        // 这里不要用 SetRelative，直接移动到终点最稳
        seq.Append(_rectTransform.DOAnchorPos(finalPos, 0.5f).SetEase(Ease.OutBack)); 
        seq.Join(_canvasGroup.DOFade(1f, 0.5f));
    }

    // 2. 状态更新（我是被选中的那个吗？还是陪跑的？）
    public void UpdateVisualState(bool isSelected, bool hasSelectionMade)
    {
        //highlightBorder.SetActive(isSelected);

        if (isSelected)
        {
            // 选中：变大，完全不透明
            transform.DOScale(selectScale, 0.3f).SetEase(Ease.OutBack);
            _canvasGroup.DOFade(1f, 0.3f);
        }
        else if (hasSelectionMade)
        {
            // 没选中我，但已经有人被选中了：变小，变半透明
            transform.DOScale(dimScale, 0.3f);
            _canvasGroup.DOFade(dimAlpha, 0.3f);
        }
        else
        {
            // 没人被选中（恢复默认）：正常大小
            transform.DOScale(normalScale, 0.3f);
            _canvasGroup.DOFade(1f, 0.3f);
        }
    }

    // 3. 确认选择时的冲击动画
    public void PlayConfirmEffect(Action onComplete)
    {
        // 震动一下
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1)
            .OnComplete(() => onComplete?.Invoke());
    }
}