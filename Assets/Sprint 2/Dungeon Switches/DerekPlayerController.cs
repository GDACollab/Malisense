using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DerekPlayerController : MonoBehaviour
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
        rb = GetComponent<Rigidbody2D>();
        triangle = transform.GetChild(0).gameObject;
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
                newInventory.carriedObject = null;
            }
        }

        if (newInventory.carriedObject)
        {
            newInventory.carriedObject.transform.position = triangle.transform.position;
            newInventory.carriedObject.transform.rotation = triangle.transform.rotation;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (interactAction.ReadValue<float>() > 0f)
        {
            var item = other.GetComponent<ItemPickup>();
            var heavyItem = other.GetComponent<HeavyItem>();
            var door = other.GetComponent<Door>();
            var doorSwitch = other.GetComponent<SwitchController>();

            if (doorSwitch)
            {
                doorSwitch.ActivateSwitch();
                return;
            }

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
