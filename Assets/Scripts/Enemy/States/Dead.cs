using UnityEngine;

public class Dead : StateBase, IState
{
    private AnimatorStateInfo currentAnimState;
    private bool isPossessioned;

    public Dead(EnemyFSM fsm, bool _isPossessioned = false) : base(fsm)
    {
        isPossessioned = _isPossessioned;
    }

    public void OnEnter()
    {
        _fsm.HeadAnimator.SetTrigger("HitByOtherAttack");
        _fsm.LegAnimator.SetTrigger("HitByOtherAttack");
        _fsm.Agent.SetSpeed(0f);
        if (isPossessioned)
        {
            _fsm.HeadAnimator.Rebind();
            _fsm.LegAnimator.Rebind();
        }
    }

    public void OnUpdate()
    {
        currentAnimState = _fsm.HeadAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimState.normalizedTime >= 1.0f)
        {
            if (!isPossessioned)
            {
                _fsm.HeadAnimator.enabled = false;
                _fsm.LegAnimator.enabled = false;
                _fsm.GetComponent<Collider2D>().enabled = false;
            }

            _fsm.enabled = false;
        }
    }

    public void OnExit()
    {
    }
}