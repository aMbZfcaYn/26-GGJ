using System.Collections.Generic;
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
        throw new System.NotImplementedException();
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