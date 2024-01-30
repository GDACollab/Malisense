using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    private float moveSpeed = 5;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            this.transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            this.transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            this.transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            this.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            
    }
}
