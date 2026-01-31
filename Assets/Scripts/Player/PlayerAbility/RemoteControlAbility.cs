using UnityEngine;
using UnityEngine.Events;

public class RemoteControlAbility : AbilityBase
{
    [Header("能力3设置")]
    public float flightSpeed = 10f;
    
    // 用于暂时禁用玩家本体移动的事件
    public UnityEvent onControlStart; 
    public UnityEvent onControlEnd;

    public override void TriggerAbility()
    {
        isAbilityActive = true;
        
        // 1. 禁用玩家本体控制
        onControlStart?.Invoke();

        // 2. 生成飞行物
        GameObject proj = Instantiate(PrefabLibrary.Instance.FlyMaskPrefab, playerObj.transform.position, Quaternion.identity);
        
        // 3. 初始化飞行物
        var controller = proj.GetComponent<ControlledProjectile>();
        if(controller) controller.Setup(this, flightSpeed);
    }

    // 由飞行物碰撞后通过SendMessage或公开方法调用
    public void OnProjectileHit(GameObject enemy)
    {
        ExecuteEnemy(enemy);
    }

    protected override void FinishAbility()
    {
        base.FinishAbility();
        // 恢复玩家控制
        onControlEnd?.Invoke();
    }
}
