using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] public GameObject selectableItemsGameObject;

    [Header("Ink File")]
    [Tooltip("The master ink file.")]
    [SerializeField] private TextAsset masterInk;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    [SerializeField] private int currentChoiceIndex = -1;
    private int defaultHeight = 115;

	[Header("Input")]
	[Tooltip("The initial delay (in seconds) between an initial move action and a repeated move action.")]
	[SerializeField] float moveRepeatDelay = 0.5f;
	[Tooltip("The speed (in seconds) that the move action repeats itself once repeating (max 1 per frame).")]
	[SerializeField] float moveRepeatRate = 0.1f;
	bool firstInput = true;
	float moveTimer = 0f;
	Controls controls;

    // Start is called before the first frame update
    private Ink.Runtime.Story currentStory;

    private static DialogueManager instance;

    [Header("Other")]
    [SerializeField] private bool isPlaying;
    [SerializeField] V_SelectableItems3New selectableScript;

    private string currentInkFileName = "";


    void Awake()
    {
		// Input
		controls = new Controls();
		controls.UI.Enable();
		controls.UI.Select.performed += Select;
	}

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
		MovementInput();

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
            string currentChar = selectableScript.CurrentCharacter;
            // If currentlySelected is true, show the dialogue panel - List of InkJson TextAssets in V_SelectableItens, variable CurInk
            if (currentChar != null)
            {
                EnterDialogueMode(currentChar);
            }
        }
    }

    void MovementInput()
    {
		// Need to check isPlaying so that these input events are not triggered before currentStory.currentChoices.Count is a valid reference
		// Check that there are dialogue options to choose from otherwise there's no option to move
		if (isPlaying && currentStory.currentChoices.Count > 0)
        {
			// Read movement input
			Vector2 inputVector = controls.UI.Move.ReadValue<Vector2>();

			// There's horizontal movement input
			if (inputVector.x != 0f)
            {

                // Moving is on cooldown
                if (moveTimer > 0f)
                {
                    moveTimer -= Time.deltaTime;

                    if (moveTimer < 0f)
                    {
                        moveTimer = 0f;
                    }
                }

                // Can Move
                if (moveTimer <= 0f)
                {
					// Check if this is the first movement input after there was just no movement
					if (firstInput)
					{
						// Put move on moveRepeatDelay cooldown
						firstInput = false;
						moveTimer = moveRepeatDelay;
					}
					else
					{
						// Put move on moveRepeatRate cooldown
						moveTimer = moveRepeatRate;
					}

					// Move
					if (inputVector.x < 0f)             // left
					{
						// Navigate up in the choices list
						currentChoiceIndex--;
						if (currentChoiceIndex < 0) currentChoiceIndex = currentStory.currentChoices.Count - 1;
						// Optionally, call a function to update the UI here
					}
					else if (inputVector.x > 0f)        // right
					{
						// Navigate down in the choices list
						currentChoiceIndex++;
						if (currentChoiceIndex >= currentStory.currentChoices.Count) currentChoiceIndex = 0;
						// Optionally, call a function to update the UI here
					}
                }

            }

            // There's no horizontal movement input
            else
            {
                // Reset movement so that the next movement input is instant
                firstInput = true;
                moveTimer = 0f;
            }
        }
    }

	void Select(InputAction.CallbackContext context)
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

    public void EnterDialogueMode(string character)
    {
        TextAsset inkJson = masterInk;

        // Load a new story or restart the current one
        currentStory = new Ink.Runtime.Story(inkJson.text);

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

        // update ink variables
        // VAR isIntro = false
        // VAR isDeathF1 = false
        // VAR isHub = false
        // VAR isDeathF2 = false
        // VAR isEnd = false
        // VAR hasDied = false

        // VAR isMayorIntro = true
        // VAR hasMayorNote1 = false
        // VAR hasMayorNote2 = false
        // VAR hasFinalMayorNote = false

        // VAR background = "First"
        // VAR StickHappiness = 0

        //        VAR character = "Crypt_Keeper"

        //{
        //            -character == "Crypt_Keeper": ->Crypt_Keeper // Go to CK 
        //            - character == "Stick": -> Stick // Go to Stick
        //            - character == "Mayor": -> Mayor // Go to Mayor
        //            - character == "Clergy": -> Clergy // Go to Clergy
        //            - character == "Scholar": -> Scholar // Go to Scholar 
        //            - isEnd: // Go to end
        //         - else: Error
        //         }


        currentStory.variablesState["isIntro"] = "false";
        currentStory.variablesState["isDeathF1"] = "false";
        currentStory.variablesState["isHub"] = "false";
        currentStory.variablesState["isDeathF2"] = "true";
        currentStory.variablesState["isEnd"] = "false";
        currentStory.variablesState["hasDied"] = "false";

        currentStory.variablesState["isMayorIntro"] = "false";
        currentStory.variablesState["hasMayorNote1"] = "false";
        currentStory.variablesState["hasMayorNote2"] = "false";
        currentStory.variablesState["hasFinalMayorNote"] = "false";

        currentStory.variablesState["character"] = character; // "Crypt_Keeper" "Stick" "Mayor" "Clergy" "Scholar"

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
                choices[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black; // Example of highlighting
                choices[i].GetComponentInChildren<TextMeshProUGUI>().overflowMode = TextOverflowModes.ScrollRect; // set overflow to allow text to expand
                choices[i].GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize; //set box to exanded size

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
