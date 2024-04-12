using Ink.Parsed;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.InputSystem.Controls.AxisControl;

/*###Switch Interactable Interface###
 * Implements an interface to be used for all objects
 *
 */
public interface ISwitchable
{
    void SwitchInteract(bool state);
}


public class SwitchController : MonoBehaviour
{
    // ### PLEASE FIX SCRIPT AFTER VS ###
    
    [SerializeField] private bool oneTimeSwitch = false;
    [SerializeField] private bool startActivated = false;
    [SerializeField] [Tooltip("Press these switches when this switch is pressed")] private SwitchController[] syncSwitches;
    [SerializeField] [Tooltip("These object will do there defined behavior when switch is pressed.")] private MonoBehaviour[] targets;
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
            foreach (var target in targets)
            {
                ISwitchable t = target as ISwitchable;
                t?.SwitchInteract(isActivated);
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
            foreach (var target in targets)
            {
                ISwitchable t = target as ISwitchable;
                t?.SwitchInteract(isActivated);
            }
            //targets.ForEach(c => (c as ISwitchInteractable)?.SwitchInteract(isActivated));
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