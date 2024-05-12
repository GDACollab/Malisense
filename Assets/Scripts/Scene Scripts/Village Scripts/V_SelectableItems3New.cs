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

public class V_SelectableItems3New : MonoBehaviour
{

    //Building Selection
    enum Buildings { SCHOLAR, CUSTODIAN, CRYPT_KEEPER, CHURCH, MAYOR }
    [Header("Building Selection")]      // Implemented by Justin Lam (Rxlling_Pxly)
    [Tooltip("0: Scholar, 1: Custodian, 3: Crypt Keeper, 4: Church, 5: Mayor")]
    [SerializeField] Building[] buildings = new Building[5];        // we know that there's only going to be 5 buildings, so we can use an array
    [Tooltip("Determines which building is selected at the start of the scene.")]
    [SerializeField] Buildings initialSelectedBuilding;
    int selectedBuildingIndex;
    Building selectedBuilding;

    //TMP Variables
    [Header("Temp Playtest Variables")]
    public GameObject dungeonMessage;

    //Each "Buildings/NPC"
    [SerializeField] private List<GameObject> UI_ELEMENTS = new List<GameObject>(); // List for UI elements
    [SerializeField] private List<TextAsset> InkScripts = new List<TextAsset>(); // List for Inkle Stuff :3
    [SerializeField] private List<string> CharacterList = new List<string>(); // List for Ink
    [SerializeField] public bool currentlySelected = false;
    [SerializeField] public bool hasEntered = false;
    [SerializeField] private bool hasSelected = false;
    [SerializeField] private bool movePointer = true;
    private GameObject thisObject;

    //UI Stuff
    public Image fadeOutUIImage; // Reference to the UI Image
    [SerializeField] public float fadeSpeed = 12f;

    //Inkle
    public TextAsset CurrentInkTextAsset;
    public string CurrentCharacter;
    public bool activateInk;
    public bool loadDungeon = false;
    
    // Global Teapot
    GlobalTeapot globalTeapot;
    int ckTea;
    int clergyTea;
    private AudioManager audioManager;

    private void Start()
    {
        // Get the Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        audioManager = GameObject.FindWithTag("Global Teapot").GetComponent<AudioManager>();
        audioManager.PlayOST(audioManager.dungeonOST);
        ckTea = globalTeapot.villageInk;
        clergyTea = globalTeapot.villageInk;
        
        
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
        CurrentInkTextAsset = InkScripts[0];
        CurrentCharacter = CharacterList[0];
        activateInk = false;
        
        if(globalTeapot.villageInk == 0){
            CurrentInkTextAsset = InkScripts[9];
            // what should i be setting here? CurrentCharacter = CharacterList[9];
            selectedBuildingIndex = 5;
            selectObject();
        }
        else if(globalTeapot.villageInk == 2){
            moveInList(-1);
            selectObject();
        }
    }

    private void Update()
    {
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
        
        // ### FIX AFTER VS PLEASE!!! ###
        if(selectedBuildingIndex == 2){
            if(ckTea == 0){
                CurrentInkTextAsset = InkScripts[selectedBuildingIndex];
                CurrentCharacter = CharacterList[selectedBuildingIndex];
            }
            else if(ckTea == 2){
                CurrentInkTextAsset = InkScripts[6];
                // what should i be setting here? CurrentCharacter = CharacterList[6];
            }
            else if(ckTea == 3){
                CurrentInkTextAsset = InkScripts[7];
                // what should i be setting here? CurrentCharacter = CharacterList[7];
            }
            else
            {
                CurrentInkTextAsset = InkScripts[5];
                // what should i be setting here? CurrentCharacter = CharacterList[5];
            }
        }
        else if(selectedBuildingIndex == 3 && clergyTea == 2){
            CurrentInkTextAsset = InkScripts[8];
            // what should i be setting here?CurrentCharacter = CharacterList[8];
        }
        else
        {
            CurrentInkTextAsset = InkScripts[selectedBuildingIndex];
            CurrentCharacter = CharacterList[selectedBuildingIndex];
        }

        itemSelected();
    }//NEEDS TO BE UPDATED FOR InputAction

    public void selectObject()
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
            if(loadDungeon)
            {
                loadDungeon = false;
                Loader.Load(Loader.Scene.Dungeon);
            }
            else{
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

        yield return new WaitForSeconds(2.0f);

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
        foreach (var uiElement in UI_ELEMENTS)
        {
            uiElement.SetActive(false);
            activateInk = false;
        }
        
        if(selectedBuildingIndex == 2){
            if(ckTea != 3){
                ckTea = 1;
                moveInList(0, true);
            }
        }
        else if(selectedBuildingIndex == 3){
            clergyTea = 1;
            moveInList(0, true);
        }
        else if(selectedBuildingIndex == 5){
            moveInList(-2, true);
        }
        
        currentlySelected = true;
    }


}
