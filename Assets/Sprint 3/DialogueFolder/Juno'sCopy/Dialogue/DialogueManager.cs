using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] public GameObject selectableItemsGameObject;

    // Start is called before the first frame update

    private Ink.Runtime.Story currentStory;

    private static DialogueManager instance;

    [SerializeField] private bool isPlaying;
    V_SelectableItems3 selectableScript;

    void Start()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        selectableScript = selectableItemsGameObject.GetComponent<V_SelectableItems3>();
    }

    public void Update()
    {
        //If Currently Selected and Input Space bar
        if (isPlaying && Input.GetKeyDown(KeyCode.L))
        {
            ContinueStory();
        }


        if (!selectableScript.activateInk)
        {
            Debug.Log("Leave");
            ExitDialogueMode();
        }

        if (isPlaying)
        {
            return;
        }
        
        Debug.Log(selectableScript.activateInk);
        if (selectableScript != null && selectableScript.activateInk)
        {
                Debug.Log("Enter");
                TextAsset currentInk = selectableScript.CurrentInkTextAsset;
                // If currentlySelected is true, show the dialogue panel - List of InkJson TextAssets in V_SelectableItens, variable CurInk
                if (currentInk != null)
                {
                EnterDialogueMode(currentInk);
                }
        }
        

        
        

        
    }



    public void EnterDialogueMode(TextAsset inkJson)
    {
        currentStory = new Ink.Runtime.Story(inkJson.text);
        isPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();

        

    }

    private void ExitDialogueMode()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
