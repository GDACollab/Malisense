using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAlert : StateBaseClass
{
    // Start is called before the first frame update
    public Transform target;

    public float speed = 100f;              // Acceleration speed
    public float nextWaypointDistance = 1f; // How close an enemy needs to be to the waypoint to move on to next one
    public float circleRadius = 1f;

    public Transform GFX;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath;
    private bool onCircumference;
    private int circumferenceIndex;
    private Vector2 center;
    private Vector2[] circlePoints = { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };

    Seeker seeker;
    Rigidbody2D rb;

    public override void Init()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        center = new Vector2(target.position.x,target.position.y);
        seeker.StartPath(rb.position, target.position, OnPathComplete);
        reachedEndOfPath = false;
        onCircumference = false;
        circumferenceIndex = 0;
    }


    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;    // Start at beginning of new path
        }
    }

    public override void On_Update()
    {
        
        if (path == null)
            return;


        if (onCircumference)
        {
            Vector2 circleDirection = center + circleRadius * circlePoints[circumferenceIndex] - rb.position;
            rb.velocity = circleDirection.normalized * speed * Time.deltaTime;
            if (circleDirection.magnitude < 0.02)
            {
                circumferenceIndex = (circumferenceIndex + 1) % circlePoints.Length;
            }
            return;
        }

        if (reachedEndOfPath)
        {
            
            Vector2 upwards = new Vector2(0, 1);
            rb.velocity = upwards * speed * Time.deltaTime;
            float centerDistance = Vector2.Distance(rb.position, center);
            if (centerDistance >= circleRadius || rb.velocity.y <= 0.2)
            {
                onCircumference = true;
            }
            return;
        }

        // If exceeded current amount of waypoints in the path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        // Find direction to waypoint
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;  // Move on to next waypoint
        }

        flipSprite(force);
        
    }

    void flipSprite(Vector2 force)
    {
        // ----- Flip Sprite -----
        // Flip left
        if (force.x >= 0.01f)
        {
            GFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        // Flip right
        else if (force.x <= -0.01f)
        {
            GFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
