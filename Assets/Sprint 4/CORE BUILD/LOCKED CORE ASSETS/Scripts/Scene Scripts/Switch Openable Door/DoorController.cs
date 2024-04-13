using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, ISwitchable
{
    // Defines 2 door behaviors: SingleSwitch and MultiSwitch
    private enum DType
    {
        SingleSwitch,
        MultiSwitch,
    }

    [SerializeField] private bool startOpen = false;
    [SerializeField] [Tooltip("#SingleSwitch: any switch that references the door will open/close it on one press. \n #MultiSwitch: MultiSwitchCounter amount of switches that reference the door need to be activated to open/close the door.")] private DType DoorType = DType.SingleSwitch;
    [SerializeField] [Tooltip("(Unused for SingleSwitch doors)This is the amount of switches that need to be activated when DoorType is set to MultiSwitch.")] private int MultiSwitchCounter = 2;

    private bool doorstate;
    private SpriteRenderer doorSprite;
    private Collider2D doorCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that the door is initially closed
        doorSprite = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        SetDoor(startOpen);
    }

    /* Defines ISwitchable SwitchInteract function defined by 2 types
     * -(if enum DoorType = SingleSwitch) any switch that references the door will change its state on activation
     * -(if enum DoorType = MultiSwitch) any switch that references the door will subtract MultiSwitch door counter
     * by 1 on activation and increase it by one when not activated. When MultiCounter is equal to or less than 0
     * the door will change state from startOpen.
     */

    public void SwitchInteract(bool Activated)
    {
        switch (DoorType) {
            case (DType.SingleSwitch):

                SetDoor(!doorstate);
                break;

            case (DType.MultiSwitch):

                if(Activated) MultiSwitchCounter--;
                else MultiSwitchCounter++;

                if (MultiSwitchCounter <= 0) SetDoor(!startOpen);
                else SetDoor(startOpen);
                break;

        }
    }
    // Open or Closes door depending on boolean open.
   
    private void SetDoor(bool open){

        doorstate = open;
        doorSprite.enabled = !open;
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