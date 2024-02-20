using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DynamiteScript : MonoBehaviour
{
    private Camera camera;

    Vector2 playerPosition;
    Vector2 mousePosition;
    Vector2 distance;


    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = transform.position;
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("throw dynamite");
        }
    }


}
