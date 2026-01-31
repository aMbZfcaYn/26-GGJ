using InputNamespace;
using UnityEngine;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Actions actions;

        [SerializeField] private float attackRadius = 1.5f;
        [SerializeField] private float attackWidth = 0.3f;
        [SerializeField] private float attackAngle = 90f;
        [SerializeField] private float attackDuration = 0.2f;
        [SerializeField] private LayerMask enemyLayer;
        private Rigidbody2D rb;
        private AbilityManager ability;
        
        private bool inAbility = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            ability = GetComponent<AbilityManager>();
        }

        void Update()
        {
            RotateTowardsMouse();
            if (InputManager.DefaultAttackWasPressed)
            {
                PerformMeleeAttack();
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
        }
        
        void FixedUpdate()
        {
            Vector2 movement = InputManager.Movement;
            rb.linearVelocity = movement * moveSpeed;
        }
        private void PerformMeleeAttack()
        {
            if (actions != null)
            {

                actions.PerformMeleeAttack(transform, attackRadius, attackWidth,
                    attackAngle, attackDuration, enemyLayer);

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
}
