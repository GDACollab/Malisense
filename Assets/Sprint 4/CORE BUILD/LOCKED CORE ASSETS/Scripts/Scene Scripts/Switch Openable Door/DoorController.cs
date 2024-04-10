using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, SwitchInteractable
{    
    // ### PLEASE FIX SCRIPT AFTER VS ###
    
    [SerializeField] private bool startOpen = false;

    private bool doorstate;
    private SpriteRenderer doorSprite;
    private Collider2D doorCollider;
    //private int activatedSwitchCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that the door is initially closed
        doorSprite = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        SetDoor(startOpen);
    }

    public void SwitchInteract()
    {
        SetDoor(!doorstate);
    }

    // Update is called once per frame
    /*
    void Update()
    {
        // Check for switch activation
        CheckSwitches();


        // Check if all switches are activated to open the door
        if (activatedSwitchCount == switches.Length)
        {
            OpenDoor();
        }
        else
        {
            //Reset the activated switch count
            activatedSwitchCount = 0;
            CloseDoor();
        }
    }

    void CheckSwitches()
    {
        activatedSwitchCount = 0;
        foreach (GameObject switchObj in switches)
        {
            SwitchController switchController = switchObj.GetComponent<SwitchController>();
            if (switchController != null && switchController.IsActivated())
            {
                // Increase the count of activated switches
                activatedSwitchCount++;
            }
        }
    }
    */
    private void SetDoor(bool open){

        doorstate = open;
        doorSprite.enabled = !open;
        doorCollider.enabled = !open;

    }

    void OpenDoor()
    {
        // You can implement the door opening animation or any other door opening logic here
        doorstate = true;
        doorSprite.enabled = false; // Deactivate the door or set its state to "open"
        doorCollider.enabled = false;
    }

    void CloseDoor()
    {
        // You can implement the door closing animation or any other door closing logic here
        doorstate = false;
        doorSprite.enabled = true; // Activate the door or set its state to "closed"
        doorCollider.enabled = true;
    }
}