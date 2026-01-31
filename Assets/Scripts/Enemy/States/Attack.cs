using UnityEngine;

public class Attack : StateBase, IState
{
    private AnimatorStateInfo currentAnimState;

    public Attack(EnemyFSM fsm) : base(fsm)
    {
    }

    public void OnEnter()
    {
        _fsm.Agent.SetSpeed(0f);
        _fsm.HeadAnimator.SetBool("Attack", true);
        Actions actions = _fsm.Actions;
        Transform attacker = _fsm.gameObject.transform;
        switch (_fsm.Parameters.weaponType)
        {
            case WeaponType.knife:
                actions.PerformMeleeAttack_knife(attacker);
                break;
            case WeaponType.sword:
                actions.PerformMeleeAttack_sword(attacker);
                break;
            case WeaponType.hammer:
                actions.PerformMeleeAttack_hammer(attacker);
                break;
            case WeaponType.Spear:
                actions.PerformSpearAttack(attacker);
                break;
            case WeaponType.magic_single:
                actions.PerformShoot(attacker);
                break;
            case WeaponType.magic_spread:
                actions.PerformSpreadShot(attacker);
                break;
        }
    }

    public void OnUpdate()
    {
        currentAnimState = _fsm.HeadAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimState.normalizedTime >= 1.0f)
            _fsm.TransitionState(new Hunt(_fsm));
    }

    public void OnExit()
    {
        _fsm.HeadAnimator.SetBool("Attack", false);
    }
}