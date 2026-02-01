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
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator legAnimator;
    [SerializeField] private Actions actions;

    public EnemyParameter Parameters => parameters;
    public AStarAgent Agent => agent;
    public Animator HeadAnimator => headAnimator;
    public Animator LegAnimator => legAnimator;
    public Transform Player => GameManager.Instance.player.transform;
    public Actions Actions => actions;

    private IState currentState;
    private Taggable taggable;

    private void Awake()
    {
        if (!agent) agent = GetComponent<AStarAgent>();
        if (!headAnimator) headAnimator = GetComponentsInChildren<Animator>()[0];
        if (!legAnimator) legAnimator = GetComponentsInChildren<Animator>()[1];
        if (!taggable) taggable = GetComponent<Taggable>();
        if (!actions) actions = GetComponent<Actions>();
    }

    private void Start()
    {
        taggable.TryAddTag(TagUtils.Type_Enemy);
        GameManager.Instance.RegisterEnemy(gameObject);
        GameEventManager.Instance.onSoundEmit.AddListener(HeardSound);
        TransitionState(new Patrol(this));
    }

    private void FixedUpdate()
    {
        currentState?.OnUpdate();
    }

    private void OnDisable()
    {
        Agent.enabled = false;
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
        Vector2 directionToPlayer = Player.position - transform.position;

        // Check if player in angle
        float angleToPlayer = Vector2.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > parameters.ViewAngle / 2)
        {
            return false;
        }

        // Check if there is an obstacle in ray path.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized);
        //Debug.Log("Raycast hit: " + (hit ? hit.transform.name : "Nothing"));
        if (hit)
        {
            return hit.transform == Player;
        }

        // hit and hit player
        return false;
    }

    public bool CanAttackPlayer()
    {
        if (!CanSeePlayer()) return false;
        Vector2 directionToPlayer = Player.position - transform.position;
        // In half of atkRange
        if (directionToPlayer.sqrMagnitude > parameters.AtkRange * parameters.AtkRange / 4)
        {
            return false;
        }

        // In half of atkAngle
        float angleToPlayer = Vector2.Angle(transform.forward, directionToPlayer);
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
    /// <param name="strength">Strength, or range of that sound</param>
    public void HeardSound(Transform soundEmitter, float strength)
    {
        if (currentState is not (Patrol or Trace)) return;
        Vector2 directionToEmitter = soundEmitter.position - transform.position;
        // Strength plus HearDistance is the real triggerable distance
        if (directionToEmitter.magnitude < parameters.HearDistance + strength)
        {
            TransitionState(new Trace(this, soundEmitter));
        }
    }
}