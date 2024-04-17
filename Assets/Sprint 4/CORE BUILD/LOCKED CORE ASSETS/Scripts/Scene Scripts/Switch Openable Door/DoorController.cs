using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DoorController : MonoBehaviour, ISwitchable
{
    // Defines 2 door behaviors: OnActivation and OnAllActivated
    private enum DType
    {
        OnActivation,
        OnAllActivated,
    }

    [SerializeField] [Tooltip("Initial state of the door or whether the door starts open or closed")] private bool startOpen = false;
    [SerializeField] [Tooltip("#OnActivation: any switch that targets the door will open/close it on one press. " +
        "\n #OnAllActivated: All switches that target the door need to be activated at once to open/close the door.")] 
    private DType DoorType = DType.OnActivation;
    [SerializeField] public List<SwitchController> SwitchList;

    private List<SwitchController> OldSwitchList;  
    private int OnAllCounter;
    private bool doorstate;
    private SpriteRenderer doorSprite;
    private Collider2D doorCollider;

    
    void OnValidate()
    {
        OldSwitchList ??= new List<SwitchController>();
        foreach (var s in OldSwitchList)
        {
            if (s == null) continue;
            else if (!SwitchList.Contains(s)) s.RemoveTarget(this);
            
        }
        foreach (var s in SwitchList)
        {
            if (s == null) continue;
            else if (!OldSwitchList.Contains(s)) s.AddTarget(this);
        }
        OldSwitchList = new List<SwitchController>(SwitchList);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure that the door is initially closed
        doorSprite = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        SetDoor(startOpen);
    }

    /* Defines ISwitchable SwitchInit
     * 
     */
    public void SwitchInit(bool activated)
    {

        if (DoorType == DType.OnAllActivated)
        {
            if (!activated) OnAllCounter++;
        }
    }
    /* Defines ISwitchable SwitchInteract function defined by 2 types
 * -(if enum DoorType = OnActivation) any switch that references the door will change its state on activation
 * -(if enum DoorType = OnAllActivated) any switch that references the door will subtract OnAllActivated door counter
 * by 1 on activation and increase it by one when not activated. When MultiCounter is equal to or less than 0
 * the door will change state from startOpen.
 */

    public void SwitchValidateAdd(SwitchController sw)
    {
        OldSwitchList ??= new List<SwitchController>();
        if (!SwitchList.Contains(sw))
        {
            SwitchList.Add(sw);
            OldSwitchList.Add(sw);
        }
        //OldSwitchList = SwitchList;

    }
    public void SwitchValidateRemove(SwitchController sw)
    {

        if (SwitchList.Contains(sw))
        {
            SwitchList.Remove(sw);
            OldSwitchList.Remove(sw);
        }

    }
    public void SwitchInteract(bool activated)
    {
        switch (DoorType) {
            case (DType.OnActivation):

                SetDoor(!doorstate);
                break;

            case (DType.OnAllActivated):

                if(activated) OnAllCounter--;
                else OnAllCounter++;

                if (OnAllCounter <= 0) SetDoor(!startOpen);
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