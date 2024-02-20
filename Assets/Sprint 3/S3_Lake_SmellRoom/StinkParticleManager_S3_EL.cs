/* StinkParticleManager.cs
 * Gabe Ahrens
 *
 * controls the spawning of stink particles
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkParticleManager_S3_EL : MonoBehaviour
{
    
    private float stinkTimer = 0;

    [Header("Stinker Settings")]
    [SerializeField]
    private float secondsTillStink = 1;

    [Header("Stink Particle Settings")]
    [SerializeField]
    private GameObject stinkParticle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stinkTimer < secondsTillStink)
            stinkTimer += Time.deltaTime;

        if (stinkTimer >= secondsTillStink)
        {
            makeStink();

            stinkTimer = 0;
        }
    }

    // FixedUpdate is called 50 times a second
    private void FixedUpdate()
    {
        
    }

    private void makeStink()
    {
        //Debug.Log("Stink Made");

        Instantiate(stinkParticle, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);

    }


}
