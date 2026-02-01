using Management;
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
    private GameObject Shooter;
    private Taggable shootertaggable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDisableTime = 0f;
        disableTime = defaultDisableTime;
    }

    public void changeShooter(GameObject newShooter)
    {
        Shooter = newShooter;
        shootertaggable = Shooter.GetComponent<Taggable>();
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
            Debug.Log("Shooter Tag: " + shootertaggable.HasTag(TagManager.GetTag("Player")));

            GameObject enemyObject = other.gameObject;
            Taggable taggable = enemyObject.GetComponent<Taggable>();
            if (shootertaggable.HasTag(TagManager.GetTag("Player")))
            {
                if (taggable && taggable.HasTag(TagManager.GetTag("Enemy")))
                {
                    Debug.Log("子弹击中敌人: " + other.name);
                    EnemyFSM otherFSM = other.GetComponent<EnemyFSM>();
                    otherFSM.TransitionState(new Dead(otherFSM));
                    DestroyBullet();
                    GameEventManager.Instance.onEnemyKilled.Invoke(other.gameObject);
                    taggable.TryRemoveTag(TagUtils.Type_Enemy);
                }
            }
            else if (shootertaggable.HasTag(TagManager.GetTag("Enemy")))
            {
                if (taggable && taggable.HasTag(TagManager.GetTag("Player")))
                {
                    Debug.Log("子弹击中玩家: " + other.name);
                    // PlayerFSM otherFSM = other.GetComponent<PlayerFSM>();
                    // otherFSM.TransitionState(new Dead(otherFSM));
                    DestroyBullet();
                    GameManager.Instance.playerHp--;
                    if (GameManager.Instance.playerHp > 0)
                    {
                        GameEventManager.Instance.onPossessionTrigger.Invoke(Shooter, other.gameObject);
                    }
                    else
                    {
                        GameEventManager.Instance.onLevelFail.Invoke();
                    }
                }
            }
            // DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}