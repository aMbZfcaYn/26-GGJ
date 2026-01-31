using UnityEngine;

public class Trace : StateBase, IState
{
    private readonly Transform _soundSource;

    public Trace(EnemyFSM fsm, Transform soundSource) : base(fsm)
    {
        _soundSource = soundSource;
    }

    public void OnEnter()
    {
        _fsm.Agent.SetTempWaypoint(_soundSource);
    }

    public void OnUpdate()
    {
        _fsm.Animator.SetFloat("MoveSpeed", _fsm.Agent.CurrentSpeed);

        if (_fsm.CanSeePlayer())
            _fsm.TransitionState(new Hunt(_fsm));
        else if (_fsm.Agent.HasReachedCurrentWaypoint)
            _fsm.TransitionState(new Patrol(_fsm));
    }

    public void OnExit()
    {
    }
}