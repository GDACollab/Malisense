using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class SwitchController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color activatedColor = Color.green;
    public LampController lamp;
    private bool isActivated = false;

    //Compare the Player prefab transform postion to the switch position and if they are close enough,
    //activate the switch, the switch cannot be turned off
    void Update()
    {
        if ((Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < 1.5f) && isActivated == false)
        {
            ActivateSwitch();
        }
    }

    // Called when the switch is clicked or activated
    public void ActivateSwitch()
    {

        lamp.TurnOn();
        // Toggle the switch state
        isActivated = true;
        spriteRenderer.color = activatedColor;

        //ADD SOUND EFFECT HERE
        // You can add a visual feedback or animation for switch activation here
    }   

    // Check if the switch is activated
    public bool IsActivated()
    {
        return isActivated;
    }
}