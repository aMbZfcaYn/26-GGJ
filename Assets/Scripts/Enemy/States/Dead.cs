using UnityEngine;

public class Dead : StateBase
{
    private AnimatorStateInfo currentAnimState;

    public Dead(EnemyFSM fsm) : base(fsm) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _fsm.Animator.SetTrigger("Dead");
        _fsm.Agent.SetSpeed(0f);
    }

    public override void OnUpdate()
    {
        currentAnimState = _fsm.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimState.normalizedTime >= 1.0f)
            _fsm.enabled = false;
    }
}
