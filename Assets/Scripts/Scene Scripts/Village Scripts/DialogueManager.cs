using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

/// <summary>
/// A script that handles dialogue UI and Ink functionality (variables, choices, etc)
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private GameObject clericsSprite;
    [SerializeField] private GameObject HPSprite;
    [SerializeField] private GameObject CKSprite;
    [SerializeField] private GameObject StickSprite;

    [Header("Village Navigation")]
    [SerializeField] VillageNavigationManager navigationManager;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    [SerializeField] private int currentChoiceIndex = -1;
    private int defaultHeight = 115;

    [Header("Status")]
    [SerializeField] private bool isPlaying;
    

    // Global Teapot
    GlobalTeapot globalTeapot;

    private bool canStartDialogue = true;

    void Start()
    {
        // Get the Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();

        isPlaying = false;
        dialoguePanel.SetActive(false);

        globalTeapot.currentStory.ResetState();
        
        // Enable correct clergy sprite
        if (globalTeapot.highPriestWasIntroduced)
        {
            clericsSprite.SetActive(false);
            HPSprite.SetActive(true);
        }

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
            canStartDialogue = true;
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
        if (isPlaying && globalTeapot.currentStory.currentChoices.Count > 0)
        {

            // Move
            if (direction == "left")             // left
            {
                // Navigate up in the choices list
                currentChoiceIndex--;
                if (currentChoiceIndex < 0) currentChoiceIndex = globalTeapot.currentStory.currentChoices.Count - 1;
                // Optionally, call a function to update the UI here
            }
            else if (direction == "right")        // right
            {
                // Navigate down in the choices list
                currentChoiceIndex++;
                if (currentChoiceIndex >= globalTeapot.currentStory.currentChoices.Count) currentChoiceIndex = 0;
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
            if (globalTeapot.currentStory.currentChoices.Count > 0)
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
        // Prevent from restarting conversation at the end
        if (!canStartDialogue)
        {
            Debug.Log("Do not restart currently running story.");
            isPlaying = true;
            return; // Skip reinitializing the story
        }
        Debug.Log("Start running story.");

        // Name of owner of house stored in variable character and is one of ["Scholar", "Stick", "Crypt_Keeper", "Clergy", "Mayor", "???????????????"]
        // AUDIOMANAGER: CK OST / Shop OST

        globalTeapot.currentStory.ChoosePathString("START");

        // Mayor introduction variables
        if (character == "Mayor" && !globalTeapot.mayorWasIntroduced)
        {
            globalTeapot.currentStory.variablesState["isMayorIntro"] = true;
            globalTeapot.mayorWasIntroduced = true;
        } else
        {
            globalTeapot.currentStory.variablesState["isMayorIntro"] = false;
        }
        // Scholar introduction variables
        if (character == "Scholar" && !globalTeapot.scholarWasIntroduced)
        {
            globalTeapot.currentStory.variablesState["isScholarIntro"] = true;
            globalTeapot.scholarWasIntroduced = true;
        }
        else
        {
            globalTeapot.currentStory.variablesState["isScholarIntro"] = false;
        }
        // High Priest introduction variables
        if (character == "Clergy" && !globalTeapot.highPriestWasIntroduced && globalTeapot.currProgress ==  GlobalTeapot.TeaType.Dungeon_F2)
        {
            globalTeapot.highPriestWasIntroduced = true;
        }

        globalTeapot.currentStory.variablesState["hasMayorNote1"] = globalTeapot.hasMayorNote1;
        globalTeapot.currentStory.variablesState["hasMayorNote2"] = globalTeapot.hasMayorNote2;
        globalTeapot.currentStory.variablesState["hasFinalMayorNote"] = globalTeapot.hasFinalMayorNote;

        globalTeapot.currentStory.variablesState["toldCKAboutHighPriest"] = globalTeapot.toldCKAboutHighPriest;
        globalTeapot.currentStory.variablesState["highPriestWasIntroduced"] = globalTeapot.highPriestWasIntroduced;

        // Set defaults, will be modified afterward if needs to be true
        globalTeapot.currentStory.variablesState["isIntro"] = false;
        globalTeapot.currentStory.variablesState["isDeathF1"] = false;
        globalTeapot.currentStory.variablesState["isHub"] = false;
        globalTeapot.currentStory.variablesState["isDeathF2"] = false;
        globalTeapot.currentStory.variablesState["isEnd"] = false;
        globalTeapot.currentStory.variablesState["isIntroductionCutscene"] = isIntroductionCutscene;
        globalTeapot.currentStory.variablesState["hasDied"] = false;

        switch (globalTeapot.currProgress)
        {
            case GlobalTeapot.TeaType.Intro:
                globalTeapot.currentStory.variablesState["isIntro"] = true;
                Debug.Log("intro with " + character);
                break;
            case GlobalTeapot.TeaType.Dungeon_F1:
                globalTeapot.currentStory.variablesState["isDeathF1"] = true;
                Debug.Log("deathf1 with " + character);
                break;
            case GlobalTeapot.TeaType.Dungeon_F2:
                if (globalTeapot.hasDied)
                {
                    globalTeapot.currentStory.variablesState["isDeathF2"] = true;
                    Debug.Log("deathf2 with " + character);
                }
                else
                {
                    globalTeapot.currentStory.variablesState["isHub"] = true;
                    Debug.Log("hub with " + character);
                }
                break;
            case GlobalTeapot.TeaType.End:
                globalTeapot.currentStory.variablesState["isEnd"] = true;
                Debug.Log("end with " + character);
                break;
            default:
                Debug.Log("default in dialogue switch statement (this is bad)");
                globalTeapot.currentStory.variablesState["isIntro"] = true;
                break;
        }

        // VAR background = "First"
        globalTeapot.currentStory.variablesState["StickHappiness"] = globalTeapot.stickHappiness;
        if(globalTeapot.stickHappiness < -2)
        {
            StickSprite.SetActive(false);
        }

        globalTeapot.currentStory.variablesState["character"] = character; // "Crypt_Keeper" "Stick" "Mayor" "Clergy" "Scholar"
        canStartDialogue = false;
        isPlaying = true;

        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        // Hide CK if she dissapeared
        if (navigationManager.CurrentCharacter == "Crypt_Keeper" && globalTeapot.currProgress == GlobalTeapot.TeaType.Intro)
        {
            CKSprite.SetActive(false);
        }

        Debug.Log(gameObject);
        globalTeapot.stickHappiness = (int)globalTeapot.currentStory.variablesState["StickHappiness"];
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
        if (globalTeapot.currentStory.canContinue)
        {
            dialogueText.text = globalTeapot.currentStory.Continue();
            // Villager name stored in variable characterNameText.text is one of "????????????", "Crypt Keeper", "Clerics", "Smiling Cleric", Thinking Cleric, "Weeping Cleric", "Smiling", "Weeping", "Thinking", "High Priest", "Stick", "Scholar", "Mayor"]
            if ((string)globalTeapot.currentStory.variablesState["CharacterTitle"] == "High Priest")
            {
                clericsSprite.SetActive(false);
                HPSprite.SetActive(true);
            }
            characterNameText.text = (string)globalTeapot.currentStory.variablesState["CharacterTitle"];

            // Only bark when a character is talking:
            if (characterNameText.text != "")
            {
                // Name of owner of house stored in variable navigationManager.CurrentCharacter and is one of ["Scholar", "Stick", "Crypt_Keeper", "Clergy", "Mayor", "???????????????"]
                // AUDIOMANAGER: Villager barks
                int buildingIndex = navigationManager.selectedBuildingIndex;
                globalTeapot.audioManager.PlayVillageBark((VillageNavigationManager.Buildings)buildingIndex);
            }

            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Ink.Runtime.Choice> currentChoices = globalTeapot.currentStory.currentChoices;

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
        if (choiceIndex >= 0 && choiceIndex < globalTeapot.currentStory.currentChoices.Count)
        {
            // Tell the story to choose the selected choice
            globalTeapot.currentStory.ChooseChoiceIndex(choiceIndex);

            // Reset the choice index as we're moving to the next part of the story
            currentChoiceIndex = -1;

            // Continue the story after making a choice
            ContinueStory();
        }
    }


}
