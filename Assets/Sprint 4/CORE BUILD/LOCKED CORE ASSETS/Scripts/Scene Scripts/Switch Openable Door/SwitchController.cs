using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class SwitchController : MonoBehaviour
{
    // ### PLEASE FIX SCRIPT AFTER VS ###
    
    [SerializeField] private bool oneTimeSwitch = false;
    [SerializeField] private bool startActivated = false;
    [SerializeField] [Tooltip("Press these switches when this switch is pressed")] private SwitchController[] syncSwitches;
    [SerializeField] [Tooltip("These object will do there defined behavior when switch is pressed.")] private DoorController[] doors;
    public LampController lamp;
    private bool isActivated = false;
    private SpriteRenderer switchSprite;

    private void Start() {
        switchSprite = GetComponent<SpriteRenderer>();
        isActivated = startActivated;
        if(isActivated){
            lamp.TurnOn();
            switchSprite.flipY = true;
        }
    }
    
    // Called when the switch is clicked or activated
    public void ActivateSwitch()
    {
        if(isActivated && !oneTimeSwitch){
            lamp.TurnOff();
            switchSprite.flipY = false;
            isActivated = false;
            foreach(var nswitch in syncSwitches){
                nswitch.ForceToggleSwitch();
            }
            //Iterate through each target (most likely a door) and calls 
            foreach (var door in doors)
            {
                door.SwitchInteract();
            }
        }
        else if (!isActivated){
            lamp.TurnOn();
            switchSprite.flipY = true;
            // Toggle the switch state
            isActivated = true;
            foreach(var nswitch in syncSwitches){
                nswitch.ForceToggleSwitch();
            }
            foreach (var door in doors)
            {
                door.SwitchInteract();
            }
        }
        

        //ADD SOUND EFFECT HERE
        // You can add a visual feedback or animation for switch activation here
    }
    
    public void ForceToggleSwitch(){
        if(isActivated){
            lamp.TurnOff();
            switchSprite.flipY = false;
            isActivated = false;
        }
        else{
            lamp.TurnOn();
            switchSprite.flipY = true;
            isActivated = true;
        }
    }

    // Check if the switch is activated
    public bool IsActivated()
    {
        return isActivated;
    }
}