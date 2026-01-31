using System;
using System.Collections.Generic;
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

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit,
                float.PositiveInfinity) && hit.transform != player) return false;
        return true;
    }

    public bool CanAttackPlayer()
    {
        throw new System.NotImplementedException();
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