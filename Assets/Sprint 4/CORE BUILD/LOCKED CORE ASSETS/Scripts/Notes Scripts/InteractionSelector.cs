using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: If we want this to run faster we could exclude non-interactable 
//       objects from being added to the list to begin with, but I'm not
//       doing that rn -- Evan Lake

public class InteractionSelector : MonoBehaviour
{
    //public bool isInteractable = false;
    public GameObject other;
    public List<GameObject> objInteracts; // list of all interactable objects

    // compares items to see which one has higher priority 
    // returns (1) if a > b
    // returns (0) if a <= b
    // returns (-1) if one of them is not an interactable item
    public int compareItems(GameObject a, GameObject b) {
        int aPrio = 0;
        int bPrio = 0;
        if (a.GetComponent<Door>()) aPrio = 1;
        else if (a.GetComponent<FloorNote>()) aPrio = 2;
        else if (a.GetComponent<SwitchController>()) aPrio = 3;
        else if (a.GetComponent<HeavyItem>()) aPrio = 4;
        else if (a.GetComponent<ItemPickup>()) aPrio = 5;
        else aPrio = -1;

        if (b.GetComponent<Door>()) bPrio = 1;
        else if (b.GetComponent<FloorNote>()) bPrio = 2;
        else if (b.GetComponent<SwitchController>()) bPrio = 3;
        else if (b.GetComponent<HeavyItem>()) bPrio = 4;
        else if (b.GetComponent<ItemPickup>()) bPrio = 5;
        else bPrio = -1;
        if (aPrio == -1 || bPrio == -1) return -1;

        if (aPrio > bPrio) return 1;
        else if (aPrio < bPrio) return 0;
        else {
            if (Vector2.Distance(transform.position, a.transform.position) <= Vector2.Distance(transform.position, b.transform.position)) return 1;
            else return 0;
        }
    }

    public List<GameObject> getInteractables() {
        Collider2D[] touchList = new Collider2D[32];
        ContactFilter2D filter = new ContactFilter2D();
        int numTouch = GetComponent<CapsuleCollider2D>().OverlapCollider(filter.NoFilter(), touchList);

        // insertion sort each collider into a list of the objects in priority order
        foreach (var touch in touchList) {
            if (touch == null) continue;
            //Debug.Log("Touch: " + touch.name);
            if (objInteracts.Count == 0 && compareItems(touch.gameObject, touch.gameObject) != -1) { 
                //Debug.Log("\tAdded as first obj");
                objInteracts.Add(touch.gameObject);
                continue;
            }
            for (int i = 0; i < objInteracts.Count; i++) {
                //Debug.Log("\tLooking at index: " + i);
                //Debug.Log("\tinteract: " + objInteracts[i].name);
                int comparator = compareItems(touch.gameObject, objInteracts[i]);
                if (comparator == 1) {
                    objInteracts.Insert(i,touch.gameObject);
                    break;
                } else if (comparator == -1) {
                    break;
                }
            }
            
            
        }
        if (objInteracts.Count == 0) return null;
        else {

            return objInteracts;
        }
    }

    // returns a bool stating if there are interactable objects around the player or nor
    public bool isInteractable() {
        Collider2D[] touchList = new Collider2D[32];
        ContactFilter2D filter = new ContactFilter2D();
        GetComponent<CapsuleCollider2D>().OverlapCollider(filter.NoFilter(), touchList);

        // insertion sort each collider into a list of the objects in priority order
        foreach (var touch in touchList) {
            if (touch == null) continue;
            if (touch.GetComponent<Door>()) return true;
            else if (touch.GetComponent<FloorNote>()) return true;
            else if (touch.GetComponent<SwitchController>()) return true;
            else if (touch.GetComponent<HeavyItem>()) return true;
            else if (touch.GetComponent<ItemPickup>()) return true;
        }
        return false;
    }

    public void removeInteracts() {
        objInteracts.Clear();
    }

}
