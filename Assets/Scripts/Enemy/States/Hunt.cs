using UnityEngine;

public class Hunt : StateBase, IState
{
    public Hunt(EnemyFSM fsm) : base(fsm) { }

    public void OnEnter()
    {
        if(!_fsm.Agent.IsFollowing)
            _fsm.Agent.EnableFollow(_fsm.Player);
        _fsm.Agent.SetSpeed(_parameters.HuntSpeed);
        _fsm.Animator.SetBool("IsHunting", true);
    }

    public void OnUpdate()
    {
        _fsm.Animator.SetFloat("MoveSpeed", _fsm.Agent.CurrentSpeed);

        if (_fsm.CanAttackPlayer())
            _fsm.TransitionState(new Attack(_fsm));
    }

    public void OnExit()
    {
        _fsm.Agent.DisableFollow();
        _fsm.Animator.SetBool("IsHunting", false);
    }
}
