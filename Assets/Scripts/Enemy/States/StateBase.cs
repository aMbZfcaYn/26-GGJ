using UnityEngine;

public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}

public class StateBase
{
    protected EnemyFSM _fsm;
    protected EnemyParameter _parameters;

    public StateBase(EnemyFSM fsm)
    {
        _fsm = fsm;
        _parameters = fsm.Parameters;
    }
}