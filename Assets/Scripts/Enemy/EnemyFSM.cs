using System;
using System.Collections.Generic;
using System.Linq;
using Management;
using Management.Tag;
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

    private IState currentState;
    private Taggable taggable;

    private void Awake()
    {
        if (agent == null) agent = GetComponent<AStarAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        if (taggable == null) taggable = GetComponent<Taggable>();
    }

    private void Start()
    {
        taggable.TryAddTag(TagUtils.Type_Enemy);
        GameManager.Instance.RegisterEnemy(gameObject);
        TransitionState(new Patrol(this));
    }

    private void FixedUpdate()
    {
        currentState?.OnUpdate();
    }

    public void TransitionState(IState state)
    {
        Debug.Log(name + " Exited State: " + state.GetType().Name);
        currentState?.OnExit();
        currentState = state;
        currentState?.OnEnter();
        Debug.Log(name + " Entered State: " + state.GetType().Name);
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
    /// <summary>
    /// Event func of event: onSoundEmit
    /// </summary>
    /// <param name="soundEmitter">Entity that make the sound</param>
    public void HeardSound(Transform soundEmitter)
    {
        if (currentState is not (Patrol or Trace)) return;
        Vector3 directionToEmitter = soundEmitter.position - transform.position;
        if (directionToEmitter.sqrMagnitude > parameters.HearDistance * parameters.HearDistance)
        {
            TransitionState(new Trace(this, soundEmitter));
        }
    }
}