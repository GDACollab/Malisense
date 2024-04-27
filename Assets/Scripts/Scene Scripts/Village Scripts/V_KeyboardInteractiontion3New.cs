using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class V_KeyboardInteractiontion3New : MonoBehaviour
{
    [Header("Input")]
	[Tooltip("The initial delay (in seconds) between an initial move action and a repeated move action.")]
	[SerializeField] float moveRepeatDelay = 0.5f;
	[Tooltip("The speed (in seconds) that the move action repeats itself once repeating (max 1 per frame).")]
	[SerializeField] float moveRepeatRate = 0.1f;
    bool firstInput = true;
    float moveTimer = 0f;
    Controls controls;


    [Header("Lists")]
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

        // Input
        controls = new Controls();
        controls.UI.Enable();
        controls.UI.Move.performed += Move;
        controls.UI.Select.performed += Select;
    }


    private void Update()
    {
        getGameObjectList();
		HeldDownMovementInput();
    }

    void HeldDownMovementInput()
    {
		// Decrement moveRepeatCooldown
		if (moveTimer > 0f)
        {
			moveTimer -= Time.deltaTime;

            if (moveTimer < 0f)
            {
				moveTimer = 0f;
            }
        }

        // Check if we can move
        if (moveTimer <= 0f)
        {
			Vector2 inputVector = controls.UI.Move.ReadValue<Vector2>();

            // Check for horizontal input
			if (inputVector.x != 0f)
			{
				// Put move on moveRepeatRate cooldown
				firstInput = false;
				moveTimer = moveRepeatRate;

				// Move
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
	}

    void Move(InputAction.CallbackContext context)
    {
		if (context.performed)
		{
			Vector2 inputVector = context.ReadValue<Vector2>();

			// Check for horizontal input
			if (inputVector.x != 0f)
            {
				// Move
				if (inputVector.x < 0f)             // left
				{
					DaSCRIPT.moveInList(-1);
				}
				else if (inputVector.x > 0f)        // right
				{
					DaSCRIPT.moveInList(1);
				}

				// Put move back on moveRepeatDelay cooldown
                firstInput = true;
				moveTimer = moveRepeatDelay;
			}
		}
	}

	void Select(InputAction.CallbackContext context)
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
