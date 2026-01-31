using UnityEngine;

public class Stun : StateBase, IState
{
    private float _stunTimer;

    public Stun(EnemyFSM fsm) : base(fsm) { }

    public void OnEnter()
    {
        _stunTimer = 0f;
        _fsm.Animator.SetBool("Stun",true);
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
