using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class V_MouseInteraction : MonoBehaviour
{
    int UILayer;
    [SerializeField] GameObject selectedGameObject; //Checks if you click twice
    [SerializeField] GameObject checkHoveredObject;
    [SerializeField] GameObject lastHoveredObject;
        
    
    private static V_MouseInteraction _instance;

    public static V_MouseInteraction Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<V_MouseInteraction>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(V_MouseInteraction).Name);
                    _instance = singletonObject.AddComponent<V_MouseInteraction>();
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


    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        //will check what you are currently hovering
        checkHoveredObject = GetHoveredObject();

        if(checkHoveredObject != null) //checks if its a pickup object
        {
            lastHoveredObject = checkHoveredObject;
            if(Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                ClickedHoveredItem(checkHoveredObject);
            }
        }
        else
        {
        }

    }

    private GameObject GetHoveredObject()
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.layer == UILayer)
            {
                return result.gameObject;
            }
        }
        return null;
    }
    
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        if (Input.touchCount > 0)
        {
            eventData.position = Input.GetTouch(0).position;
        }
        else
        {
            eventData.position = Input.mousePosition;
        }
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }


    private void ClickedHoveredItem(GameObject obj)
    {
        if(obj.CompareTag("VillageHouseUI") == true)
        {
            interactObject(obj);
        }
    }

    // RECHANGE FOR OTHER
    private void interactObject(GameObject item)
    {
        selectedGameObject = item;
    }

    private IEnumerator WaitForAnimation(Animation animation, GameObject item)
    {
        while (animation.isPlaying)
        {
            yield return null;
        }
        // item.SetActive(false);
    }

    public GameObject getSelectedObject()
    {
        if(selectedGameObject != null)
        {
            return null;
        }
        return selectedGameObject;
    }

}
