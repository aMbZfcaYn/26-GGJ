using UnityEngine;

public class Trace : StateBase
{
    private readonly Transform _soundSource;
    public Trace(EnemyFSM fsm, Transform soundSource) : base(fsm)
    {
        _soundSource = soundSource;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _fsm.Agent.SetTempWaypoint(_soundSource);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_fsm.CanSeePlayer())
            _fsm.TransitionState(new Hunt(_fsm));
        else if(_fsm.HeardSound())
            _fsm.TransitionState(new Trace(_fsm, _fsm.SoundSource));
        else if (_fsm.Agent.HasReachedCurrentWaypoint)
            _fsm.TransitionState(new Patrol(_fsm));
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
