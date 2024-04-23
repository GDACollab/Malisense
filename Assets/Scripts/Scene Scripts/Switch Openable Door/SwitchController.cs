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
 * Implements an interface to be used for all objects that are interactable with the switch.
 * This is done to make the utility of the switch expandable in the future in case this switch will have multiple uses beyond doors. 
 * All you need to do is add ISwitchable to any MonoBehavior class inheriting MonoBehavior in the following syntax: class classname: MonoBehavior, ISwitchable
 * Then define the function of SwitchInteract(bool Activated) in that class. Afterward you can add that object to targets and the SwitchInteract function will trigger.
 */
public interface ISwitchable
{
    void SwitchInit(bool activated);
    void SwitchInteract(bool activated);
}




public class SwitchController : MonoBehaviour
{

    [SerializeField] private bool oneTimeSwitch = false;
    [SerializeField] private bool startActivated = false;
    [SerializeField] [Tooltip("Press these switches when this switch is pressed. (Leave this empty for OnAllActivated Doors)")] private SwitchController[] syncSwitches;
    [SerializeField] [Tooltip("These objects (currently just a doorcontroller) will do there defined behavior when switch is pressed (Most likely closing/opening a door).")] private List<MonoBehaviour> targets;
    public LampController lamp;
    private bool isActivated = false;
    private SpriteRenderer switchSprite;

    //Remove non ISwitchable objects from targets
    private void OnValidate()
    {
        targets.RemoveAll(NotSwitchable);
    }

    //Checks if target is a Not a member of ISwitchable while also not being null
    public bool NotSwitchable(MonoBehaviour target) => Swable(target) == null && target != null;
    

    //Converts target to a ISwitcable interface object, null if not
    public ISwitchable Swable(MonoBehaviour target) => target as ISwitchable;



    private void Start()
    {
        switchSprite = GetComponent<SpriteRenderer>();
        isActivated = startActivated;
        if (isActivated)
        {
            lamp.TurnOn();
            switchSprite.flipY = true;
        }
        //Initialiises all SwitchTargets, currently used to set OnAllActivated doors to
        foreach (var target in targets) Swable(target)?.SwitchInit(isActivated);

    }

    // Called when the switch is clicked or activated
    public void ActivateSwitch()
    {
        if (isActivated && !oneTimeSwitch)
        {
            // Flips self
            FlipSwitch();

            //Flips activations in all switches in syncswitches
            foreach (var nswitch in syncSwitches) nswitch.FlipSwitch();

            //Iterate through each target that implements ISwitchable interface (just doorcontrollers atm)
            //Calls SwitchInteract on each target
            foreach (var target in targets) Swable(target)?.SwitchInteract(isActivated);

        }
        else if (!isActivated)
        {
            // Flips self
            FlipSwitch();

            //Flips activations in all switches in syncswitches
            foreach (var nswitch in syncSwitches) nswitch.FlipSwitch();

            //Iterate through each target that implements ISwitchable interface (just doorcontrollers atm)
            //Calls SwitchInteract on each target

            foreach (var target in targets) Swable(target)?.SwitchInteract(isActivated);
            
        }


        //ADD SOUND EFFECT HERE
        // You can add a visual feedback or animation for switch activation here
    }

    //Sets switch to not activated and turns switch
    public void FlipSwitch()
    {
        SetSwitch(!isActivated);
    }

    // Check if the switch is activated
    public bool IsActivated() => isActivated;

    public void SetSwitch(bool activated)
    {
        if (activated)
        {
            lamp.TurnOn();
            switchSprite.flipY = true;
            isActivated = true;
        }
        else
        {
            lamp.TurnOff();
            switchSprite.flipY = false;
            isActivated = false;
        }
    }

}