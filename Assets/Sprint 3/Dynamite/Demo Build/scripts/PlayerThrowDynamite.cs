using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerThrowDynamite : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject thrownDynamitePrefab;
    public Transform player;

    public bool canThrow;
    public float cooldown;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canThrow) 
        {
            timer += Time.deltaTime;
            if (timer > cooldown)
            {
                canThrow = true;
                timer = 0;
            }
        }



        if (Input.GetMouseButton(0) && canThrow) 
        {
            canThrow = false;
            Instantiate(thrownDynamitePrefab, player.position, Quaternion.identity);
        }
    }
}
