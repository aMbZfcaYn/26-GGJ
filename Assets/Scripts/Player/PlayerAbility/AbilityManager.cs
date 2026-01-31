using Management;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    // 可以在Inspector中拖入三个不同的脚本组件（如果它们挂在子物体上）
    // 或者用代码AddComponent动态添加
    private AbilityBase currentAbility;
    public bool needSwitcher = false;

    private void Start()
    {
        GameEventManager.Instance.onLevelStart.AddListener(SelectAbility);
    }

    // 供UI界面调用：传入 1, 2, 3 来选择
    public void SelectAbility()
    {
        Debug.Log("Set ability" + GameManager.Instance.playerAbilityIndex);
        int abilityIndex = GameManager.Instance.playerAbilityIndex;
        // 先移除旧能力（如果有）
        if (currentAbility) Destroy(currentAbility);

        switch (abilityIndex)
        {
            case 1:
                needSwitcher = true;
                currentAbility = gameObject.AddComponent<SelectAbility>();
                break;
            case 2:
                currentAbility = gameObject.AddComponent<AutoSelectAbility>();
                break;
            case 3:
                currentAbility = gameObject.AddComponent<RemoteControlAbility>();
                break;
        }

        // 初始化能力
        if (currentAbility)
        {
            currentAbility.Initialize(gameObject);

            // 绑定事件（假设你有一个全局的EventManager或者直接在这里写逻辑）
            // currentAbility.onEnemyExecuted.AddListener(MyGameLogic.OnEnemyDie);
        }
    }

    public void OnAbilityButtonPressed()
    {
        if (currentAbility)
        {
            currentAbility.TriggerAbility();
        }
    }
}