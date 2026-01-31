using UnityEngine;

public class Stun : StateBase, IState
{
    private float _stunTimer;
    private readonly Transform _hitFrom;

    public Stun(EnemyFSM fsm, Transform hitFrom) : base(fsm)
    {
        _hitFrom = hitFrom;
    }

    public void OnEnter()
    {
        _stunTimer = 0f;
        _fsm.Animator.SetBool("Stun", true);

        Vector3 lookPosition = new(_hitFrom.position.x, _fsm.transform.position.y, _hitFrom.position.z);
        _fsm.transform.LookAt(lookPosition);

        _fsm.gameObject.GetComponent<Rigidbody2D>().AddForce(
            Vector3.back * _parameters.KnockbackMagnitude);

        _fsm.Agent.SetSpeed(0f);
    }

    public void OnUpdate()
    {
        _stunTimer += Time.fixedDeltaTime;

        if (_stunTimer >= _parameters.StunDuration)
            _fsm.TransitionState(new Hunt(_fsm));
    }

    public void OnExit()
    {
        _fsm.Animator.SetBool("Stun", false);
    }
}
