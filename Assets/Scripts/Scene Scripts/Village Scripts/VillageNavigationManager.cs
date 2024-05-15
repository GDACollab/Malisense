using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;			// If needed
using Ink.Runtime;      // Make sure you have this using directive for Ink script

[System.Serializable]		// this lets me fill in the following two fields in the inspector
public class Building
{
    public GameObject light;
}

public class VillageNavigationManager : MonoBehaviour
{

    //Building Selection
    enum Buildings { SCHOLAR, CUSTODIAN, CRYPT_KEEPER, CHURCH, MAYOR }
    [Header("Building Selection")]      // Implemented by Justin Lam (Rxlling_Pxly)
    [Tooltip("0: Scholar, 1: Custodian, 3: Crypt Keeper, 4: Church, 5: Mayor")]
    [SerializeField] Building[] buildings = new Building[5];        // we know that there's only going to be 5 buildings, so we can use an array
    [Tooltip("Determines which building is selected at the start of the scene.")]
    [SerializeField] Buildings initialSelectedBuilding;
    public int selectedBuildingIndex;
    Building selectedBuilding;

    //TMP Variables
    [Header("Temp Playtest Variables")]
    public GameObject dungeonMessage;

    //Each "Buildings/NPC"
    [SerializeField] private List<GameObject> UI_ELEMENTS = new List<GameObject>(); // List for UI elements
    [SerializeField] private List<string> CharacterList = new List<string>(); // List for Ink
    [SerializeField] public bool currentlySelected = false;
    [SerializeField] public bool hasEntered = false;
    [SerializeField] private bool hasSelected = false;
    [SerializeField] private bool movePointer = true;
    private GameObject thisObject;

    //UI Stuff
    public Image fadeOutUIImage; // Reference to the UI Image
    [SerializeField] public float fadeSpeed = 12f;

    //Ink
    public string CurrentCharacter;
    public bool activateInk;
    public bool loadDungeon = false;

    //Story Variables
    private bool hasForcedCKIntro = false;

    // Global Teapot
    GlobalTeapot globalTeapot;

    private AudioManager audioManager;

    private void Start()
    {
        // Get the Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        audioManager = GameObject.FindWithTag("Global Teapot").GetComponent<AudioManager>();
        audioManager.PlayOST(audioManager.dungeonOST);

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
        } // Force CK Intro After 1st Death
        else if (globalTeapot.deathCount == 1 && globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F1)
        { 
            moveInList(-1);
            selectBuilding();
        } // TODO: Force Mayor Intro 
        else if (false)
        { 
        }

        activateInk = false;
    }

    private void Update()
    {
        // Force CK Intro After Intro Cutscene
        if (!hasSelected && !hasEntered && !hasForcedCKIntro && globalTeapot.currProgress == GlobalTeapot.TeaType.Intro)
        {
            Debug.Log("forcing CK INTRO");
            moveInList(-3);
            selectBuilding();
            hasForcedCKIntro = true;
        }

        if (!movePointer) return;
    }

    public void moveInList(int move, bool force = false)
    {
        if ((!currentlySelected || hasSelected) && !force) return;

        selectedBuildingIndex += move;
        if (selectedBuildingIndex < 0)
        {
            selectedBuildingIndex = buildings.Length - 1;
        }
        else if (selectedBuildingIndex >= buildings.Length)
        {
            selectedBuildingIndex = 0;
        }

        selectedBuilding = buildings[selectedBuildingIndex];


        CurrentCharacter = CharacterList[selectedBuildingIndex];


        itemSelected();
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
            StartCoroutine(FadeToBlack());
            return;
        };
        hasEntered = true;
        StartCoroutine(FadeToBlack());

    }

    private void itemSelected()
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

        dungeonMessage.SetActive(selectedBuildingIndex == 3);
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
                Loader.Load(Loader.Scene.Dungeon);
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
        foreach (var uiElement in UI_ELEMENTS)
        {
            uiElement.SetActive(false);
        }

        // Activate the UI element that corresponds to the selected object
        if (index >= 0 && index < UI_ELEMENTS.Count)
        {
            //UI Activate
            UI_ELEMENTS[index].SetActive(true);
            //Ink Activate
            activateInk = true;

            currentlySelected = false;
        }

    }

    private void DeActivateUI(int index)  //Deactivates UI Function
    {
        Debug.Log("DeActivateUI");
        // Deactivate all UI elements
        activateInk = false;
        foreach (var uiElement in UI_ELEMENTS)
        {
            uiElement.SetActive(false);
        }

        currentlySelected = true;
    }


}
