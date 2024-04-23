using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class V_KeyboardInteractiontion3New : MonoBehaviour
{
    [SerializeField] GameObject currentListSelected;

    private int currentIndex;
    private V_SelectableItems3New DaSCRIPT;


    private static V_KeyboardInteractiontion3New _instance;

    public static V_KeyboardInteractiontion3New Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<V_KeyboardInteractiontion3New>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(V_KeyboardInteractiontion3New).Name);
                    _instance = singletonObject.AddComponent<V_KeyboardInteractiontion3New>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        getGameObjectList();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();

            if (inputVector.x < 0f)             // left
            {
                DaSCRIPT.moveInList(-1);
            }
            else if (inputVector.x > 0f)        // right
            {
                DaSCRIPT.moveInList(1);
            }
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!DaSCRIPT.hasEntered)
            {
                DaSCRIPT.selectObject();
            }
        }
    }

    private void getGameObjectList()
    {
        //USE LATER
        DaSCRIPT = currentListSelected.GetComponent<V_SelectableItems3New>();
    }

    public GameObject currentlySelectedObject() //For Other script to check if its listed
    {
        return currentListSelected;
    }

}
