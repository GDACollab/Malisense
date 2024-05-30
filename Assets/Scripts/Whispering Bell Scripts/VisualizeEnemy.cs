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
        Transform targetEnemy = transform.parent;
        wbTime += Time.deltaTime;
        Vector3 targetPos = targetEnemy.position + Vector3.up;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3f);
        if (wbTime >= wbDuration)
        {
            GetComponent<ParticleSystem>().Stop();
        }
        else if (wbTime >= wbDuration + wbDuration)
        {
            Destroy(gameObject);
        }
    }
}
