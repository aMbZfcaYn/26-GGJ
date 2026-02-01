using InputNamespace;
using Management;
using Management.Tag;
using UnityEngine;

public class SelectAbility : AbilityBase
{
    [Header("能力1设置")] public float slowMotionScale = 0.1f;

    private int cnt = 0;

    public override void TriggerAbility()
    {
        isAbilityActive = true;
        TimeManager.Instance.SetTimeScale(slowMotionScale);
    }

    private void Update()
    {
        if (!isAbilityActive)
        {
            cnt = 0;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);
            
            foreach(var enemy in GameManager.Instance.EnemyList)
            {
                var pos = enemy.transform.position;
                if(pos.x - 0.5 <= mousePos.x && mousePos.x <= pos.x + 0.5 &&
                   pos.y - 0.5 <= mousePos.y && mousePos.y <= pos.y + 0.5)
                {
                    ExecuteEnemy(enemy);
                    return;
                }
            }
        }

        if (InputManager.SkillWasPressed && cnt > 0)
            FinishAbility();
        cnt++;
    }

    protected override void FinishAbility()
    {
        base.FinishAbility();
        //TimeManager.Instance.SetTimeScale(1); // 恢复时间
    }
}