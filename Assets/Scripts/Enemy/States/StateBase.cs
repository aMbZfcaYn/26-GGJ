using UnityEngine;

public class StateBase
{
    protected EnemyFSM _fsm;
    protected EnemyParameter _parameters;

    public StateBase(EnemyFSM fsm)
    {
        _fsm = fsm;
        _parameters = fsm.Parameters;
    }

    public virtual void OnEnter()
    {
        Debug.Log(_fsm.name + " Entered State: " + GetType().Name);
    }

    public virtual void OnUpdate()
    {
        if (_fsm.HitByOtherAttack())
            _fsm.TransitionState(new Dead(_fsm));
        else if (_fsm.HitByBlunt())
            _fsm.TransitionState(new Stun(_fsm));
    }

    public virtual void OnExit()
    {
        Debug.Log(_fsm.name + " Exited State: " + GetType().Name);
    }
}

