using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.Rendering;
using UnityEngine;


public class DynamiteMovement : MonoBehaviour
{
    private Vector3 mousePosition;
    private Camera mainCamera;

    public int speed;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    private void Update()
    {
        

        Vector2 position = Vector2.MoveTowards(transform.position, mousePosition, Time.deltaTime * speed);

        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, (mousePosition - transform.position).normalized);

        transform.SetPositionAndRotation(position, rotation);
    }
}
