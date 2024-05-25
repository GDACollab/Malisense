using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class insense_use : MonoBehaviour
{
    //No scent system is in place, replace the placeholder vairable with it when implemented
    int scentPlaceholder = 420;
    public scr_noise noiseMaker;
    public float noiseSize = 5;

    // Start is called before the first frame update
    void Start()
    {
        scentPlaceholder -= 210;
        noiseMaker.MakeSound(transform.position, noiseSize);
        GameObject.Find("Global Teapot").GetComponent<AudioManager>().PlayElixirSFX();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
