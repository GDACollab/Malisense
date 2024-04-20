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
    void SwitchValidateAdd(SwitchController sw);
    void SwitchValidateRemove(SwitchController sw);
    void SwitchInteract(bool activated);
}


public class SwitchController : MonoBehaviour
{
    
    [SerializeField] private bool oneTimeSwitch = false;
    [SerializeField] private bool startActivated = false;
    [SerializeField] [Tooltip("Press these switches when this switch is pressed. (Leave this empty for OnAllActivated Doors)")] private SwitchController[] syncSwitches;
    [SerializeField] [Tooltip("These objects (currently just a doorcontroller) " +
        "will do there defined behavior when switch is pressed (Most likely closing/opening a door).")] public List<MonoBehaviour> targets;

    [HideInInspector] [SerializeField]private List<MonoBehaviour> oldtargets;
    public LampController lamp;
    private bool isActivated = false;
    private SpriteRenderer switchSprite;
    
    private void OnValidate()
    {
        Debug.Log("Switch: Validate Start");
        List<MonoBehaviour> ISwitches = new List<MonoBehaviour>(targets);

        if(oldtargets == null) oldtargets = new List<MonoBehaviour>();
        foreach (var target in oldtargets)
        {
            if (target == null) continue;
            else if (!targets.Contains(target))
            {
                ISwitchable t = target as ISwitchable;
                t?.SwitchValidateRemove(this);
            }
        }
        
        foreach (var target in targets)
        {
            if (target == null) continue;
            else if (target is not ISwitchable t) ISwitches.Remove(target);
            else if (!oldtargets.Contains(target))
            {
                t.SwitchValidateAdd(this);
            }
        }

        targets = new List<MonoBehaviour>(ISwitches);
        oldtargets = new List<MonoBehaviour>(targets);

    }
    
    private void Start() {
        switchSprite = GetComponent<SpriteRenderer>();
        isActivated = startActivated;
        if(isActivated){
            lamp.TurnOn();
            switchSprite.flipY = true;
        }
        //Initialiises all SwitchTargets, currently used to set OnAllActivated doors to
        foreach (var target in targets)
        {
            ISwitchable t = target as ISwitchable;
            t?.SwitchInit(isActivated);
        }
    }
    
    // Called when the switch is clicked or activated
    public void ActivateSwitch()
    {
        if(isActivated && !oneTimeSwitch){
            FlipSwitch();
            foreach(var nswitch in syncSwitches){
                nswitch.FlipSwitch();
            }
            //Iterate through each target that implements ISwitchable interface (just doorcontrollers atm)
            //Calls SwitchInteract on each target
            foreach (var target in targets)
            {
                ISwitchable t = target as ISwitchable;
                t?.SwitchInteract(isActivated);
            }

        }
        else if (!isActivated){
            FlipSwitch();
            foreach (var nswitch in syncSwitches){
                nswitch.FlipSwitch();
            }
            //Iterate through each target that implements ISwitchable interface (just doorcontrollers atm)
            //Calls SwitchInteract on each target
            foreach (var target in targets)
            {
                ISwitchable t = target as ISwitchable;
                t?.SwitchInteract(isActivated);
            }
        }
        

        //ADD SOUND EFFECT HERE
        // You can add a visual feedback or animation for switch activation here
    }
    
    public void FlipSwitch(){
        SetSwitch(!isActivated);
    }

    // Check if the switch is activated
    public bool IsActivated()
    {
        return isActivated;
    }

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
    bool IsSwitchable(MonoBehaviour s) 
    {
        if (s == null) return true;
        else return (s is ISwitchable);
    }
    public void RemoveTarget(MonoBehaviour target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
            oldtargets.Remove(target);
        }

    }
    public void AddTarget(MonoBehaviour target)
    {
        if (oldtargets == null) oldtargets = new List<MonoBehaviour>();
        if (!targets.Contains(target))
        {
            targets.Add(target);
            oldtargets.Add(target);
        }
    }
}