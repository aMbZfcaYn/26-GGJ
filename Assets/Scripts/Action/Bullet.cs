using UnityEngine;
using Management.Tag;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float defaultDisableTime = 0f; // 默认禁用时间

    private float currentLifeTime;
    private float currentDisableTime;
    private float disableTime; // 实际使用的禁用时间
    private Rigidbody2D rb;
    private bool canFly = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDisableTime = 0f;
        disableTime = defaultDisableTime;
    }


    public void SetDisableTime(float newDisableTime)
    {
        disableTime = newDisableTime;
    }

    void FixedUpdate()
    {
        currentLifeTime += Time.deltaTime;

        if (!canFly)
        {
            currentDisableTime += Time.deltaTime;
            if (currentDisableTime >= disableTime)
            {
                canFly = true;
            }
        }
        else
        {
            rb.linearVelocity = transform.right * speed;
        }

        if (currentLifeTime >= lifeTime)
        {
            DestroyBullet();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canFly)
        {
            GameObject enemyObject = other.gameObject;
            Taggable taggable = enemyObject.GetComponent<Taggable>();
            if (taggable != null && taggable.HasTag(TagManager.GetTag("Enemy")))
            {
                Debug.Log("子弹击中敌人: " + other.name);
                EnemyFSM otherFSM = other.GetComponent<EnemyFSM>();
                otherFSM.TransitionState(new Dead(otherFSM));
                DestroyBullet();
            }
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}