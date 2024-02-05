using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoshPlayer : MonoBehaviour
{
    public PlayerInput input;
    public InputAction interactAction;
    public Rigidbody2D rb;
    
    // TODO add drop Big item IA
    
    // example implementation of inventory
    [SerializeField, Category("Inventory")]
    public InventoryBase.Inventory newInventory;
        
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        interactAction = input.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (!newInventory.carriedObject)
        {
            return;
        }

        newInventory.carriedObject.transform.position = transform.position;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (interactAction.ReadValue<float>() > 0f)
        {
            var item = other.GetComponent<ItemPickup>();
            var heavyItem = other.GetComponent<HeavyItem>();

            if (!item)
            {
                if (!heavyItem)
                {
                    return;
                }

                newInventory.carriedObject = heavyItem;
                return;    
            };

            bool success = newInventory.AddItem(item.item, 1);
            if (success)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
