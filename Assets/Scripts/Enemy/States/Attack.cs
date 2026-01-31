using UnityEngine;

public class Attack : StateBase, IState
{
    private AnimatorStateInfo currentAnimState;

    public Attack(EnemyFSM fsm) : base(fsm) { }

    public void OnEnter()
    {
        _fsm.Agent.SetSpeed(0f);
        _fsm.Animator.SetBool("Attack", true);
    }

    public void OnUpdate()
    {
        currentAnimState = _fsm.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimState.normalizedTime >= 1.0f)
            _fsm.TransitionState(new Hunt(_fsm));
    }

    public void OnExit()
    {
        _fsm.Animator.SetBool("Attack", false);
    }
}