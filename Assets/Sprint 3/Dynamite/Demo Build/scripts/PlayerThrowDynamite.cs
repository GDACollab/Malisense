using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerThrowDynamite : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePosition;
    public GameObject thrownDynamitePrefab;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0)) 
        {
            Instantiate(thrownDynamitePrefab, player.position, Quaternion.identity);
        }
    }
}
