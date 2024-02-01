using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_SelectableItems : MonoBehaviour
{
    [SerializeField] private List<GameObject> SELECTABLES = new List<GameObject>();
    [SerializeField] private int listIndex;
    [SerializeField] private bool currentlySelected = false;
    GameObject selectedGameObject;
    [SerializeField] private GameObject theFunnyArrow;

    private GameObject thisObject;

    private void Start()
    {
        thisObject = gameObject;
    }

    private void Update()
    {
        ifSelected();
        if(!currentlySelected)
        {   
            return;
        }
    }

    public void moveInList(int move)
    {
        if(!currentlySelected)
        {   
            return;
        }
        listIndex += move;
        if (listIndex < 0)
        {
            listIndex = SELECTABLES.Count - 1;
        }
        else if (listIndex >= SELECTABLES.Count)
        {
            listIndex = 0;
        }
        selectedGameObject = SELECTABLES[listIndex];
        itemSelected();
    }

    public void selectObject()
    {
        //selectedGameObject
    }

    private void itemSelected()
    {
        float targetXPosition = selectedGameObject.transform.position.x;

        Vector3 currentPosition = theFunnyArrow.transform.position;
        Vector3 newPosition = new Vector3(targetXPosition, currentPosition.y, currentPosition.z);
        theFunnyArrow.transform.position = newPosition;
    }

    private void ifSelected()
    {
        if(V_KeyboardInteractiontion.Instance.currentlySelectedObject() == thisObject)
        {
            currentlySelected = true;
        }
        else
        {
            currentlySelected = false;
        }
    }

}
