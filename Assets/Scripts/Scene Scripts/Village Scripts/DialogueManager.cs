using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] public GameObject selectableItemsGameObject;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    [SerializeField] private int currentChoiceIndex = -1;

    // Start is called before the first frame update
    private Ink.Runtime.Story currentStory;

    private static DialogueManager instance;

    [SerializeField] private bool isPlaying;
    [SerializeField] V_SelectableItems3New selectableScript;

    private string currentInkFileName = "";


    void Start()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        selectableScript = selectableItemsGameObject.GetComponent<V_SelectableItems3New>();

        //Get all choices texts
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void Update()
    {
        int previousChoiceIndex = currentChoiceIndex;
        UpdateChoiceSelectionVisuals();

        if (!selectableScript.activateInk)
        {
            currentInkFileName = "";
            ClearDialogueMode();
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

    public void Move(InputAction.CallbackContext context)
    {
        if (isPlaying)
        {  // Need to check so that these input events are not triggered before currentStory.currentChoices.Count is a valid reference

            if (context.performed)
            {
                // Check if there are any choices to navigate
                if (currentStory.currentChoices.Count > 0)
                {
                    Vector2 inputVector = context.ReadValue<Vector2>();

                    if (inputVector.x < 0f)                                         // left
                    {
                        // Navigate up in the choices list
                        currentChoiceIndex--;
                        if (currentChoiceIndex < 0) currentChoiceIndex = currentStory.currentChoices.Count - 1;
                        // Optionally, call a function to update the UI here
                    }
                    else if (inputVector.x > 0f)                                    // right
                    {
                        // Navigate down in the choices list
                        currentChoiceIndex++;
                        if (currentChoiceIndex >= currentStory.currentChoices.Count) currentChoiceIndex = 0;
                        // Optionally, call a function to update the UI here
                    }
                }
            }
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (isPlaying)
        {  // Need to check so that these input events are not triggered before currentStory.currentChoices.Count is a valid reference
            if (context.performed)
            {
                // Check if there are any choices to navigate
                if (currentStory.currentChoices.Count > 0)
                {
                    MakeChoice(currentChoiceIndex);
                }
                else
                {
                    ContinueStory();
                }
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJson)
    {

        if (currentInkFileName == inkJson.name)
        {

            Debug.Log("RETURN");
            // The story is already loaded and can continue, so just activate the UI
            isPlaying = true;
            dialoguePanel.SetActive(true);
            // Optionally, update the UI here if needed (e.g., refresh choice buttons)
            return; // Skip reinitializing the story
        }
        Debug.Log(" Not RETURN " + currentInkFileName + " Names " + inkJson.name);
        // Load a new story or restart the current one
        currentStory = new Ink.Runtime.Story(inkJson.text);
        currentInkFileName = inkJson.name; // Update the current ink file name
        isPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        selectableScript.selectObject();
    }

    private void ClearDialogueMode()
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
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        currentChoiceIndex = 0;
    }

    private void UpdateChoiceSelectionVisuals()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            if (i == currentChoiceIndex)
            {
                // Highlight the current choice
                choices[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow; // Example of highlighting
            }
            else
            {
                // Revert other choices to their normal state
                choices[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white; // Example of normal state
            }
        }
    }

    private void MakeChoice(int choiceIndex)
    {
        // Check if the choiceIndex is valid for the current choices
        if (choiceIndex >= 0 && choiceIndex < currentStory.currentChoices.Count)
        {
            // Tell the story to choose the selected choice
            currentStory.ChooseChoiceIndex(choiceIndex);

            // Reset the choice index as we're moving to the next part of the story
            currentChoiceIndex = -1;

            // Continue the story after making a choice
            ContinueStory();
        }
    }


}
