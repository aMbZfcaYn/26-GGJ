using InputNamespace;
using Management;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool blocked = false;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Actions actions;
    [SerializeField] private WeaponType currentWeaponType = WeaponType.knife;

    private Rigidbody2D rb;
    private AbilityManager ability;

    private bool inAbility = false;

    void Start()
    {
        Init(true);
    }

    public void Init(bool first = false)
    {
        var camScript = Camera.main.GetComponent<CameraControl>();
        camScript.target = transform;

        rb = GetComponent<Rigidbody2D>();
        ability = GetComponent<AbilityManager>();

        GameManager.Instance.RegisterPlayer(gameObject, first);

        // 设置当前武器类型
        if (actions)
        {
            actions.SetCurrentWeapon(currentWeaponType);
        }
    }

    void FixedUpdate()
    {
        if (blocked)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector2 movement = InputManager.Movement;
        rb.linearVelocity = movement * moveSpeed;

        RotateTowardsMouse();
    }

    void Update()
    {
        if (InputManager.SkillWasPressed)
        {
            if (ability.needSwitcher)
            {
                if (!inAbility)
                    ability.OnAbilityButtonPressed();
                inAbility = !inAbility;
            }
            else
                ability.OnAbilityButtonPressed();
        }

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
        if (actions)
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
        if (actions)
        {
            actions.PerformSpearAttack(transform);
        }
    }

    private void PerformShoot()
    {
        if (actions)
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
        if (actions)
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
        if (actions)
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