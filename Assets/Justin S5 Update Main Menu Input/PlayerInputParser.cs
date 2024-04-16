using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputParser : MonoBehaviour
{
    [SerializeField] GameObject[] mainMenuInteractables = new GameObject[4];

    GameObject selectedInteractable = null;

    [SerializeField] PlayerInput playerInput;
    InputAction moveAction;
    InputAction selectAction;

    void Awake()
    {
        // set actions
        moveAction = playerInput.actions.FindAction("Move");            // from UI action map
        selectAction = playerInput.actions.FindAction("Select");        // from UI action map
    }

    void Update()
    {
        if (moveAction.ReadValue<Vector2>().y > 0f && moveAction.triggered)
        {
            // move up
        }
        else if (moveAction.ReadValue<Vector2>().y < 0f && moveAction.triggered)
        {
            // move down
        }
        else if (moveAction.ReadValue<Vector2>().x > 0f && moveAction.triggered)
        {
            // if slider (sliders are only present in the options screen)
            // move slider right
        }
        else if (moveAction.ReadValue<Vector2>().x < 0f && moveAction.triggered)
        {
            // if slider (sliders are only present in the options screen)
            // move slider left
        }
        else if (selectAction.triggered)
        {
            // if button
            // press button
        }
    }
}