using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class V_KeyboardInteractiontion3 : MonoBehaviour
{
    PlayerInput playerInput;
	InputAction moveAction;
    InputAction selectAction;
    
    [SerializeField] GameObject currentListSelected;
    [SerializeField] GameObject test;

    private int currentIndex;
    private V_SelectableItems3 DaSCRIPT;


    private static V_KeyboardInteractiontion3 _instance;

    public static V_KeyboardInteractiontion3 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<V_KeyboardInteractiontion3>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(V_KeyboardInteractiontion3).Name);
                    _instance = singletonObject.AddComponent<V_KeyboardInteractiontion3>();
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
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (moveAction.ReadValue<Vector2>().x < 0f && moveAction.triggered))
        {
            DaSCRIPT.moveInList(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || (moveAction.ReadValue<Vector2>().x > 0f && moveAction.triggered))
        {
            DaSCRIPT.moveInList(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || selectAction.triggered)
        {
            DaSCRIPT.selectObject();
        }
    }


    private void getGameObjectList()
    {
        //USE LATER
        DaSCRIPT = currentListSelected.GetComponent<V_SelectableItems3>();
    }

    public GameObject currentlySelectedObject() //For Other script to check if its listed
    {
        return currentListSelected;
    }

}
