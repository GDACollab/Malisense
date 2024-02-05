using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private bool isActivated = false;

    // Called when the switch is clicked or activated
    public void ActivateSwitch()
    {
        // Toggle the switch state
        isActivated = !isActivated;

        // You can add a visual feedback or animation for switch activation here
    }

    // Check if the switch is activated
    public bool IsActivated()
    {
        return isActivated;
    }
}