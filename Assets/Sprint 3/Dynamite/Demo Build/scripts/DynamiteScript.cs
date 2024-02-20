using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class DynamiteScript : MonoBehaviour
{
    private Camera camera;

    [SerializeField] GameObject DynamitePrefab;
    
    
    public GameObject playerPosition;
    public GameObject thrown_Dynamite;
    Vector2 mousePosition;


    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            thrown_Dynamite = Instantiate(DynamitePrefab);
            thrown_Dynamite.transform.position = playerPosition.transform.position;
            Debug.Log(mousePosition);
        }
    }


}
