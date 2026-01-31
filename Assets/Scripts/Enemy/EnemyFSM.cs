using System;
using System.Collections.Generic;
using System.Linq;
using Management;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField] private EnemyParameter parameters;
    [SerializeField] private AStarAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform soundSource;
    [SerializeField] private Transform player;

    public EnemyParameter Parameters => parameters;
    public AStarAgent Agent => agent;
    public Animator Animator => animator;
    public Transform SoundSource => soundSource;
    public Transform Player => player;

    private StateBase currentState;

    // Tips: 50 may cause bug if we have some unexpected small entities.
    private RaycastHit[] _hits = new RaycastHit[50];

    private void Awake()
    {
        if (agent == null) agent = GetComponent<AStarAgent>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TransitionState(new Patrol(this));
    }

    private void FixedUpdate()
    {
        currentState?.OnUpdate();
    }

    public void TransitionState(StateBase state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState?.OnEnter();
    }

    public bool CanSeePlayer()
    {
        // Do not check player is null, since it just should crack when happen rather than consume an "if"
        player = GameManager.Instance.player.transform;
        Vector3 directionToPlayer = player.position - transform.position;

        // Check if player in angle
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > parameters.ViewAngle / 2)
        {
            return false;
        }

        // Check if there is an obstacle in ray path.
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, 100000f))
        {
            return hit.transform == player;
        }

        // hit and hit player
        return false;
    }

    public bool CanAttackPlayer()
    {
        if (!CanSeePlayer()) return false;
        player = GameManager.Instance.player.transform;
        Vector3 directionToPlayer = player.position - transform.position;
        // In half of atkRange
        if (directionToPlayer.sqrMagnitude > parameters.AtkRange * parameters.AtkRange / 4)
        {
            return false;
        }

        // In half of atkAngle
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > parameters.AtkAngle / 3)
        {
            return false;
        }

        return true;
    }

    public bool HitByBlunt()
    {
        throw new System.NotImplementedException();
    }

    public bool HitByOtherAttack()
    {
        throw new System.NotImplementedException();
    }

    public bool HeardSound()
    {
        throw new System.NotImplementedException();
    }
}