using UnityEngine;

public class Stun : StateBase
{
    private float _stunTimer;

    public Stun(EnemyFSM fsm) : base(fsm) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _stunTimer = 0f;
        _fsm.Animator.SetTrigger("HitByBlunt");
        _fsm.Agent.SetSpeed(0f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        _stunTimer += Time.fixedDeltaTime;
        
        if (_stunTimer >= _parameters.StunDuration)
            _fsm.TransitionState(new Hunt(_fsm));
    }

    public override void OnExit()
    {
        base.OnExit();
        _fsm.Animator.SetTrigger("WakeFromStun");
    }
}
