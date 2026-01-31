using InputNamespace;
using UnityEngine;

public class SelectAbility : AbilityBase
{
    [Header("能力1设置")]
    public float slowMotionScale = 0.1f;

    public override void TriggerAbility()
    {
        isAbilityActive = true;
        TimeManager.Instance.SetTimeScale(slowMotionScale);
        // 这里可以添加代码显示鼠标光标
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!isAbilityActive) return;
    
        Debug.Log("SelectAbility Update");
        
        // 检测鼠标左键点击 (假设使用旧Input系统，新Input System同理)
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);
            
            Debug.Log(hit.collider);
            
            if (hit.collider != null && hit.collider.CompareTag("TestEnemy"))
            {
                ExecuteEnemy(hit.collider.gameObject);
            }
        }
        
        if(InputManager.SkillWasPressed)
            FinishAbility();
    }

    protected override void FinishAbility()
    {
        base.FinishAbility();
        TimeManager.Instance.SetTimeScale(1); // 恢复时间
        Cursor.visible = false; // 隐藏光标
    }
}
