using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput input;
    public InputAction interactAction;
    
    // string is name of item, int is amount in inv
    public InventoryBase.Inventory playerInv;
        
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        interactAction = input.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (interactAction.ReadValue<float>() > 0f)
        {
            
            var item = other.GetComponent<ItemPickup>();

            if (!item) return;
            
            bool success = playerInv.AddItem(item.item, 1);
            if (success)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
