using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;			// If needed
using Ink.Runtime;      // Make sure you have this using directive for Ink script

[System.Serializable]		// this lets me fill in the following two fields in the inspector
public class Building
{
    public string characterName;
    public GameObject light;
}

/// <summary>
/// A script that handles village UI and navigation, and which calls functions on the Dialogue Manager.
/// </summary>
public class VillageNavigationManager : MonoBehaviour
{

    //Building Selection
    public enum Buildings { SCHOLAR, CUSTODIAN, CRYPT_KEEPER, CHURCH, MAYOR }
    [Header("Building Selection")]      // Implemented by Justin Lam (Rxlling_Pxly)
    [Tooltip("0: Scholar, 1: Custodian, 2: Crypt Keeper, 3: Church, 4: Mayor")]
    [SerializeField] Building[] buildings = new Building[5];        // we know that there's only going to be 5 buildings, so we can use an array
    [Tooltip("Determines which building is selected at the start of the scene.")]
    [SerializeField] Buildings initialSelectedBuilding;
    public int selectedBuildingIndex;
    Building selectedBuilding;

    //Each "Buildings/NPC"
    [Header("Selection States")]
    [SerializeField] public bool currentlySelected = false;
    [SerializeField] public bool hasEntered = false;
    [SerializeField] private bool hasSelected = false;
    [SerializeField] private bool movePointer = true;

    // UI Stuff
    [Header("UI")]
    [SerializeField] private List<GameObject> DialogueUI = new List<GameObject>(); // List for UI elements
    [SerializeField] private GameObject DialogueTextUI; // List for UI elements
    public Image fadeOutUIImage; // Reference to the UI Image
    [SerializeField] public float fadeSpeed = 12f;

    //Ink
    [Header("Ink")]
    [SerializeField] private List<string> CharacterList = new List<string>(); // List for Ink
    public string CurrentCharacter; 
    public bool activateInk;
    public bool loadDungeon = false;

    //Story Variables
    private bool hasForcedCKIntro = false;
    private bool hasForcedClergyIntro = false;
    private bool hasForcedMayorIntro = false;

    // Global Teapot
    GlobalTeapot globalTeapot;

    // Audio Manager
    private AudioManager audioManager;

    // This Object
    private GameObject thisObject;

    private void Start()
    {
        // Get the Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        audioManager = GameObject.FindWithTag("Global Teapot").GetComponent<AudioManager>();
        audioManager.PlayVillageOST();

        // Building Selection:
        // Turn off every building's light
        foreach (Building building in buildings)
        {
            building.light.SetActive(false);
        }

        // Set selectedBuildingIndex to the index of the building that's initially selected at the start of the scene
        selectedBuildingIndex = (int)initialSelectedBuilding;

        // Set selectedBuilding to the building associated with selectedBuildingIndex
        selectedBuilding = buildings[selectedBuildingIndex];

        // Turn on the light of the selected building 
        buildings[selectedBuildingIndex].light.SetActive(true);

        thisObject = gameObject;
        CurrentCharacter = CharacterList[selectedBuildingIndex];

        // If this is the first time the village is visited, play village cutscene
        if (globalTeapot.currProgress == GlobalTeapot.TeaType.Intro)
        {
            // Dialogue Manager will change selected building back to clergy
            selectedBuildingIndex = 5;
            selectBuilding();
        } // Force Mayor Intro 
        else if (!globalTeapot.mayorWasIntroduced && globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F2)
        {
            moveBuildingSelection(1);
            selectBuilding();
        } // Force CK Intro After 1st Death
        else if (globalTeapot.deathCount >= 1 && (globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F1 || globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F2))
        { 
            moveBuildingSelection(-1);
            selectBuilding();
        }
        else if (false)
        { 

        }

        activateInk = false;
    }

    private void Update()
    {
        if (globalTeapot.currProgress == GlobalTeapot.TeaType.Intro)
        {
            // Force CK Intro After Intro Cutscene
            if (!hasSelected && !hasEntered && !hasForcedCKIntro )
            {
                Debug.Log("forcing CK INTRO");
                moveBuildingSelection(-3);
                selectBuilding();
                hasForcedCKIntro = true;
            }

            //// Force Clergy Intro After CK Intro
            //if (!hasSelected && !hasEntered && hasForcedCKIntro && !hasForcedClergyIntro)
            //{
            //    Debug.Log("forcing CLERGY INTRO");
            //    moveBuildingSelection(1);
            //    selectBuilding();
            //    hasForcedClergyIntro = true;
            //}
        }
        

        if (!movePointer) return;
    }

