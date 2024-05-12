using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorNote : MonoBehaviour
{
    public string noteID = "";
    public bool isNear = false;

    [Header("Note Sprites")]
    public bool disappear = true;
    private SpriteRenderer spriteRenderer;
    public Sprite notSelected;
    public Sprite selected;

    public string noteTitle = "Title";
    [TextArea(5,20)]
    public string noteBody = "Body";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get sprite renderer
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
        // dungeonManager.ActivateNote(this);
        if (Time.timeScale != 0f) 
        {
            Time.timeScale = 0f;
        }
    }
    public void DeactivateNote(){
        // dungeonManager.DectivateNote();
        if (disappear)
        {
            Object.Destroy(this.gameObject);
        }
        if (Time.timeScale == 0f) 
        {
            Time.timeScale = 1f;
        }
    }
}