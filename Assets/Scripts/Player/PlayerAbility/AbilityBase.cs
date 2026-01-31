using Management;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityBase : MonoBehaviour
{
    [Header("通用设置")]
    // 这里是你已经实现的处决事件
    public UnityEvent<GameObject> onEnemyExecuted; 

    protected bool isAbilityActive = false;
    protected GameObject playerObj;

    public virtual void Initialize(GameObject player)
    {
        playerObj = player;
    }

    // 外部（输入系统）调用的入口
    public abstract void TriggerAbility();

    // 所有能力最终都会调用这个方法
    protected void ExecuteEnemy(GameObject enemy)
    {
        if (enemy == null) return;
        
        Debug.Log($"对敌人 {enemy.name} 执行了特殊操作");
        GameEventManager.Instance.onPossessionTrigger.Invoke(enemy, playerObj);
        
        // 执行完后的清理工作（如恢复时间、重置状态）
        FinishAbility();
    }

    protected virtual void FinishAbility()
    {
        isAbilityActive = false;
    }
}
