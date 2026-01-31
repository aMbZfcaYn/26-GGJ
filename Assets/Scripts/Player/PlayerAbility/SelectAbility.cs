using InputNamespace;
using Management;
using UnityEngine;

public class SelectAbility : AbilityBase
{
    [Header("能力1设置")] public float slowMotionScale = 0.1f;

    private int cnt = 0;

    public override void TriggerAbility()
    {
        isAbilityActive = true;
        TimeManager.Instance.SetTimeScale(slowMotionScale);

        var enemies = GameManager.Instance.EnemyList;
        foreach (GameObject enemy in enemies)
        {
            var setmaterial = enemy.GetComponent<EnemySetMaterial>();
            setmaterial.enabled = true;
        }
    }

    private void Update()
    {
        if (!isAbilityActive)
        {
            cnt = 0;
            return;
        }

        Debug.Log("SelectAbility Update");
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);

            Debug.Log(hit.collider);

            if (hit.collider && hit.collider.CompareTag("TestEnemy"))
            {
                ExecuteEnemy(hit.collider.gameObject);
            }
        }

        if (InputManager.SkillWasPressed && cnt > 0)
            FinishAbility();
        cnt++;
    }

    protected override void FinishAbility()
    {
        base.FinishAbility();
        TimeManager.Instance.SetTimeScale(1); // 恢复时间

        var enemies = GameManager.Instance.EnemyList;
        foreach (GameObject enemy in enemies)
        {
            var setmaterial = enemy.GetComponent<EnemySetMaterial>();
            setmaterial.SetSelected(false);
            setmaterial.enabled = false;
        }
    }
}