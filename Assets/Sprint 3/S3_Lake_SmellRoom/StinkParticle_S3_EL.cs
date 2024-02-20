using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkParticle_S3_EL : MonoBehaviour
{
    
    [Header("Particle Settings")]
    [SerializeField]
    private float stinkValue = 15;
    private float stinkDepletionRate = 3;

    [Header("Stink Particle Prefab")]
    [SerializeField]
    private GameObject stinkParticle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        stinkValue += -(stinkDepletionRate/50);

        if (stinkValue <= 0)
        {
            Debug.Log("Stink destroyed");
            Destroy(stinkParticle);
        }
    }
}
