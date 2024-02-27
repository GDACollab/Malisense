using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sprint_3.CORE_BUILD.LOCKED_CORE_ASSETS.Scripts.Player_Scripts.Item_Logic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoshPlayer : MonoBehaviour
{
    public PlayerInput input;
    public InputAction interactAction;
    public InputAction setDownAction;
    public Rigidbody2D rb;
    public GameObject triangle;

    [SerializeField]
    public GameObject invObject;

    [HideInInspector]
    public InventoryBase newInventory;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(invObject);
        newInventory = invObject.GetComponent<InventoryBase>();
        input = GetComponent<PlayerInput>();
        triangle = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        interactAction = input.actions.FindAction("Interact");
        setDownAction = input.actions.FindAction("SetDown");
    }

    // Update is called once per frame
    void Update()
    {
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

        if (interactAction.triggered)
        {
            Vector2 pos = triangle.transform.position;
            Collider2D[] candidates = Physics2D.OverlapCircleAll(pos, 2f);

            foreach (Collider2D obj in candidates
                .OrderBy(collider => Vector2.Distance(collider.ClosestPoint(pos), pos)))
            {
                var item = obj.GetComponent<ItemPickup>();
                var heavyItem = obj.GetComponent<HeavyItem>();
                var door = obj.GetComponent<Door>();
                var doorSwitch = obj.GetComponent<SwitchController>();

                if (doorSwitch)
                {
                    doorSwitch.ActivateSwitch();
                    break;
                }
                else if (door)
                {
                    if (newInventory.carriedObject)
                    {
                        Destroy(newInventory.carriedObject.gameObject);
                        newInventory.carriedObject = null;
                        Destroy(door.gameObject);
                        break;
                    }
                }
                else if (heavyItem)
                {
                    if (!newInventory.carriedObject)
                    {
                        newInventory.carriedObject = heavyItem;
                        break;
                    }
                }
                else if (item)
                {
                    bool success = newInventory.AddItem(item.item, 1);
                    if (success)
                    {
                        Destroy(obj.gameObject);
                        break;
                    }
                }
            }
        }
    }
}