using UnityEngine;

public class Stun : StateBase, IState
{
    private const float ACCEL = -5f;
    private float _stunTimer;
    private readonly Transform _hitFrom;
    private readonly Rigidbody2D _rb;

    public Stun(EnemyFSM fsm, Transform hitFrom) : base(fsm)
    {
        _hitFrom = hitFrom;
        _rb = _fsm.gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        _stunTimer = 0f;
        _fsm.HeadAnimator.SetBool("Stun", true);
        _fsm.LegAnimator.SetBool("Stun", true);

        // Disable pathfinding movement to allow physics knockback
        _fsm.Agent.SetCanMove(false);

        if (_hitFrom != null)
        {
            // Rotate to look at hitFrom
            Vector2 lookDirection = _hitFrom.position - _fsm.transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            _fsm.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Knockback away from hitFrom
            Vector2 knockbackDirection = -lookDirection.normalized;
            _rb.AddForce(knockbackDirection * _parameters.KnockbackMagnitude, ForceMode2D.Impulse);
        }
    }

    public void OnUpdate()
    {
        _stunTimer += Time.fixedDeltaTime;

        _rb.linearVelocity += ACCEL * Time.fixedDeltaTime * _rb.linearVelocity.normalized;

        if (_stunTimer >= _parameters.StunDuration)
            _fsm.TransitionState(new Hunt(_fsm));
    }

    public void OnExit()
    {
        _fsm.HeadAnimator.SetBool("Stun", false);
        _fsm.LegAnimator.SetBool("Stun", false);

        _fsm.Agent.SetCanMove(true);
    }
}