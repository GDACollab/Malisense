using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class MagicHandScript : MonoBehaviour
{
    //Magic hand can be controlled to move around ala the player, so it needs to spawn a new object that you control instead of the player for a short period

    //This script is the one run when the artifact is used, and spawns (and despawns) the hand that the player controls

    public float MagicHandCooldown;
    public float MagicHandDuration;
    public GameObject controllableHand;
    public Artifact MagicHand;
    GameObject spawnedObject;
    GameObject player;
    //float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("in the hand");
        //below line (just the one) literally copied and pasted from WhisperingBellArtifact.cs
        player = GameObject.FindGameObjectWithTag("Player");
        if (MagicHand.duration > 0.0f || MagicHand.cooldown > 0.0f) {
            if (MagicHand.duration > 0.0f)
            {
                //Get rid of the prev hand
                GameObject prevHand = GameObject.FindGameObjectWithTag("Magic Hand");
                Destroy(prevHand);
                MagicHand.cooldown = MagicHandCooldown * (1 - (MagicHand.duration / MagicHandDuration)); //cooldown inversely proportional to how long hand has been out
                MagicHand.duration = 0.0f;
            }
            player.GetComponent<Player>().canMove = true;
            Destroy(gameObject);
        }
        //stealPlayerInput();
        //MagicHand.duration = MagicHandDuration;
    }

    //(modified from whispering bell artifact code for consistency)
    // When using editor, clears all previous runtime values of the object
    private void OnValidate()
    {
        MagicHand.duration = 0.0f;
        MagicHand.cooldown = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (MagicHand.duration == 0 && MagicHand.cooldown == 0) //first frame of existing
        {
            //Debug.Log("starting");
            stealPlayerInput();
            MagicHand.duration = MagicHandDuration;
        }
        else if (MagicHand.duration > 0)
        {
            //Debug.Log("decr duration");
            /*
            timer += Time.deltaTime;
            if(timer > MagicHandDuration)
            {
                timer = 0;
                returnPlayerInput();
            }
            */
            MagicHand.duration -= Time.deltaTime;
            if (MagicHand.duration <= 0)
            {
                MagicHand.duration = 0;
                returnPlayerInput();
            }
        }
        else if (MagicHand.cooldown >= 0)
        {
            //Debug.Log("decr cooldown");
            MagicHand.cooldown -= Time.deltaTime;
            if(MagicHand.cooldown <= 0)
            {
                MagicHand.cooldown = 0;
                Destroy(gameObject);
            }
        }
        //Debug.Log("why is cooldown not saying it's >= 0");
    }

    void stealPlayerInput()
    {
        Debug.Log("Stealing Player input");
        player.GetComponent<Player>().canMove = false;
        spawnedObject = Instantiate(controllableHand, player.transform.position, Quaternion.identity);
    }

    void returnPlayerInput()
    {
        Debug.Log("Returning Player input");
        player.GetComponent<Player>().canMove = true;
        MagicHand.cooldown = MagicHandCooldown;
        Destroy(spawnedObject);
        //Destroy(gameObject);
    }
}
