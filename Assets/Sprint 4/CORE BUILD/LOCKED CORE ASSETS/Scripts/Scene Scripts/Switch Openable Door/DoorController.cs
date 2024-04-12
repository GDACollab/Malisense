using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, ISwitchable
{
    // ### PLEASE FIX SCRIPT AFTER VS ###
    private enum DType
    {
        SingleSwitch,
        MultiSwitch,
    }

    [SerializeField] private bool startOpen = false;
    [SerializeField] private DType DoorType = DType.SingleSwitch;
    [SerializeField] private int MultiSwitchCounter = 2;

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

    public void SwitchInteract(bool state)
    {
        switch (DoorType) {
            case (DType.SingleSwitch):

                SetDoor(!doorstate);
                break;

            case (DType.MultiSwitch):

                if(state) MultiSwitchCounter--;
                else MultiSwitchCounter++;

                if (MultiSwitchCounter <= 0) SetDoor(!startOpen);
                else SetDoor(startOpen);
                break;

        }
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
        doorSprite.enabled = !open; // Deactivate the door or set its state to "open"
        doorCollider.enabled = !open;

    }

    public void OpenDoor()
    {
        // You can implement the door opening animation or any other door opening logic here
        SetDoor(true);
    }

    public void CloseDoor()
    {
        // You can implement the door closing animation or any other door closing logic here
        SetDoor(false);
    }
}