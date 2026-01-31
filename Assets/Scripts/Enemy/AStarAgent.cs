using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarAgent : MonoBehaviour
{
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private Seeker _seeker;
    [SerializeField] private AIDestinationSetter _destinationSetter;

    // Waypoints Mode ----------------------------------------------------
    public Transform CurrentWaypoint => HasWaypoints ? _waypoints[_currentWaypointIndex] : null;
    public bool HasWaypoints => _currentWaypointIndex < _waypoints.Count;

    [SerializeField] private List<Transform> _waypoints = new();

    [SerializeField] private int _currentWaypointIndex = 0;

    [SerializeField] private float _showRouteDuration = 1.5f;

    private bool _hasReachedCurrentWaypoint = false;

    public void ClearWaypoints()
    {
        _waypoints.Clear();
        _currentWaypointIndex = 0;
        _hasReachedCurrentWaypoint = false;
        if (_seeker != null) _seeker.drawGizmos = false;
    }

    public void AddWayPoint(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning($"[Agent] Attempted to add null waypoint to {name}");
            return;
        }
        _waypoints.Add(target);
        if (_waypoints.Count == 1 && _currentWaypointIndex == 0 && _aiPath != null)
        {
            SetDestinationToCurrentWaypoint();
        }
    }

    protected void OnReachedFinalDestination()
    {
        if (_seeker != null) _seeker.drawGizmos = false;
    }
    private void SetDestinationToCurrentWaypoint()
    {
        if (CurrentWaypoint != null && _aiPath != null)
        {
            _aiPath.destination = CurrentWaypoint.position;
            StartCoroutine(nameof(ShowRoute));
        }
    }

    private void MoveToNextWaypoint()
    {
        if (_currentWaypointIndex >= _waypoints.Count - 1)
        {
            OnReachedFinalDestination();
            return;
        }
        _currentWaypointIndex++;
        SetDestinationToCurrentWaypoint();
    }
    // --------------------------------------------------------------------

    // Follow Mode -------------------------------------------------------
    private bool _isFollowing = false;

    [SerializeField] private Transform _movingTarget;
    public Transform MovingTarget => _movingTarget;

    public void EnableFollow(Transform movingTarget)
    {
        if (movingTarget == null)
        {
            //Debug.LogWarning($"[Agent] Attempted to follow null target for {name}");
            return;
        }
        if (_isFollowing)
        {
            //Debug.LogWarning($"[Agent] {name} is already following a target");
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
            //Debug.LogWarning($"[Agent] {name} is not currently following a target");
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
        if (_aiPath == null) _aiPath = GetComponent<AIPath>();
        if (_seeker == null) _seeker = GetComponent<Seeker>();
        if (_destinationSetter == null) _destinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Awake()
    {
        GetComponents();
        if (_waypoints.Count > 0) SetDestinationToCurrentWaypoint();
    }

    private void Update()
    {
        if (_aiPath == null) return;
        if (_isFollowing) return; // follow mode overrides waypoints
        if (!HasWaypoints) return;

        if (_aiPath.reachedEndOfPath != _hasReachedCurrentWaypoint)
        {
            _hasReachedCurrentWaypoint = _aiPath.reachedEndOfPath;
            if (_hasReachedCurrentWaypoint) MoveToNextWaypoint();
        }
    }

    private IEnumerator ShowRoute()
    {
        if (_seeker != null)
        {
            _seeker.drawGizmos = true;
            yield return new WaitForSeconds(_showRouteDuration);
            _seeker.drawGizmos = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_isFollowing && _movingTarget != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, _movingTarget.position);
            Gizmos.DrawWireSphere(_movingTarget.position, 0.4f);
        }

        if (_waypoints == null || _waypoints.Count == 0 || _isFollowing) return;

        Gizmos.color = Color.yellow;
        for (int i = _currentWaypointIndex; i < _waypoints.Count; i++)
        {
            if (_waypoints[i] == null) continue;
            if (i == _currentWaypointIndex)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_waypoints[i].position, 0.8f);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_waypoints[i].position, 0.5f);
            }

            // Draw lines between _waypoints
            if (i > _currentWaypointIndex)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_waypoints[i - 1].position, _waypoints[i].position);
            }
        }

        // Draw line from agent to current target
        if (CurrentWaypoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, CurrentWaypoint.position);
        }
    }
}
