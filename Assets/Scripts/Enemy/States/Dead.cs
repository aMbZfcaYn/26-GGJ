using UnityEngine;

public class Dead : StateBase, IState
{
    private AnimatorStateInfo currentAnimState;

    public Dead(EnemyFSM fsm) : base(fsm) { }

    public void OnEnter()
    {
        _fsm.Animator.SetTrigger("HitByOtherAttack");
        _fsm.Agent.SetSpeed(0f);
    }

    public void OnUpdate()
    {
        currentAnimState = _fsm.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimState.normalizedTime >= 1.0f)
            _fsm.enabled = false;
    }

    public void OnExit()
    {
    }
}