using UnityEngine;

public class Hunt : StateBase
{
    public Hunt(EnemyFSM fsm) : base(fsm) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _fsm.Agent.EnableFollow(_fsm.Player);
        _fsm.Agent.SetSpeed(_parameters.HuntSpeed);
        _fsm.Animator.SetBool("IsHunting", true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (_fsm.CanAttackPlayer())
            _fsm.TransitionState(new Attack(_fsm));
    }

    public override void OnExit()
    {
        base.OnExit();
        _fsm.Agent.DisableFollow();
        _fsm.Animator.SetBool("IsHunting", false);
    }
}
