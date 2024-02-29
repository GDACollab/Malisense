using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JournalManager : MonoBehaviour
{
    public Canvas JournalUI;

    private bool JournalActive = false;
    
    PlayerInput playerInput;
	InputAction journalAction;
    
    private void Start() {
        playerInput = GetComponent<PlayerInput>();
		journalAction = playerInput.actions.FindAction("Journal Action");
    }
    
    public void Update()
    {
        if (journalAction.triggered && !JournalActive)
        {
            JournalUI.gameObject.SetActive(true);
            JournalActive = true;
        }
        else if (journalAction.triggered && JournalActive)
        {
            JournalUI.gameObject.SetActive(false);  
            JournalActive = false;
        }
    }
    
}
