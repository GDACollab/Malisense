using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MODIFIED FROM https://stuartspixelgames.com/2018/06/24/simple-2d-top-down-movement-unity-c/
public class MovementScript : MonoBehaviour 
{   
    Rigidbody2D rb;
    

    float horizontal;
    float vertical;

    public float runSpeed = 10;
    public Canvas floorNote;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
    // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        floorNote.gameObject.SetActive(true);
        
    }
}
