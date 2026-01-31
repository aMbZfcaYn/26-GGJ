using UnityEngine;
using System.Collections;



public class Actions : MonoBehaviour
{
    [SerializeField] private float attackDuration = 0.2f;
    [Header("近战攻击设置")]
    [SerializeField] private GameObject attackColliderPrefab;

    private bool isAttacking = false;

    public void PerformMeleeAttack(Transform attacker, float attackRadius, float attackWidth,
        float attackAngle, float attackDuration, LayerMask enemyLayer)
    {
        if (isAttacking) return;

        StartCoroutine(MeleeAttackCoroutine(attacker, attackRadius, attackWidth,
            attackAngle, attackDuration, enemyLayer));
    }

    private IEnumerator MeleeAttackCoroutine(Transform attacker, float attackRadius, float attackWidth,
        float attackAngle, float attackDuration, LayerMask enemyLayer)
    {
        isAttacking = true;

        Vector2 startDirection = new Vector2(
            Mathf.Cos(attacker.eulerAngles.z * Mathf.Deg2Rad),
            Mathf.Sin(attacker.eulerAngles.z * Mathf.Deg2Rad)
        );

        float startAngle = attacker.eulerAngles.z - attackAngle / 2f;
        float currentAngle = startAngle;
        float targetAngle = startAngle + attackAngle;


        Vector3 attackPosition = attacker.position + (Vector3)(startDirection * attackRadius / 2);
        Debug.Log("生成攻击");
        GameObject attackCollider = CreateAttackCollider(attackPosition, attacker.rotation, attackRadius, attackWidth);
        Debug.Log("生成攻击成功");
        float progress = 0f;
        float totalRotationTime = attackDuration;

        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / totalRotationTime);


            currentAngle = Mathf.Lerp(startAngle, targetAngle, progress);


            float radian = currentAngle * Mathf.Deg2Rad;
            Vector2 currentDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            if (attackCollider != null)
            {
                attackCollider.transform.position = attacker.position + (Vector3)(currentDirection * attackRadius / 2);
                attackCollider.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }


            Vector3 detectionPosition = attacker.position + (Vector3)(currentDirection * attackRadius / 2);
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(detectionPosition,
                new Vector2(attackRadius, attackWidth),
                currentAngle,
                enemyLayer);


            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("攻击到敌人: " + enemy.name);
            }

            yield return null;
        }

        if (attackCollider != null)
        {
            Destroy(attackCollider);
        }

        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }

    private GameObject CreateAttackCollider(Vector3 position, Quaternion rotation, float attackRadius, float attackWidth)
    {
        if (attackColliderPrefab != null)
        {
            GameObject colliderObj = Instantiate(attackColliderPrefab, position, rotation);
            return colliderObj;
        }
        else
        {
            GameObject tempCollider = new GameObject("TempAttackCollider");
            BoxCollider2D boxCollider = tempCollider.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(attackRadius, attackWidth);
            boxCollider.isTrigger = true;
            tempCollider.transform.position = position;
            tempCollider.transform.rotation = rotation;


            Destroy(tempCollider, attackDuration + 0.1f);

            return tempCollider;
        }
    }


}