using UnityEngine;
using System.Collections;
using Management.Tag;

public enum WeaponType
{
    knife, sword, hammer, Spear, magic_single, magic_spread, magic_riffle, magic_magic
}

public class Actions : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private GameObject attackColliderPrefab;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Knife Settings")]
    [SerializeField] private float attackRadius_knife = 0.5f;
    [SerializeField] private float attackWidth_knife = 0.3f;
    [SerializeField] private float attackAngle_knife = 90f;
    [SerializeField] private float attackDuration_knife = 0.2f;

    [Header("Sword Settings")]
    [SerializeField] private float attackRadius_sword = 1f;
    [SerializeField] private float attackWidth_sword = 0.3f;
    [SerializeField] private float attackAngle_sword = 90f;
    [SerializeField] private float attackDuration_sword = 0.2f;

    [Header("Hammer Settings")]
    [SerializeField] private float attackRadius_hammer = 1.5f;
    [SerializeField] private float attackWidth_hammer = 0.3f;
    [SerializeField] private float attackAngle_hammer = 90f;
    [SerializeField] private float attackDuration_hammer = 0.2f;

    [Header("Spear Settings")]
    [SerializeField] private float attackRadius_spear = 2f;
    [SerializeField] private float attackWidth_spear = 0.3f;
    [SerializeField] private float attackDuration_spear = 0.2f;

    [Header("Magic Melee Settings")]
    [SerializeField] private float attackRadius_magic = 0.5f;
    [SerializeField] private float attackWidth_magic = 0.3f;
    [SerializeField] private float attackAngle_magic = 90f;
    [SerializeField] private float attackDuration_magic = 0.2f;

    [Header("Magic Single Shot Settings")]
    [SerializeField] private float shootCooldown_single = 0.5f;

    [Header("Magic Spread Shot Settings")]
    [SerializeField] private float shootCooldown_spread = 1f;
    [SerializeField] private int spreadBulletsCount = 10;
    [SerializeField] private float spreadAngleRange = 15f;
    [SerializeField] private float spreadFireRate = 0.02f;
    [SerializeField] private float spreadDistance = 0.8f;

    [Header("Magic Rifle Settings")]
    [SerializeField] private float shootCooldown_rifle = 0.2f;
    [SerializeField] private float rifleAngleRange = 10f;

    [SerializeField] private LayerMask enemyLayer;

    private bool isAttacking = false;
    private bool canShoot = true;
    private WeaponType currentWeaponType = WeaponType.knife;

    public void SetCurrentWeapon(WeaponType weaponType)
    {
        currentWeaponType = weaponType;
    }

    public void PerformMeleeAttack(Transform attacker, bool isBlunk = false)
    {
        if (isAttacking) return;

        float attackRadius, attackWidth, attackAngle, attackDurationValue;

        switch (currentWeaponType)
        {
            case WeaponType.knife:
                attackRadius = attackRadius_knife;
                attackWidth = attackWidth_knife;
                attackAngle = attackAngle_knife;
                attackDurationValue = attackDuration_knife;
                break;
            case WeaponType.sword:
                attackRadius = attackRadius_sword;
                attackWidth = attackWidth_sword;
                attackAngle = attackAngle_sword;
                attackDurationValue = attackDuration_sword;
                break;
            case WeaponType.hammer:
                attackRadius = attackRadius_hammer;
                attackWidth = attackWidth_hammer;
                attackAngle = attackAngle_hammer;
                attackDurationValue = attackDuration_hammer;
                break;
            case WeaponType.magic_magic: // Magic melee
                attackRadius = attackRadius_magic;
                attackWidth = attackWidth_magic;
                attackAngle = attackAngle_magic;
                attackDurationValue = attackDuration_magic;
                isBlunk = true; // Magic melee defaults to blunt
                break;
            default:
                attackRadius = 0.5f;
                attackWidth = 0.3f;
                attackAngle = 90f;
                attackDurationValue = 0.2f;
                break;
        }

        StartCoroutine(MeleeAttackCoroutine(attacker, attackRadius, attackWidth,
            attackAngle, attackDurationValue, enemyLayer, isBlunk));
    }

    public void PerformSpearAttack(Transform attacker)
    {
        if (isAttacking) return;

        float attackDistance = attackRadius_spear;
        float attackWidth = attackWidth_spear;
        float attackDurationValue = attackDuration_spear;

        StartCoroutine(SpearAttackCoroutine(attacker, attackDistance, attackWidth,
            attackDurationValue, enemyLayer, false));
    }

    public void PerformShoot(Transform shooter)
    {
        if (!canShoot) return;

        StartCoroutine(ShootCoroutine(shooter));
    }

    public void PerformSpreadShot(Transform shooter)
    {
        if (!canShoot) return;

        StartCoroutine(SpreadShotCoroutine(shooter));
    }

    public void PerformRiffleShot(Transform shooter)
    {
        if (!canShoot) return;

        StartCoroutine(ShootRiffleCoroutine(shooter));
    }

    private IEnumerator MeleeAttackCoroutine(Transform attacker, float attackRadius, float attackWidth,
     float attackAngle, float attackDurationValue, LayerMask enemyLayer, bool isBlunk)
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
        float totalRotationTime = attackDurationValue;

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

    private IEnumerator SpearAttackCoroutine(Transform attacker, float attackDistance, float attackWidth,
        float attackDurationValue, LayerMask enemyLayer, bool isBlunk)
    {
        isAttacking = true;

        Vector2 direction = new Vector2(
            Mathf.Cos(attacker.eulerAngles.z * Mathf.Deg2Rad),
            Mathf.Sin(attacker.eulerAngles.z * Mathf.Deg2Rad)
        );

        Vector3 startPosition = attacker.position;
        Vector3 endPosition = attacker.position + (Vector3)(direction * attackDistance);

        GameObject attackCollider = CreateSpearAttackCollider(startPosition + (Vector3)(direction * attackDistance / 2),
            attacker.rotation, attackDistance, attackWidth, isBlunk);

        float progress = 0f;
        float totalMoveTime = attackDurationValue;

        while (progress < 0.4f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / (totalMoveTime * 0.4f));

            if (attackCollider != null)
            {
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, progress);
                attackCollider.transform.position = newPosition + (Vector3)(direction * attackDistance / 2);
            }

            yield return null;
        }

        float stayTime = totalMoveTime * 0.2f;
        yield return new WaitForSeconds(stayTime);

        float returnProgress = 0f;
        while (returnProgress < 1f)
        {
            returnProgress = Mathf.Clamp01(returnProgress + Time.deltaTime / (totalMoveTime * 0.4f));

            if (attackCollider != null)
            {
                Vector3 newPosition = Vector3.Lerp(endPosition, startPosition, returnProgress);
                attackCollider.transform.position = newPosition + (Vector3)(direction * attackDistance / 2);
            }

            yield return null;
        }

        if (attackCollider != null)
        {
            Destroy(attackCollider);
        }

        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
    }

    private GameObject CreateSpearAttackCollider(Vector3 position, Quaternion rotation,
        float attackDistance, float attackWidth, bool isBlunk)
    {
        GameObject colliderObj = Instantiate(attackColliderPrefab, position, rotation);
        BoxCollider2D boxCollider = colliderObj.GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            boxCollider.size = new Vector2(attackDistance, attackWidth);
        }

        Melee melee = colliderObj.GetComponent<Melee>();
        melee.isblunk = isBlunk;

        return colliderObj;
    }

    private IEnumerator ShootCoroutine(Transform shooter)
    {
        canShoot = false;

        Vector3 spawnPosition = bulletSpawnPoint != null ? bulletSpawnPoint.position : shooter.position;

        GameObject bullet = CreateBullet(spawnPosition, shooter.rotation);

        yield return new WaitForSeconds(shootCooldown_single);
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

    private IEnumerator SpreadShotCoroutine(Transform shooter)
    {
        canShoot = false;

        Vector3 forward = shooter.up;
        Vector3 right = shooter.right;

        for (int i = 0; i < spreadBulletsCount; i++)
        {
            float randomDisableTime = Random.Range(0.05f, 0.3f);

            float randomAngleOffset = Random.Range(-spreadAngleRange, spreadAngleRange);
            Quaternion randomRotation = Quaternion.Euler(0, 0, shooter.eulerAngles.z + randomAngleOffset);

            Vector3 randomPosition = shooter.position + forward * spreadDistance;

            CreateSpreadBullet(randomPosition, randomRotation, randomDisableTime);

            yield return new WaitForSeconds(spreadFireRate);
        }

        yield return new WaitForSeconds(shootCooldown_spread);
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

    private IEnumerator ShootRiffleCoroutine(Transform shooter)
    {
        canShoot = false;

        Vector3 spawnPosition = bulletSpawnPoint != null ? bulletSpawnPoint.position : shooter.position;

        float randomAngleOffset = Random.Range(-rifleAngleRange, rifleAngleRange);
        Quaternion randomRotation = Quaternion.Euler(0, 0, shooter.eulerAngles.z + randomAngleOffset);

        CreateBullet(spawnPosition, randomRotation);

        yield return new WaitForSeconds(shootCooldown_rifle);
        canShoot = true;
    }
}