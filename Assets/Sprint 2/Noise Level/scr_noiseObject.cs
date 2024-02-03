using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_noiseObject : MonoBehaviour
{
    //When sound object made:
    //start at size 0,0,1
    //increase x and y size to a 'max' over time
    //After hitting max, quickly fade, and delete self

    public Transform self;
    public float growthTime = 0.25f; //Time to grow to max
    public float deathTime = 0; //Time to fade into deletion after hitting max
    public float diameter = 4;
    public float timeStore = 0;

    // Start is called before the first frame update
    void Start()
    {
        self.transform.localScale = new Vector3(0,0,1);
    }

    // Update is called once per frame
    void Update()
    {
        //Calls deletion check first to give a single frame of lenience for enemies to check player presence if there is intense lag
        if(timeStore > growthTime + deathTime)
        {
            Destroy(gameObject);
        }
        //Increase time, and scale circle to match how much time has passed
        timeStore += Time.deltaTime;
        self.transform.localScale = new Vector3(Mathf.Min(diameter, diameter * (timeStore / growthTime)), Mathf.Min(diameter, diameter * (timeStore / growthTime)), 1);
    }
}
