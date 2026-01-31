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

        if (_fsm.CanSeePlayer())
            _fsm.TransitionState(new Hunt(_fsm));
        else
            _fsm.Listen();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
