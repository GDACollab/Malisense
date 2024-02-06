using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//from jim Assets/Sprint 2/Floor Notes Reader/Scripts Floor Note Reader/Journal Manager.cs

public class NewBehaviourScript : MonoBehaviour
{

    public Canvas JournalUI;

    private bool JournalActive = false;

    // Update is called once per frame
    void Update(){

        if (Input.GetKeyDown(KeyCode.J) && !JournalActive)
        {
            JournalUI.gameObject.SetActive(true);
            JournalActive = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.J) && JournalActive)
        {
            JournalUI.gameObject.SetActive(false);  
            JournalActive = false;
        }
    }

}

