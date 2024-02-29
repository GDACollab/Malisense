using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : StateBaseClass
{
    [SerializeField]
    public float speed = 5;
    
    // x: x value of waypoint
    // y: y value of waypoint
    // z: time in seconds to wait at that waypoint
    public Vector3[] waypoints;
    private Vector3 targetPos;
    private Vector2 velocity;
    private int index;
    private bool waiting;   

    IEnumerator lookAround(float time)
    {
        waiting = true;

        // put animation code here
        yield return new WaitForSeconds(time);

        waiting = false;
    }
    public override void Init(){
        waiting = false;
        index = 0;
        targetPos = waypoints[index];
    }
    public override void On_Update(){
        if ((Vector2)transform.position == (Vector2)targetPos) // Goes to the next waypoint if the current one is reached
        {
            StartCoroutine(lookAround(targetPos.z));
            if (index < waypoints.Length)
            {
                index++;
            }
            index = index % waypoints.Length;
            StartCoroutine(lookAround(targetPos.z));
            targetPos = waypoints[index];
        }

        if(!waiting)
        {
            velocity = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); 
        }

        transform.position = velocity;

        // face towards target
        Vector3 diff = targetPos - transform.position;
        diff.Normalize();
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90);
    }
}
