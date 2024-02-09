using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject enemy = GameObject.Find("Test Enemy");
        Vector3 pos = transform.position;
        enemy.GetComponent<EnemyPatrol>().waypointsL.Add(pos);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
