using UnityEngine;
using System.Collections;
using Management.Tag;



public class Actions : MonoBehaviour
{

    [Header("近战攻击设置")]
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private GameObject attackColliderPrefab;
    [Header("射击设置")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform bulletSpawnPoint;

    private bool isAttacking = false;
    private bool canShoot = true;

    public void PerformMeleeAttack(Transform attacker, float attackRadius, float attackWidth,
        float attackAngle, float attackDuration, LayerMask enemyLayer, bool isBlunk)
    {
        if (isAttacking) return;

        StartCoroutine(MeleeAttackCoroutine(attacker, attackRadius, attackWidth,
            attackAngle, attackDuration, enemyLayer, isBlunk));
    }
    public void PerformShoot(Transform shooter)
    {
        if (!canShoot) return;

        StartCoroutine(ShootCoroutine(shooter));
    }

    private IEnumerator MeleeAttackCoroutine(Transform attacker, float attackRadius, float attackWidth,
     float attackAngle, float attackDuration, LayerMask enemyLayer, bool isBlunk)
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

        GameObject attackCollider = CreateAttackCollider(attackPosition, attacker.rotation, attackRadius, attackWidth, isBlunk);

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

            yield return null;
        }

        if (attackCollider != null)
        {
            Destroy(attackCollider);
        }

        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }
    private GameObject CreateAttackCollider(Vector3 position, Quaternion rotation, float attackRadius, float attackWidth, bool isBlunk)
    {

        GameObject colliderObj = Instantiate(attackColliderPrefab, position, rotation);
        BoxCollider2D boxCollider = colliderObj.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.size = new Vector2(attackRadius, attackWidth);
        }
        Melee melee = colliderObj.GetComponent<Melee>();
        melee.isblunk = isBlunk;
        return colliderObj;


    }

    private IEnumerator ShootCoroutine(Transform shooter)
    {
        canShoot = false;
        float shootCooldown = 0.5f;

        Vector3 spawnPosition = bulletSpawnPoint != null ? bulletSpawnPoint.position : shooter.position;


        GameObject bullet = CreateBullet(spawnPosition, shooter.rotation);



        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }


    private GameObject CreateBullet(Vector3 position, Quaternion rotation)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, position, rotation);
            return bullet;
        }
        return null;
    }



    public void PerformSpreadShot(Transform shooter)
    {
        if (!canShoot) return;

        StartCoroutine(SpreadShotCoroutine(shooter));
    }

    private IEnumerator SpreadShotCoroutine(Transform shooter)
    {
        canShoot = false;
        float shootCooldown = 1f;

        float spreadRange = 0.5f;

        for (int i = 0; i < 10; i++)
        {

            float randomDisableTime = Random.Range(0.05f, 0.3f);

            float randomAngleOffset = Random.Range(-15f, 15f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, shooter.eulerAngles.z + randomAngleOffset);


            Vector3 forward = shooter.up;
            Vector3 right = shooter.right;


            // float distanceForward = Random.Range(0.7f, 0.9f);
            // float distanceRight = Random.Range(-spreadRange, spreadRange);

            float distanceForward = 0.8f;
            float distanceRight = 0;

            Vector3 randomPosition = shooter.position + forward * distanceRight + right * distanceForward;


            CreateSpreadBullet(randomPosition, randomRotation, randomDisableTime);


            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }



    private GameObject CreateSpreadBullet(Vector3 position, Quaternion rotation, float disableTime = 0.1f)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, position, rotation);

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDisableTime(disableTime);
            }

            return bullet;
        }
        return null;
    }

    public void PerformRiffleShot(Transform shooter)
    {
        if (!canShoot) return;

        StartCoroutine(ShootRiffleCoroutine(shooter));
    }

    private IEnumerator ShootRiffleCoroutine(Transform shooter)
    {
        canShoot = false;
        float shootCooldown = 0.2f;

        Vector3 spawnPosition = bulletSpawnPoint != null ? bulletSpawnPoint.position : shooter.position;

        float randomAngleOffset = Random.Range(-10f, 10f);
        Quaternion randomRotation = Quaternion.Euler(0, 0, shooter.eulerAngles.z + randomAngleOffset);


        CreateBullet(spawnPosition, randomRotation);



        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }


}
