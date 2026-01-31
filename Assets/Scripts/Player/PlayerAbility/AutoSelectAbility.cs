using UnityEngine;

public class AutoSelectAbility : AbilityBase
{
    [Header("能力2设置")]
    public float searchRadius = 15f;
    public LayerMask enemyLayer;

    public override void TriggerAbility()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerObj.transform.position, searchRadius, enemyLayer);

        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D col in enemies)
        {
            float dist = Vector2.Distance(playerObj.transform.position, col.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestEnemy = col.gameObject;
            }
        }

        if (nearestEnemy != null)
        {
            // 可以在这里加一些特效，比如瞬移过去或者发射光束
            ExecuteEnemy(nearestEnemy);
        }
        else
        {
            Debug.Log("范围内没有敌人");
            FinishAbility();
        }
    }
}
