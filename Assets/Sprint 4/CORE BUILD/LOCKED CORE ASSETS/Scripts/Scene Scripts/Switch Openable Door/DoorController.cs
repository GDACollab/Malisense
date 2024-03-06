using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{    
    [SerializeField] private bool startOpen = false;
    public GameObject door;
    public GameObject[] switches;

    private int activatedSwitchCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that the door is initially closed
        door = this.gameObject;
        SetDoor(startOpen);
    }

    // Update is called once per frame
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
    
    private void SetDoor(bool open){
        if(open){
            OpenDoor();
        }
        else{
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        // You can implement the door opening animation or any other door opening logic here
        door.SetActive(false); // Deactivate the door or set its state to "open"
    }

    void CloseDoor()
    {
        // You can implement the door closing animation or any other door closing logic here
        door.SetActive(true); // Activate the door or set its state to "closed"
    }
}