using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class SwitchController : MonoBehaviour
{
    public LampController lamp;
    private bool isActivated = false;


    // Called when the switch is clicked or activated
    public void ActivateSwitch()
    {

        lamp.TurnOn();
        // Toggle the switch state
        isActivated = true;

        //ADD SOUND EFFECT HERE
        // You can add a visual feedback or animation for switch activation here
    }   

    // Check if the switch is activated
    public bool IsActivated()
    {
        return isActivated;
    }
}