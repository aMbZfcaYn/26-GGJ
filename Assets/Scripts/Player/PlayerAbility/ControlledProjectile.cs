using InputNamespace;
using Management.Tag;
using UnityEngine;

// 挂在生成的飞行物Prefab上
public class ControlledProjectile : MonoBehaviour
{
    public Transform initialTransform;
    
    private RemoteControlAbility ownerAbility;
    private float speed;

    public void Setup(RemoteControlAbility owner, float moveSpeed)
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
        Debug.Log(collision.name);

        var tag = collision.GetComponent<Taggable>();
        
        if (Taggable.HasTag(collision.gameObject, TagManager.GetTag("Enemy")))
        {
            // 通知能力脚本处理结果
            ownerAbility.OnProjectileHit(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
