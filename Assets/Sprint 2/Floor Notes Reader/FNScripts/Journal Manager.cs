using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
    public Canvas JournalUI;

    private bool JournalActive = false;
    
    public void Update()
    {
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
