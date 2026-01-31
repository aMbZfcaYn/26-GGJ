using InputNamespace;
using UnityEngine;

// 挂在生成的飞行物Prefab上
public class ControlledProjectile : MonoBehaviour
{
    private AbilityBase ownerAbility;
    private float speed;

    public void Setup(AbilityBase owner, float moveSpeed)
    {
        ownerAbility = owner;
        speed = moveSpeed;
    }

    void FixedUpdate()
    {
        transform.Translate(InputManager.Movement * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 假设敌人的Tag是Enemy
        if (collision.CompareTag("TestEnemy"))
        {
            // 通知能力脚本处理结果
            ownerAbility.SendMessage("OnProjectileHit", collision.gameObject);
            Destroy(gameObject);
        }
    }
}
