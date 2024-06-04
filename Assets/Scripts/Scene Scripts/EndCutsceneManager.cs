using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using System;


public class EndCutsceneManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private GameObject CKSprite;
    [SerializeField] private GameObject MayorSprite;
    [SerializeField] private GameObject DisgracedSprite;
    [SerializeField] private GameObject DarknessSprite;
    [SerializeField] private GameObject DarkSprite;
    [SerializeField] private VideoPlayer endCutsceneAnimation;
    [SerializeField] private GameObject EndImage;
    [SerializeField] private GameObject EndHint;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    [SerializeField] private int currentChoiceIndex = -1;
    private int defaultHeight = 115;

    [Header("Status")]
    [SerializeField] private bool isPlaying;
    [SerializeField] private float fadeInTime = 2f;
    [SerializeField] private float fadeOutTime = 2f;
    [SerializeField] private float waitOnEndImage = 3f;
    [SerializeField] private float characterTranstionTime = 1f;
    private bool movingOn = false;
    bool setCharacter;

    // Global Teapot
    GlobalTeapot globalTeapot;
    PlayerInput playerInput;
    InputAction move;

    private bool canStartDialogue = true;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions.FindAction("Move");
        // Get the Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        globalTeapot.currProgress = GlobalTeapot.TeaType.End;

        isPlaying = true;
        dialoguePanel.SetActive(false);

        globalTeapot.currentStory.ResetState();
        globalTeapot.currentStory.ObserveVariable("character", ChangeImage);
        globalTeapot.audioManager.PlayCryptKeeperOST();
        globalTeapot.mayorWasIntroduced = true;

        endCutsceneAnimation.frame = 0;

        //Get all choices texts
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        EnterDialogueMode("Mayor");
        StartCoroutine(globalTeapot.fader.FadeFromBlack(fadeInTime));
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
        }
        else
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
        if (character == "Clergy" && !globalTeapot.highPriestWasIntroduced && globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F2)
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

        globalTeapot.currentStory.variablesState["character"] = character; // "Crypt_Keeper" "Stick" "Mayor" "Clergy" "Scholar"
        canStartDialogue = false;
        isPlaying = true;

        dialoguePanel.SetActive(true);
        setCharacter = false;
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        isPlaying = false;
        Action action = () =>
        {
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
            characterNameText.text = "";
            endCutsceneAnimation.gameObject.SetActive(true);
            StartCoroutine(globalTeapot.fader.FadeFromBlack(0.5f));
            endCutsceneAnimation.Play();
            StartCoroutine(ShowEndImage());
        };
        StartCoroutine(globalTeapot.fader.FadeToBlack(action, 1f));

    }

    private IEnumerator ShowEndImage()
    {
        yield return new WaitForSeconds((float)endCutsceneAnimation.clip.length + 1f);
        Action action = () =>
        {
            EndImage.SetActive(true);
            StartCoroutine(globalTeapot.fader.FadeFromBlack(1f));
        };
        StartCoroutine(globalTeapot.fader.FadeToBlack(action, 1f));
        yield return new WaitForSeconds(waitOnEndImage);
        EndHint.SetActive(true);
        EndHint.GetComponent<Animator>().SetBool("SkipHint", true);
        movingOn = true;
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
            if (setCharacter)
            {
                Action action = () =>
                {
                    StartCoroutine(globalTeapot.fader.FadeFromBlack(characterTranstionTime));
                    playerInput.enabled = true;
                    LogicContStory();
                };
                if ((string)globalTeapot.currentStory.variablesState["character"] == "Crypt_Keeper")
                {
                    action += () =>
                    {
                        CKSprite.SetActive(true);
                        MayorSprite.SetActive(false);
                        DisgracedSprite.SetActive(false);
                        DarknessSprite.SetActive(false);
                        globalTeapot.audioManager.StopCurrentSong();

                        StartCoroutine(WaitToCall(() => globalTeapot.audioManager.PlayEndingOST(), 1f));
                    };
                }
                else if ((string)globalTeapot.currentStory.variablesState["character"] == "Mayor")
                {
                    action += () =>
                    {
                        MayorSprite.SetActive(true);
                        CKSprite.SetActive(false);
                        DisgracedSprite.SetActive(false);
                        DarknessSprite.SetActive(false);
                    };
                }
                else if ((string)globalTeapot.currentStory.variablesState["character"] == "Disgraced")
                {
                    action += () =>
                    {
                        DisgracedSprite.SetActive(true);
                        CKSprite.SetActive(false);
                        MayorSprite.SetActive(false);
                        DarknessSprite.SetActive(false);
                    };
                }
                else if ((string)globalTeapot.currentStory.variablesState["character"] == "Darkness")
                {
                    action += () =>
                    {
                        DarknessSprite.SetActive(true);
                        DisgracedSprite.SetActive(false);
                        CKSprite.SetActive(false);
                        MayorSprite.SetActive(false);
                    };
                }
                else if ((string)globalTeapot.currentStory.variablesState["character"] == "Dark")
                {
                    action += () => { DarkSprite.SetActive(true); };
                }
                setCharacter = false;
                playerInput.enabled = false;
                StartCoroutine(globalTeapot.fader.FadeToBlack(action, characterTranstionTime));
            }
            else
            {
                LogicContStory();
            }
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void LogicContStory()
    {
        dialogueText.text = globalTeapot.currentStory.Continue();
        // Villager name stored in variable characterNameText.text is one of "????????????", "Crypt Keeper", "Clerics", "Smiling Cleric", Thinking Cleric, "Weeping Cleric", "Smiling", "Weeping", "Thinking", "High Priest", "Stick", "Scholar", "Mayor"]

        characterNameText.text = (string)globalTeapot.currentStory.variablesState["CharacterTitle"];

        // Only bark when a character is talking:
        if (characterNameText.text != "")
        {
            // Name of owner of house stored in variable navigationManager.CurrentCharacter and is one of ["Scholar", "Stick", "Crypt_Keeper", "Clergy", "Mayor", "???????????????"]
            // AUDIOMANAGER: Villager barks
            VillageNavigationManager.Buildings buildingIndex = ((string)globalTeapot.currentStory.variablesState["character"] == "Crypt_Keeper") ? VillageNavigationManager.Buildings.CRYPT_KEEPER : VillageNavigationManager.Buildings.MAYOR;
            globalTeapot.audioManager.PlayVillageBark(buildingIndex);
        }

        DisplayChoices();
    }

    IEnumerator WaitToCall(Action act, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        act();
    }

    private void ChangeImage(string name, object value)
    {
        setCharacter = true;
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
        UpdateChoiceSelectionVisuals();
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
            UpdateChoiceSelectionVisuals();
        }
    }

    public void OnMove()
    {
        if (globalTeapot.currentStory.currentChoices.Count > 0)
        {
            // Move
            float horiMove = move.ReadValue<Vector2>().x;
            if (horiMove < 0) // left
            {
                // Navigate up in the choices list
                currentChoiceIndex--;
                if (currentChoiceIndex < 0) currentChoiceIndex = globalTeapot.currentStory.currentChoices.Count - 1;
            }
            else if (horiMove > 0) // right
            {
                // Navigate down in the choices list
                currentChoiceIndex++;
                if (currentChoiceIndex >= globalTeapot.currentStory.currentChoices.Count) currentChoiceIndex = 0;
            }
        }
    }

    public void OnSelect()
    {
        if (isPlaying)
        {
            selectDialogue();
        }
        else if (movingOn)
        {
            StartCoroutine(globalTeapot.fader.FadeToBlack(() => Loader.Load(Loader.Scene.Credits, true), fadeOutTime));
        }
    }
}
