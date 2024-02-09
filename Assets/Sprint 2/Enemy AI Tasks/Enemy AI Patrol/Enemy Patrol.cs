using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    float enemyPatrolSpeed;
    public List<Vector3> waypointsL = new  List<Vector3>(); // Points to make patrol path
    Vector3 targetPos;
    Vector3 velocity;
    int index;
    int length;
    
    void Start()
    {
        targetPos = waypointsL[index];
        enemyPatrolSpeed = 1.5f;
        length = waypointsL.Count;
        index = length - 1;
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
            if (index > 0)
            {
                index--;
            }
            else
            {
                index = length - 1;
            }
            targetPos = waypointsL[index];
        }
        // Finds the vector the enemy needs to get to the next waypoint
        velocity = Vector3.MoveTowards(transform.position, targetPos, enemyPatrolSpeed * Time.deltaTime); 
    }

    private void move()  // Moves enemy
    {
        transform.position = velocity;
    }
}
