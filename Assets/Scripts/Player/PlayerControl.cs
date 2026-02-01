using InputNamespace;
using Management;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool blocked = false;

    [SerializeField] private float moveSpeed = 5f;
    public Actions actions;
    public WeaponType currentWeaponType = WeaponType.knife;

    private Rigidbody2D rb;
    private AbilityManager ability;
    private AnimatorStateInfo currentAnimState;


    public GameObject leg;
    public Animator LegAnimator;
    public Animator BodyAnimator;
    public RuntimeAnimatorController PlayerBodyAC;
    public RuntimeAnimatorController PlayerLegAC;

    private bool inAbility = false;

    [Header("SoundEmit")] [Range(0, 10)] public float walkSoundStrength = 5f;
    [Range(0, 30)] public float atkSoundStrength = 10f;

    private int walkSoundEmitCooldown = 0;
    private int walkSoundEmitCooldownMax = 25;

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

        BodyAnimator = GetComponentsInChildren<Animator>()[0];
        LegAnimator = GetComponentsInChildren<Animator>()[1];


    }

    void FixedUpdate()
    {
        if (blocked)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Debug.Log(InputManager.Movement);

        Vector2 movement = InputManager.Movement;
        rb.linearVelocity = movement * moveSpeed;

        // Debug.Log(rb.linearVelocity);

        LegAnimator.SetFloat("MoveSpeed", rb.linearVelocity.magnitude);
        if (movement.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            leg.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // Emit walk sound event. Up to 2 times per second.
            if (walkSoundEmitCooldown > walkSoundEmitCooldownMax)
            {
                walkSoundEmitCooldown = 0;
                GameEventManager.Instance.onSoundEmit.Invoke(transform, walkSoundStrength);
            }
        }

        walkSoundEmitCooldown++;

        RotateTowardsMouse();
    }

    void Update()
    {
        if (BodyAnimator.GetBool("Attack"))
        {
            currentAnimState = BodyAnimator.GetCurrentAnimatorStateInfo(0);
            if (currentAnimState.normalizedTime >= 1f)
                BodyAnimator.SetBool("Attack", false);
        }

        if (BodyAnimator.GetBool("Attack2"))
        {
            currentAnimState = BodyAnimator.GetCurrentAnimatorStateInfo(0);
            if (currentAnimState.normalizedTime >= 1f)
                BodyAnimator.SetBool("Attack2", false);
        }


        if (InputManager.SkillWasPressed)
        {
            if (ability.needSwitcher)
            {
                if (!inAbility)
                {
                    if (GameManager.Instance.IsEnergyFilled())
                        ability.OnAbilityButtonPressed();
                }
                inAbility = !inAbility;
            }
            else
            {
                if(GameManager.Instance.IsEnergyFilled())
                    ability.OnAbilityButtonPressed();
            }
        }

        if (InputManager.DefaultAttackWasPressed)
        {
            BodyAnimator.SetBool("Attack", true);
            //Debug.Log(BodyAnimator.GetBool("Attack"));

            switch (currentWeaponType)
            {
                case WeaponType.knife:
                    actions.PerformMeleeAttack_knife(transform);
                    break;
                case WeaponType.sword:
                    actions.PerformMeleeAttack_sword(transform);
                    break;
                case WeaponType.hammer:
                    actions.PerformMeleeAttack_hammer(transform);
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

            GameEventManager.Instance.onSoundEmit.Invoke(transform, atkSoundStrength);
        }

        if (InputManager.DefaultAttackIsHeld)
        {
            if (currentWeaponType == WeaponType.magic_riffle)
            {
                BodyAnimator.SetBool("Attack", true);
                PerformRiffleShot();
            }

            // BodyAnimator.SetBool("Attack", false);
            GameEventManager.Instance.onSoundEmit.Invoke(transform, atkSoundStrength);
        }

        if (InputManager.SpecialAttackWasPressed)
        {
            BodyAnimator.SetBool("Attack2", true);
            if (currentWeaponType == WeaponType.magic_riffle ||
                currentWeaponType == WeaponType.magic_spread ||
                currentWeaponType == WeaponType.magic_single)
            {
                actions.PerformMeleeAttack_magic(transform);
            }

            BodyAnimator.SetBool("Attack2", false);
            GameEventManager.Instance.onSoundEmit.Invoke(transform, atkSoundStrength);
        }
    }

    // private void PerformMeleeAttack(WeaponType weaponType)
    // {
    //     if (actions)
    //     {
    //         actions.PerformMeleeAttack(transform, weaponType);
    //     }
    //     else
    //     {
    //         Debug.LogError("Actions组件未分配！");
    //     }
    // }
    // private void PerformMeleeAttack(WeaponType weaponType)
    // {
    //     if (actions)
    //     {
    //         actions.PerformMeleeAttack(transform, weaponType);
    //     }
    //     else
    //     {
    //         Debug.LogError("Actions组件未分配！");
    //     }
    // }


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

        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}