using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Tracks how long the effect should last
public class VisualizeEnemy : MonoBehaviour
{
    [Tooltip("Determine how long the Whispering Bell effects last")]
    [SerializeField] public float wbDuration;

    public Transform targetEnemy; 
    private float wbTime = 0;

    // Update is called once per frame
    void Update()
    {
        wbTime += Time.deltaTime;
        // Need to update to follow enemy
        if (wbTime >= wbDuration) Destroy(gameObject);
    }
}
