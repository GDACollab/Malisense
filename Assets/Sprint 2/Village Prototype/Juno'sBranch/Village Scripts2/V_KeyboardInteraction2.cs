using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class V_KeyboardInteractiontion2 : MonoBehaviour
{
    [SerializeField] GameObject currentListSelected;
    [SerializeField] GameObject test;

    private int currentIndex;
    private V_SelectableItems2 DaSCRIPT;


    private static V_KeyboardInteractiontion2 _instance;

    public static V_KeyboardInteractiontion2 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<V_KeyboardInteractiontion2>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(V_KeyboardInteractiontion2).Name);
                    _instance = singletonObject.AddComponent<V_KeyboardInteractiontion2>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void Update()
    {
        simpleMovement();
        getGameObjectList();
    }
    private void simpleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DaSCRIPT.moveInList(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DaSCRIPT.moveInList(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DaSCRIPT.selectObject();
        }
    }


    private void getGameObjectList()
    {
        //USE LATER
        DaSCRIPT = currentListSelected.GetComponent<V_SelectableItems2>();
    }

    public GameObject currentlySelectedObject() //For Other script to check if its listed
    {
        return currentListSelected;
    }

}
