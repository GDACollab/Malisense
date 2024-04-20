using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: If we want this to run faster we could exclude non-interactable 
//       objects from being added to the list to begin with, but I'm not
//       doing that rn -- Evan Lake

public class InteractionSelector : MonoBehaviour
{
    public bool isInteractable = false;
    public GameObject other;
    public List<GameObject> objInteracts; // list of all interactable objects

    // compares items to see which one has higher priority (returns true if a > b)
    public bool compareItems(GameObject a, GameObject b) {
        int aPrio = 0;
        int bPrio = 0;
        if (a.GetComponent<Door>()) aPrio = 1;
        else if (a.GetComponent<FloorNote>()) aPrio = 2;
        else if (a.GetComponent<SwitchController>()) aPrio = 3;
        else if (a.GetComponent<HeavyItem>()) aPrio = 4;
        else if (a.GetComponent<ItemPickup>()) aPrio = 5;

        if (b.GetComponent<Door>()) bPrio = 1;
        else if (b.GetComponent<FloorNote>()) bPrio = 2;
        else if (b.GetComponent<SwitchController>()) bPrio = 3;
        else if (b.GetComponent<HeavyItem>()) bPrio = 4;
        else if (b.GetComponent<ItemPickup>()) bPrio = 5;

        if (aPrio > bPrio) return true;
        else if (aPrio < bPrio) return false;
        else {
            if (Vector2.Distance(transform.position, a.transform.position) <= Vector2.Distance(transform.position, b.transform.position)) return true;
            else return false;
        }
    }

    public GameObject getInteractable() {
        if (objInteracts.Count == 0) return null;
        else return objInteracts[0];
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        //check if this is the first item in the list
        Debug.Log("can interact with item: " + collider.gameObject.name);
        if (objInteracts.Count == 0) objInteracts.Add(collider.gameObject);
        else {
            // place into list by order of interaction priority
            for (int it = 0; it < objInteracts.Count; it++) {
                if (compareItems(collider.gameObject, objInteracts[it])) {
                    objInteracts.Insert(it,collider.gameObject);
                    break;
                }
            }
        }
        
        
        other = getInteractable();
        // isInteractable = true;
        // if(collider.GetComponent<FloorNote>()){
        //     collider.GetComponent<FloorNote>().isNear = true;
        // }
        Debug.Log(objInteracts);
        if (objInteracts.Count > 0) isInteractable = true;
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("got away from item: " + collider.gameObject.name);
        foreach (GameObject g in objInteracts) {
            if (g == collider.gameObject) {
                objInteracts.Remove(g);
                break;
            } 
            
        }
        if (objInteracts.Count == 0) isInteractable = false;
        if(collider.GetComponent<FloorNote>()){
            collider.GetComponent<FloorNote>().isNear = false;
        }
    }

}
