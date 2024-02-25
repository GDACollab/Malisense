using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSelector : MonoBehaviour
{
    public bool isInteractable = false;
    public Collider2D other;
    
    private void OnTriggerStay2D(Collider2D collider) {
        other = collider;
        isInteractable = true;
        if(collider.GetComponent<NearFloorNote>()){
            collider.GetComponent<NearFloorNote>().isNear = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        isInteractable = false;
        if(collider.GetComponent<NearFloorNote>()){
            collider.GetComponent<NearFloorNote>().isNear = false;
        }
    }
}
