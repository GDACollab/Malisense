using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        var collider = GetComponent<Collider2D>();
        var astar = FindObjectOfType<AstarPath>();
        if (collider && astar)
        {
            astar.UpdateGraphs(collider.bounds);
        }
    }
}
