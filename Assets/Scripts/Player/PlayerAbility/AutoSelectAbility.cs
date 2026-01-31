using Management;
using UnityEngine;

public class AutoSelectAbility : AbilityBase
{
    [Header("能力2设置")]
    public float searchRadius = 150f;
    public LayerMask enemyLayer = 3;

    public override void TriggerAbility()
    {
        var enemies = GameManager.Instance.EnemyList;

        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in enemies)
        {
            Collider2D col = obj.GetComponent<Collider2D>();
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
