using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    float enemyPatrolSpeed;
    Vector3[] waypoints = new Vector3[3]; // Points to make patrol path
    Vector3 targetPos;
    Vector3 velocity;
    int index = 0;
    
    void Start()
    {
        waypoints[0] = new Vector3(3, 0, 0);
        waypoints[1] = new Vector3(3, 3, 0);
        waypoints[2] = new Vector3(0, 0, 0);
        targetPos = waypoints[index];
        enemyPatrolSpeed = 1.5f;
    }

    
    void Update()
    {
        getMoveVector();
        move();
    }

    private void getMoveVector()
    {
        if (transform.position == targetPos) // Goes to the next waypoint if the current one is reached
        {
            if (index < 2)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            targetPos = waypoints[index];
        }
        // Finds the vector the enemy needs to get to the next waypoint
        velocity = Vector3.MoveTowards(transform.position, targetPos, enemyPatrolSpeed * Time.deltaTime); 
    }

    private void move()  // Moves enemy
    {
        transform.position = velocity;
    }
}
