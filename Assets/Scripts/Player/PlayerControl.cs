using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Actions actions;
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
                    PerformMeleeAttack(currentWeaponType);
                    break;
                case WeaponType.sword:
                    PerformMeleeAttack(currentWeaponType);
                    break;
                case WeaponType.hammer:
                    PerformMeleeAttack(currentWeaponType);
                    break;
                case WeaponType.Spear:
                    PerformSpearAttack();
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
            if (currentWeaponType == WeaponType.magic_riffle ||
                currentWeaponType == WeaponType.magic_spread ||
                currentWeaponType == WeaponType.magic_single)
            {
                PerformMeleeAttack(currentWeaponType);
            }
        }
    }

    private void PerformMeleeAttack(WeaponType weaponType)
    {
        if (actions != null)
        {
            actions.PerformMeleeAttack(transform, weaponType);
        }
        else
        {
            Debug.LogError("Actions组件未分配！");
        }
    }



    private void PerformSpearAttack()
    {
        if (actions != null)
        {
            actions.PerformSpearAttack(transform);
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