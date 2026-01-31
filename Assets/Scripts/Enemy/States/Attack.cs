using UnityEngine;

public class Attack : StateBase
{
    private AnimatorStateInfo currentAnimState;
    public Attack(EnemyFSM fsm) : base(fsm) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _fsm.Agent.SetSpeed(0f);
        _fsm.Animator.SetTrigger("Attack");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        currentAnimState = _fsm.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimState.normalizedTime >= 1.0f)
        {
            if (_fsm.CanAttackPlayer())
                _fsm.TransitionState(new Attack(_fsm));
            else
                _fsm.TransitionState(new Hunt(_fsm));
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
