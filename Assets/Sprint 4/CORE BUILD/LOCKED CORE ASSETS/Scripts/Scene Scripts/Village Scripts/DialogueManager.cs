using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] public GameObject selectableItemsGameObject;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    // Start is called before the first frame update
    private Ink.Runtime.Story currentStory;

    private static DialogueManager instance;

    [SerializeField] private bool isPlaying;
    [SerializeField] V_SelectableItems3New selectableScript;

    void Start()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        selectableScript = selectableItemsGameObject.GetComponent<V_SelectableItems3New>();

        //Get all choices texts
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach( GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }


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
            ExitDialogueMode();
        }

        if (isPlaying)
        {
            return;
        }
        
        if (selectableScript != null && selectableScript.activateInk)
        {
                TextAsset currentInk = selectableScript.CurrentInkTextAsset;
                // If currentlySelected is true, show the dialogue panel - List of InkJson TextAssets in V_SelectableItens, variable CurInk
                if (currentInk != null)
                {
                EnterDialogueMode(currentInk);
                }
        }
  
    }


    private string currentInkFileName = "";
    public void EnterDialogueMode(TextAsset inkJson)
    {
        if (currentStory == null || currentInkFileName != inkJson.name)
        {
            currentStory = new Ink.Runtime.Story(inkJson.text);
            currentInkFileName = inkJson.name; // Update the current ink file name
        }

        isPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentInkFileName = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Ink.Runtime.Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices than UI can support");
        }

        //Enable Options
        int index = 0;
        foreach (Ink.Runtime.Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        //Hide all other options
        for (int i = index;  i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

    }
}
