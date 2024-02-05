using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearFloorNote : MonoBehaviour
{
    public GameObject Note;
    public GameObject Player;
    public bool isNear = false;

    [Header("Note Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite notSelected;
    public Sprite selected;

    public GameObject FloorNotePopup;
    public KeyCode InteractButton;
    bool NoteActive = false; // not sure if this is needed


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
        if (isNear && Input.GetKeyDown(InteractButton) && !NoteActive)
        {
            FloorNotePopup.SetActive(true);
            NoteActive = true; 
        }
    }

}