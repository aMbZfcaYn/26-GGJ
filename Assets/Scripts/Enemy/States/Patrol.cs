using UnityEngine;

public class Patrol : StateBase
{
    public Patrol(EnemyFSM fsm) : base(fsm) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _fsm.Agent.SetSpeed(_fsm.Parameters.PatrolSpeed);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        _fsm.Animator.SetFloat("MoveSpeed", _fsm.Agent.CurrentSpeed);

        if (_fsm.CanSeePlayer())
            _fsm.TransitionState(new Hunt(_fsm));
        else if (_fsm.HeardSound())
            _fsm.TransitionState(new Trace(_fsm, _fsm.SoundSource));
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
