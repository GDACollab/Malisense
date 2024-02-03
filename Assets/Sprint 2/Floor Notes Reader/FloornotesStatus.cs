using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloornotesStatus : MonoBehaviour
{
    public bool isSelectedFN = false;
    public Animator animatorFN;
    // Start is called before the first frame update
    public void SelectNote()
    {
        if (!isSelectedFN)
        {
            isSelectedFN = true;
            Debug.Log("Note is selected...");
            animatorFN.SetBool("isSelected", isSelectedFN);
        }
    }
}
