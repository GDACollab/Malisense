using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// A script that handles dialogue UI and Ink functionality (variables, choices, etc)
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterNameText;

    [Header("Village Navigation")]
    [SerializeField] VillageNavigationManager navigationManager;

    [Header("Ink File")]
    [Tooltip("The master ink file.")]
    [SerializeField] private TextAsset masterInk;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    [SerializeField] private int currentChoiceIndex = -1;
    private int defaultHeight = 115;

    // Start is called before the first frame update
    private Ink.Runtime.Story currentStory;

    [Header("Status")]
    [SerializeField] private bool isPlaying;
    

    // Global Teapot
    GlobalTeapot globalTeapot;

    private string currentInkFileName = "";

    void Start()
    {
        // Get the Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();

        isPlaying = false;
        dialoguePanel.SetActive(false);

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
        UpdateChoiceSelectionVisuals();

        if (!navigationManager.activateInk)
        {
            currentInkFileName = "";
            ClearDialogueMode();
        }

        if (isPlaying)
        {
            return;
        }

        if (navigationManager != null && navigationManager.activateInk)
        {
            string currentChar = navigationManager.CurrentCharacter;
            // If currentlySelected is true, show the dialogue panel - List of InkJson TextAssets in V_SelectableItens, variable CurInk
            if (currentChar != null)
            {
                // If this is the introduction cutscene, pass it with an extra parameter
                if (navigationManager.selectedBuildingIndex == 5)
                {
                    EnterDialogueMode(isIntroductionCutscene: true);
                }
                else
                {
                    EnterDialogueMode(currentChar);
                }

            }
        }
    }

    public void moveChoiceSelection(string direction)
    {
        // Need to check isPlaying so that these input events are not triggered before currentStory.currentChoices.Count is a valid reference
        // Check that there are dialogue options to choose from otherwise there's no option to move
        if (isPlaying && currentStory.currentChoices.Count > 0)
        {

            // Move
            if (direction == "left")             // left
            {
                // Navigate up in the choices list
                currentChoiceIndex--;
                if (currentChoiceIndex < 0) currentChoiceIndex = currentStory.currentChoices.Count - 1;
                // Optionally, call a function to update the UI here
            }
            else if (direction == "right")        // right
            {
                // Navigate down in the choices list
                currentChoiceIndex++;
                if (currentChoiceIndex >= currentStory.currentChoices.Count) currentChoiceIndex = 0;
                // Optionally, call a function to update the UI here
            }
        }
    }

    public void selectDialogue()
    {
        // Need to check so that these input events are not triggered before currentStory.currentChoices.Count is a valid reference
        if (isPlaying)
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

    public void EnterDialogueMode(string character = "???????????????", bool isIntroductionCutscene = false)
    {
        TextAsset inkJson = masterInk;

        // Prevent from restarting conversation at the end
        if (currentInkFileName == inkJson.name)
        {
            Debug.Log("Do not restart currently running story.");
            isPlaying = true;
            return; // Skip reinitializing the story
        }
        Debug.Log("Start running story.");

        currentStory = new Ink.Runtime.Story(inkJson.text);

        currentStory.variablesState["isMayorIntro"] = globalTeapot.currProgress == GlobalTeapot.TeaType.Intro; // MAYOR INTRO DELAY NOT IMPLEMENTED, CURRENTLY JUST HAPPENS DURING THE NORMAL INTRO
        currentStory.variablesState["hasMayorNote1"] = globalTeapot.hasMayorNote1;
        currentStory.variablesState["hasMayorNote2"] = globalTeapot.hasMayorNote2;
        currentStory.variablesState["hasFinalMayorNote"] = globalTeapot.hasFinalMayorNote;

        // Set defaults, will be modified afterward if needs to be true
        currentStory.variablesState["isIntro"] = false;
        currentStory.variablesState["isDeathF1"] = false;
        currentStory.variablesState["isHub"] = false;
        currentStory.variablesState["isDeathF2"] = false;
        currentStory.variablesState["isEnd"] = false;
        currentStory.variablesState["isIntroductionCutscene"] = isIntroductionCutscene;
        currentStory.variablesState["hasDied"] = false;

        switch (globalTeapot.currProgress)
        {
            case GlobalTeapot.TeaType.Intro:
                currentStory.variablesState["isIntro"] = true;
                Debug.Log("intro with " + character);
                break;
            case GlobalTeapot.TeaType.Dungeon_F1:
                currentStory.variablesState["isDeathF1"] = true;
                Debug.Log("deathf1 with " + character);
                break;
            case GlobalTeapot.TeaType.Dungeon_F2:
                if (globalTeapot.hasDied)
                {
                    currentStory.variablesState["isDeathF2"] = true;
                    Debug.Log("deathf2 with " + character);
                }
                else
                {
                    currentStory.variablesState["isHub"] = true;
                    Debug.Log("hub with " + character);
                }
                break;
            case GlobalTeapot.TeaType.End:
                currentStory.variablesState["isEnd"] = true;
                Debug.Log("end with " + character);
                break;
            default:
                Debug.Log("default in dialogue switch statement (this is bad)");
                currentStory.variablesState["isIntro"] = true;
                break;
        }

        // VAR background = "First"
        currentStory.variablesState["StickHappiness"] = globalTeapot.stickHappiness;

        currentStory.variablesState["character"] = character; // "Crypt_Keeper" "Stick" "Mayor" "Clergy" "Scholar"
        currentInkFileName = inkJson.name; // Update the current ink file name         
        isPlaying = true;

        dialoguePanel.SetActive(true);
        if (character == "Crypt_Keeper")
        {
            characterNameText.text = "Crypt Keeper";
        } else
        {
            characterNameText.text = character;
        }
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        Debug.Log(gameObject);
        globalTeapot.stickHappiness = (int)currentStory.variablesState["StickHappiness"];
        isPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        characterNameText.text = "";
        navigationManager.selectBuilding();
    }

    private void ClearDialogueMode()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        characterNameText.text = "";
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
                choices[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black; // Example of highlighting
                choices[i].GetComponentInChildren<TextMeshProUGUI>().overflowMode = TextOverflowModes.ScrollRect; // set overflow to allow text to expand
                choices[i].GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize; //set box to exanded size
                // choices[i].GetComponentInChildren<TextMeshProUGUI>().text

            }
            else
            {
                // Revert other choices to their normal state
                choices[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.grey; // Example of normal state
                choices[i].GetComponentInChildren<TextMeshProUGUI>().overflowMode = TextOverflowModes.Ellipsis; //reset overflow to Ellipsis
                choices[i].GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained; //don't fit to text
                choices[i].GetComponent<RectTransform>().sizeDelta = new Vector2(choices[i].GetComponent<RectTransform>().rect.width, defaultHeight);   //return box to normal size  
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
