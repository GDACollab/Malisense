using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NearFloorNote : MonoBehaviour
{
    public GameObject Note;
    public GameObject Player;
    public bool isNear = false;

    [Header("Note Sprites")]
    public bool disappear = true;
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
        if ((Note.transform.position - Player.transform.position).sqrMagnitude < 6.0f)
        { 
            isNear = true;
            spriteRenderer.sprite = selected; // change sprite to selectable
        }
        else { // change sprite to normal
            isNear = false;
            spriteRenderer.sprite = notSelected;
        }
        // if near note and key pressed down, popup the window
        if (isNear && !NoteActive && selectAction.triggered)
        {
            FloorNotePopup.showFNR(noteTitle, noteBody);
            NoteActive = true; 
        }
        else if(NoteActive && selectAction.triggered){
            FloorNotePopup.hideFNR();
            NoteActive = false; 
            // disappear when closed
            if (disappear) {
                Object.Destroy(Note);
            }
        }
    }

}