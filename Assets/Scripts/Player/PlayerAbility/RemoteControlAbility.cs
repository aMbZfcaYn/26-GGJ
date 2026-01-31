using Player;
using UnityEngine;
using UnityEngine.Events;

public class RemoteControlAbility : AbilityBase
{
    [Header("能力3设置")]
    public float flightSpeed = 10f;

    private PlayerControl playerScript;

    public override void TriggerAbility()
    {
        isAbilityActive = true;
        
        // 1. 禁用玩家本体控制
        playerScript = playerObj.GetComponent<PlayerControl>();
        playerScript.blocked = true;

        // 2. 生成飞行物
        GameObject proj = Instantiate(PrefabLibrary.Instance.FlyMaskPrefab, playerObj.transform.position, Quaternion.identity);
        
        // 3. 初始化飞行物
        var controller = proj.GetComponent<ControlledProjectile>();
        if(controller) controller.Setup(this, flightSpeed);
        controller.initialTransform = transform;
        
        var camScript = Camera.main.GetComponent<CameraControl>();
        camScript.target = proj.transform;
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
        playerScript.blocked = false;
    }
}
