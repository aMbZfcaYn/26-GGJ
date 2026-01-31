using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Actions actions;

    // [SerializeField] private float attackRadius = 1.5f;
    // [SerializeField] private float attackWidth = 0.3f;
    // [SerializeField] private float attackAngle = 90f;
    // [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private WeaponType currentWeaponType = WeaponType.knife;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 movement = InputManager.Movement;
        rb.linearVelocity = movement * moveSpeed;

        RotateTowardsMouse();
    }

    void Update()
    {

        if (InputManager.DefaultAttackWasPressed)
        {

            switch (currentWeaponType)
            {
                case WeaponType.knife:
                    PerformMeleeAttack(1.5f, 0.3f, 90f, 0.2f, enemyLayer, false);
                    break;
                case WeaponType.magic_single:
                    PerformShoot();
                    break;
                case WeaponType.magic_spread:
                    PerformSpreadShot();
                    break;

            }
        }
        if (InputManager.DefaultAttackIsHeld)
        {
            if (currentWeaponType == WeaponType.magic_riffle)
            {
                PerformRiffleShot();
            }
        }

        if (InputManager.SpecialAttackWasPressed)
        {
            if (currentWeaponType == WeaponType.magic_riffle || currentWeaponType == WeaponType.magic_spread || currentWeaponType == WeaponType.magic_single)
            {
                PerformMeleeAttack(1f, 0.3f, 90f, 0.2f, enemyLayer, true);
            }
        }
    }
    private void PerformMeleeAttack(float attackRadius, float attackWidth,
                float attackAngle, float attackDuration, LayerMask enemyLayer, bool isblunk)
    {
        if (actions != null)
        {

            actions.PerformMeleeAttack(transform, attackRadius, attackWidth,
                attackAngle, attackDuration, enemyLayer, isblunk);

        }
        else
        {
            Debug.LogError("Actions组件未分配！");
        }
    }

    private void PerformShoot()
    {
        if (actions != null)
        {

            actions.PerformShoot(transform);

        }
        else
        {
            Debug.LogError("Actions组件未分配！");
        }
    }

    private void PerformSpreadShot()
    {
        if (actions != null)
        {

            actions.PerformSpreadShot(transform);

        }
        else
        {
            Debug.LogError("Actions组件未分配！");
        }
    }

    private void PerformRiffleShot()
    {
        if (actions != null)
        {

            actions.PerformRiffleShot(transform);

        }
        else
        {
            Debug.LogError("Actions组件未分配！");
        }
    }

    private void RotateTowardsMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPos.z = 0;

        Vector2 directionToMouse = new Vector2(
            mouseWorldPos.x - transform.position.x,
            mouseWorldPos.y - transform.position.y
        );

        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