    public void moveBuildingSelection(int move, bool force = false)
    {
        if ((!currentlySelected || hasSelected) && !force) return;

        selectedBuildingIndex += move;
        // Hard-coded behavior for intro and first floor
        if (globalTeapot.currProgress == GlobalTeapot.TeaType.Intro || globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F1)
        {
            if (selectedBuildingIndex < 2)
            {
                selectedBuildingIndex = buildings.Length - 2;
            }
            else if (selectedBuildingIndex > 3)
            {
                selectedBuildingIndex = 2;
            }
        } else
        {
            if (selectedBuildingIndex < 0)
            {
                selectedBuildingIndex = buildings.Length - 1;
            }
            else if (selectedBuildingIndex >= buildings.Length)
            {
                selectedBuildingIndex = 0;
            }
        }
        

        selectedBuilding = buildings[selectedBuildingIndex];


        CurrentCharacter = CharacterList[selectedBuildingIndex];


        updateBuildingLights();
    }//NEEDS TO BE UPDATED FOR InputAction

    public void selectBuilding()
    {
        if (hasSelected)
        {
            return;
        }
        else
        {
            hasSelected = true;
        }
        if (selectedBuilding == null)
        {
            Debug.LogError("No selected GameObject.");
            return;
        }

        if (hasEntered)
        {
            hasEntered = false;
            if(selectedBuildingIndex == 0){
                audioManager.SetShopOST(false, 1/fadeSpeed);
            }
            else if(selectedBuildingIndex == 2){
                audioManager.PlayVillageOST();
            }
            StartCoroutine(FadeToBlack());
            return;
        };
        hasEntered = true;
        if(selectedBuildingIndex == 0){
            audioManager.SetShopOST(true, 1/fadeSpeed);
        }
        else if(selectedBuildingIndex == 2){
            audioManager.PlayCryptKeeperOST();
        }
        StartCoroutine(FadeToBlack());
    }

    private void updateBuildingLights()
    {
        // Turn on the light of the building that's selected
        selectedBuilding.light.SetActive(true);

        // Turn off the lights of all the other buildings
        foreach (Building building in buildings)
        {
            if (building != selectedBuilding)
            {
                building.light.SetActive(false);
            }
        }
    }//Turns on Light 

    private IEnumerator FadeToBlack() //Coroutine Function
    {
        print("FadingToBlack");
        Color objectColor = fadeOutUIImage.color;
        float fadeAmount;
        while (fadeOutUIImage.color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeOutUIImage.color = objectColor;
            yield return null;
        }

        if (selectedBuildingIndex == 3)
        {
            if (loadDungeon)
            {
                loadDungeon = false;
                Loader.Load(Loader.Scene.Level_Select, true);
                //Loader.Load(Loader.Scene.Dungeon_F1);
            }
            else
            {
                loadDungeon = true;
            }
        }

        StartCoroutine(FadeToClear());
    }

    private IEnumerator FadeToClear() //Coroutine Function
    {
        print("FadingToClear");
        Color objectColor = fadeOutUIImage.color;
        float fadeAmount;
        if (hasEntered)
        {
            ActivateUI(selectedBuildingIndex);
        }
        else
        {
            DeActivateUI(selectedBuildingIndex);
        }

        yield return new WaitForSeconds(2.0f/fadeSpeed);

        while (fadeOutUIImage.color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
        hasSelected = false;
    }

    private void ActivateUI(int index) //Activates UI Function
    {
        Debug.Log("ActivateUI");
        // Deactivate all UI elements
        foreach (var uiElement in DialogueUI)
        {
            uiElement.SetActive(false);
        }

        // Activate the UI element that corresponds to the selected object
        if (index >= 0 && index < DialogueUI.Count)
        {
            //UI Activate
            DialogueUI[index].SetActive(true);
            //Ink Activate
            activateInk = true;

            currentlySelected = false;
        }

        // Set Dialogue Text UI Visible
        DialogueTextUI.SetActive(true);

    }

    private void DeActivateUI(int index)  //Deactivates UI Function
    {
        Debug.Log("DeActivateUI");
        // Deactivate all UI elements
        activateInk = false;
        foreach (var uiElement in DialogueUI)
        {
            uiElement.SetActive(false);
        }

        currentlySelected = true;

        // AUDIOMANAGER: Village OST
    }


}
