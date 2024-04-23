using Pathfinding;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Seeker))]
public class EnemyPathfinder : MonoBehaviour
{
    public float acceleration = 10f;
    public float waypointDistance = 0.5f;
    public float goalDistance = 0.25f;
    public float recalculateInterval = 0.5f;
    public LayerMask obstacleLayers;
    public Transform visuals;

    [HideInInspector]
    public Vector2 direction;

    private Vector2 _targetPosition;
    private Path _path;
    private int _pathWaypoint;
    private Seeker _seeker;

    private Rigidbody2D _rb2d;

    public bool AtGoal => Vector2.Distance(_rb2d.position, _targetPosition) < goalDistance;

    public void SetTarget(Vector2 point)
    {
        _targetPosition = point;
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _seeker = GetComponent<Seeker>();

        _targetPosition = _rb2d.position;

        StartCoroutine(UpdatePath());
    }

    private IEnumerator UpdatePath()
    {
        // Recalculate path on an interval
        var delay = new WaitForSeconds(recalculateInterval);

        while (true)
        {
            if (AtGoal || !_seeker.IsDone())
            {
                yield return null;
            }
            else
            {
                _seeker.StartPath(_rb2d.position, _targetPosition, OnPathComplete);
                yield return delay;
            }
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _pathWaypoint = 0;
        }
    }

    private void MoveTowards(Vector2 target)
    {
        direction = (target - _rb2d.position).normalized;
        Vector2 force = direction * acceleration;

        _rb2d.AddForce(force);

        // Turn sprite
        if (Mathf.Abs(target.x - _rb2d.position.x) >= 0.01f)
        {
            visuals.localScale = new Vector3(Mathf.Sign(_rb2d.position.x - target.x), 1f, 1f);
        }
    }

    private void AdvancePathWaypoint()
    {
        if (_path == null)
            return;

        // Skip waypoints within a threshold distance
        while (_pathWaypoint < _path.vectorPath.Count
            && Vector2.Distance(_rb2d.position, _path.vectorPath[_pathWaypoint]) < waypointDistance)
        {
            _pathWaypoint++;
        }

        // If exceeded current amount of waypoints in the path
        if (_pathWaypoint >= _path.vectorPath.Count)
        {
            _path = null;
            return;
        }
    }

    private RaycastHit2D[] _hitResults = new RaycastHit2D[1];

    private void FixedUpdate()
    {
        if (AtGoal)
            return;

        AdvancePathWaypoint();

        int hits = _rb2d.Cast(
            direction: (_targetPosition - _rb2d.position).normalized,
            results: _hitResults,
            distance: Vector2.Distance(_rb2d.position, _targetPosition)
        );

        if (hits == 0)
        {
            // Accelerate directly towards target
            MoveTowards(_targetPosition);
        }
        else
        {
            // Accelerate towards waypoint
            if (_path != null)
                MoveTowards(_path.vectorPath[_pathWaypoint]);
        }
    }
}
