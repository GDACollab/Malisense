using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorNote : MonoBehaviour
{
    public GameObject Note;
    public GameObject Player;
    public bool isNear = false;

    [Header("Note Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite notSelected;
    public Sprite selected;

    public FNRManager FloorNotePopup;
    [TextArea(15,20)]
    public string noteTitle = "Title";
    [TextArea(15,20)]
    public string noteBody = "Body";
    
    public KeyCode InteractButton;
    bool NoteActive = false; // not sure if this is needed
    
    private PlayerInput playerInput;
    private InputAction selectAction;

    void Start()
    {
        Note = this.gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set input system variables
        playerInput = Player.GetComponent<PlayerInput>();
        selectAction = playerInput.actions.FindAction("Interact");
        
    }

    private void Update()
    {
        if (isNear)
        { 
            spriteRenderer.sprite = selected; // change sprite to selectable
        }
        else { // change sprite to normal
            spriteRenderer.sprite = notSelected;
        }
    }
    
        // if near note and key pressed down, popup the window
    public void ActivateNote(){
        FloorNotePopup.showFNR(noteTitle, noteBody);
    }
    public void DeactivateNote(){
        FloorNotePopup.hideFNR();
    }
}