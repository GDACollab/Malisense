using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_noise : MonoBehaviour
{
    //In case I'm unable to work on this:
    //When a character move, it creates a sound circle around it
    //size of circle is based on movement type
    //a command from this script gets called, creating the object on top of the character that moved
    //If the circle hits a sound monster, it gets alerted

    public GameObject noiseObject;

    public bool noiseDistractsSound;

    // Start is called before the first frame update
    void Start()
    {
        noiseObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Call whenever a sound is made
    //pos is the location the sound originates, size is the diameter of the area in which the sound can be heard
    public void MakeSound(Vector3 pos, float size)
    {
        GameObject noise;
        noise = Instantiate(noiseObject,pos,Quaternion.identity);
        noise.GetComponent<scr_noiseObject>().noiseDistractsSound = noiseDistractsSound;
        //Warning: if the object has no scr_noiseObject, then the game will crash
        noise.GetComponent<scr_noiseObject>().diameter = size; //This grabs the component scr_noiseObject in noise, and sets the diamter variable in it to size
        noise.GetComponent<scr_noiseObject>().parent = gameObject;

    }
}
