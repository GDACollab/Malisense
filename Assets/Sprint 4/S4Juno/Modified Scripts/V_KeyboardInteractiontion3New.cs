using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class V_KeyboardInteractiontion3New : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction selectAction;

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
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions.FindAction("8 Directions Movement");
            selectAction = playerInput.actions.FindAction("Interact");
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        simpleMovement();
        getGameObjectList();
    }
    private void simpleMovement()
    {

        if (moveAction.ReadValue<Vector2>().x < 0f && moveAction.triggered)
        {
            DaSCRIPT.moveInList(-1);
        }
        else if (moveAction.ReadValue<Vector2>().x > 0f && moveAction.triggered)
        {
            DaSCRIPT.moveInList(1);
        }
        else if (selectAction.triggered)
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
