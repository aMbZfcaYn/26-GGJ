using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Patrol,
    Attack
}
public class EnemyFSM : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyParameter parameters;
    [SerializeField] private AStarAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;

    public EnemyParameter Parameters => parameters;
    public AStarAgent Agent => agent;
    public Animator Animator => animator;
    public Transform Target => target;

    private IState currentState;
    private readonly Dictionary<StateType, IState> states = new();

    private void Awake()
    {
        if (agent == null) agent = GetComponent<AStarAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        // states.Add(StateType.Idle, new Idle(this));
    }
    private void Start()
    {
        TransitionState(StateType.Idle);
    }

    private void Update()
    {
        currentState?.OnUpdate();
    }
    public void TransitionState(StateType state)
    {
        currentState?.OnExit();
        currentState = states[state];
        currentState?.OnEnter();
    }
}