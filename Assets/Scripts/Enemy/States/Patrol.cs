using UnityEngine;

public class Patrol : StateBase, IState
{
    public Patrol(EnemyFSM fsm) : base(fsm) { }

    public void OnEnter()
    {
        _fsm.Agent.SetSpeed(_fsm.Parameters.PatrolSpeed);
    }

    public void OnUpdate()
    {
        _fsm.Animator.SetFloat("MoveSpeed", _fsm.Agent.CurrentSpeed);

        if (_fsm.CanSeePlayer())
            _fsm.TransitionState(new Hunt(_fsm));
        else if (_fsm.HeardSound())
            _fsm.TransitionState(new Trace(_fsm, _fsm.SoundSource));
    }

    public void OnExit()
    {
    
    }
}
