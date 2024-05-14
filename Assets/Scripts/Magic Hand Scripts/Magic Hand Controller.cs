using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicHandController : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //below line (just the one) literally copied and pasted from WhisperingBellArtifact.cs
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().canMove = false;
        //player.GetComponent<Player>().magicHand = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
