using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarAgent : MonoBehaviour
{
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private Seeker _seeker;
    [SerializeField] private AIDestinationSetter _destinationSetter;

    public void SetSpeed(float speed)
    {
        if (_aiPath)
        {
            _aiPath.maxSpeed = speed;
        }
    }

    public void SetCanMove(bool canMove)
    {
        if (_aiPath)
        {
            _aiPath.canMove = canMove;
        }
    }

    public float CurrentSpeed => _aiPath ? _aiPath.velocity.magnitude : 0f;

    // Waypoints Mode ----------------------------------------------------
    public Transform CurrentWaypoint
    {
        get
        {
            if (_temporaryWaypoint) return _temporaryWaypoint;
            return _patrolWaypoints.Count > 0 ? _patrolWaypoints[_currentPatrolIndex] : null;
        }
    }

    public bool HasWaypoints => _patrolWaypoints.Count != 0 || _temporaryWaypoint;
    public bool HasReachedCurrentWaypoint => _hasReachedCurrentWaypoint;

    [SerializeField] private List<Transform> _patrolWaypoints = new();
    [SerializeField] private int _currentPatrolIndex = 0;


    [SerializeField] private float _showRouteDuration = 1.5f;

    private Transform _temporaryWaypoint;
    private bool _hasReachedCurrentWaypoint = false;

    public void ClearWaypoints()
    {
        _patrolWaypoints.Clear();
        _currentPatrolIndex = 0;
        _temporaryWaypoint = null;
        _hasReachedCurrentWaypoint = false;
        if (_seeker) _seeker.drawGizmos = false;
    }

    public void SetTempWaypoint(Transform target)
    {
        if (!target)
        {
            Debug.LogWarning($"[Agent] Attempted to add null waypoint to {name}");
            return;
        }

        _temporaryWaypoint = target;
        SetDestinationToCurrentWaypoint();
    }

    private void SetDestinationToCurrentWaypoint()
    {
        if (CurrentWaypoint && _aiPath)
        {
            _aiPath.destination = CurrentWaypoint.position;
            StartCoroutine(nameof(ShowRoute));
        }
    }

    private void PatrolToNextWaypoint()
    {
        if (_temporaryWaypoint)
        {
            _temporaryWaypoint = null;
            SetDestinationToCurrentWaypoint();
            return;
        }

        _currentPatrolIndex++;
        if (_currentPatrolIndex == _patrolWaypoints.Count)
            _currentPatrolIndex = 0;
        SetDestinationToCurrentWaypoint();
    }
    // --------------------------------------------------------------------

    // Follow Mode -------------------------------------------------------

    public Transform MovingTarget => _movingTarget;
    public bool IsFollowing => _isFollowing;
    private bool _isFollowing = false;
    private Transform _movingTarget;

    public void EnableFollow(Transform movingTarget)
    {
        if (!movingTarget)
        {
            Debug.LogWarning($"[Agent] Attempted to follow null target for {name}");
            return;
        }

        if (_isFollowing)
        {
            Debug.LogWarning($"[Agent] {name} is already following a target");
            return;
        }

        _movingTarget = movingTarget;
        _isFollowing = true;
        _destinationSetter.enabled = _isFollowing;
        _destinationSetter.target = movingTarget;
    }

    public void DisableFollow()
    {
        if (!_isFollowing)
        {
            Debug.LogWarning($"[Agent] {name} is not currently following a target");
            return;
        }

        _movingTarget = null;
        _isFollowing = false;
        _destinationSetter.target = null;
        _destinationSetter.enabled = false;
    }
    // --------------------------------------------------------------------


    private void GetComponents()
    {
        if (!_aiPath) _aiPath = GetComponent<AIPath>();
        if (!_seeker) _seeker = GetComponent<Seeker>();
        if (!_destinationSetter) _destinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Awake()
    {
        GetComponents();
        if (_patrolWaypoints.Count > 0) SetDestinationToCurrentWaypoint();
    }

    private void Update()
    {
        if (!_aiPath) return;
        if (_isFollowing) return; // follow mode overrides waypoints
        if (!HasWaypoints) return;

        if (_aiPath.reachedEndOfPath != _hasReachedCurrentWaypoint)
        {
            _hasReachedCurrentWaypoint = _aiPath.reachedEndOfPath;
            if (_hasReachedCurrentWaypoint) PatrolToNextWaypoint();
        }
    }

    private IEnumerator ShowRoute()
    {
        if (_seeker)
        {
            _seeker.drawGizmos = true;
            yield return new WaitForSeconds(_showRouteDuration);
            _seeker.drawGizmos = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (_isFollowing && _movingTarget)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, _movingTarget.position);
            Gizmos.DrawWireSphere(_movingTarget.position, 0.4f);
        }

        if (_patrolWaypoints == null || _patrolWaypoints.Count == 0 || _isFollowing) return;

        Gizmos.color = Color.yellow;
        for (int i = _currentPatrolIndex; i < _patrolWaypoints.Count; i++)
        {
            if (!_patrolWaypoints[i]) continue;
            if (i == _currentPatrolIndex)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_patrolWaypoints[i].position, 0.8f);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_patrolWaypoints[i].position, 0.5f);
            }

            // Draw lines between _waypoints
            if (i > _currentPatrolIndex)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_patrolWaypoints[i - 1].position, _patrolWaypoints[i].position);
            }
        }

        // Draw line from agent to current target
        if (CurrentWaypoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, CurrentWaypoint.position);
        }
    }
}