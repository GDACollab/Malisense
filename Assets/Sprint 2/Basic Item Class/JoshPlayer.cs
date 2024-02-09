using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoshPlayer : MonoBehaviour
{
    public PlayerInput input;
    public InputAction interactAction;
    public InputAction setDownAction;
    public Rigidbody2D rb;
    public GameObject triangle;
    
    // TODO add drop Big item IA
    
    // example implementation of inventory
    [SerializeField, Category("Inventory")]
    public InventoryBase.Inventory newInventory;
        
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        triangle = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        interactAction = input.actions.FindAction("Interact");
        setDownAction = input.actions.FindAction("SetDown");
    }

    // Update is called once per frame
    void Update()
    {
        if (!newInventory.carriedObject)
        {
            return;
        }

        if (setDownAction.ReadValue<float>() > 0f)
        {
            if (newInventory.carriedObject)
            {
                newInventory.carriedObject.transform.parent = null;
                newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                newInventory.carriedObject = null;
            }
        }

        if (newInventory.carriedObject)
        {
            newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            newInventory.carriedObject.transform.parent = triangle.transform.parent;
            newInventory.carriedObject.transform.position = triangle.transform.position;
            newInventory.carriedObject.transform.rotation = triangle.transform.rotation;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (interactAction.triggered)
        {
            var item = other.GetComponent<ItemPickup>();
            var heavyItem = other.GetComponent<HeavyItem>();
            var door = other.GetComponent<Door>();

            if (door && newInventory.carriedObject)
            {
                Destroy(newInventory.carriedObject.gameObject);
                newInventory.carriedObject = null;
                Destroy(other.gameObject);
                return;
            }
            
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
