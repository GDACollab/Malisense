using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private bool oneTimeSwitch = false;
    [SerializeField] private bool startActivated = false;
    public LampController lamp;
    private bool isActivated = false;

    private void Start() {
        isActivated = (!oneTimeSwitch) ? startActivated : false;
    }
    
    // Called when the switch is clicked or activated
    public void ActivateSwitch()
    {
        if(isActivated && !oneTimeSwitch){
            lamp.TurnOff();
            isActivated = false;
        }
        else{
            lamp.TurnOn();
            // Toggle the switch state
            isActivated = true;
        }

        //ADD SOUND EFFECT HERE
        // You can add a visual feedback or animation for switch activation here
    }   

    // Check if the switch is activated
    public bool IsActivated()
    {
        return isActivated;
    }
}