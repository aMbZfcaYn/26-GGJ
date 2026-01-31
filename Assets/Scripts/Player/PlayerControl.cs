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
    private AnimatorStateInfo currentAnimState;


    [SerializeField] private GameObject leg;
    public Animator LegAnimator;
    public Animator BodyAnimator;

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
        LegAnimator.SetFloat("MoveSpeed", rb.linearVelocity.magnitude);
        if (movement.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            leg.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        RotateTowardsMouse();
    }

    void Update()
    {
        if (BodyAnimator.GetBool("Attack"))
        {
            currentAnimState = BodyAnimator.GetCurrentAnimatorStateInfo(0);
            if (currentAnimState.normalizedTime >= 1.0f)
                BodyAnimator.SetBool("Attack", false);
        }
        if (BodyAnimator.GetBool("Attack2"))
        {
            currentAnimState = BodyAnimator.GetCurrentAnimatorStateInfo(0);
            if (currentAnimState.normalizedTime >= 1.0f)
                BodyAnimator.SetBool("Attack2", false);
        }


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

            BodyAnimator.SetBool("Attack", true);
            Debug.Log(BodyAnimator.GetBool("Attack"));

            switch (currentWeaponType)
            {

                case WeaponType.knife:
                    PerformMeleeAttack(currentWeaponType);

                    break;
                case WeaponType.sword:
                    PerformMeleeAttack(currentWeaponType);
                    BodyAnimator.SetBool("Attack", false);
                    break;
                case WeaponType.hammer:
                    PerformMeleeAttack(currentWeaponType);
                    BodyAnimator.SetBool("Attack", false);
                    break;
                case WeaponType.Spear:
                    PerformSpearAttack();
                    BodyAnimator.SetBool("Attack", false);
                    break;
                case WeaponType.magic_single:
                    PerformShoot();
                    BodyAnimator.SetBool("Attack", false);
                    break;
                case WeaponType.magic_spread:
                    PerformSpreadShot();
                    BodyAnimator.SetBool("Attack", false);
                    break;
            }
        }

        if (InputManager.DefaultAttackIsHeld)
        {
            BodyAnimator.SetBool("Attack", true);
            if (currentWeaponType == WeaponType.magic_riffle)
            {
                PerformRiffleShot();
            }
            //BodyAnimator.SetBool("Attack",false);
        }

        if (InputManager.SpecialAttackWasPressed)
        {
            BodyAnimator.SetBool("Attack2", true);
            if (currentWeaponType == WeaponType.magic_riffle ||
                currentWeaponType == WeaponType.magic_spread ||
                currentWeaponType == WeaponType.magic_single)
            {
                PerformMeleeAttack(currentWeaponType);
            }
            BodyAnimator.SetBool("Attack2", false);
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