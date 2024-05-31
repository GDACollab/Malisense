using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelDoor
{
    public string levelNumber;
    public GameObject light;
    public string levelSceneName;
}

public class LevelNavigationManager : MonoBehaviour
{
    enum LevelDoors { ONE, TWO }
    [Header("Door Selection")]      // Looked at Justin Lam's VillageNavigationManager.cs
    [Tooltip("0: LevelOne, 1: LevelTwo")]
    [SerializeField] LevelDoor[] doors = new LevelDoor[2];        // we know that there's only going to be 2 doors, so we can use an array
    [Tooltip("Determines which door is selected at the start of the scene.")]
    [SerializeField] LevelDoors initialSelectedDoor;
    public int selectedDoorIndex;
    LevelDoor selectedDoor;

    //Each "Buildings/NPC"
    [Header("Selection States")]
    [SerializeField] public bool currentlySelected = false;
    [SerializeField] public bool hasEntered = false; //was commented out
    [SerializeField] private bool hasSelected = false;
    [SerializeField] private bool movePointer = true;

    public Image fadeOutUIImage; // Reference to the UI Image
    [SerializeField] public float fadeSpeed = 12f;

    public bool loadDungeon = false;

    private GameObject thisObject;

    GlobalTeapot globalTeapot;

    //Image secondDoor;

    // Start is called before the first frame update
    void Start()
    {
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        //secondDoor = GameObject.FindWithTag("Second Door").GetComponent<Image>();

        if(globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F2){
            Debug.Log("The second door should be opened");
            //secondDoor.enabled = false;
            //secondDoor.Destroy();
        }
        // Level Door Selection:
        // Turn off every door's light
        foreach (LevelDoor door in doors)
        {
            door.light.SetActive(false);
        }

        // Set selectedDoorIndex to the index of the door that's initially selected at the start of the scene
        selectedDoorIndex = (int)initialSelectedDoor;

        //Update currently selected door based on current index
        //(Replaced lines previously here with function containing said lines)
        updateDoor();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveInList(int change)
    {
        if (selectedDoorIndex + change < 0 || selectedDoorIndex + change >= doors.Length)
            return;
        doors[selectedDoorIndex].light.SetActive(false);
        selectedDoorIndex += change;
        updateDoor();
    }

    //Updates currently selected door based on current index
    void updateDoor()
    {
        // Set selectedDoor to the door associated with selectedDoorIndex
        selectedDoor = doors[selectedDoorIndex];

        // Turn on the light of the selected building 
        doors[selectedDoorIndex].light.SetActive(true);

        thisObject = gameObject;
    }

    public void selectBuilding()
    {

        //debug check taken from VilllageNavigationManager.cs
        if (selectedDoor == null)
        {
            Debug.LogError("No selected GameObject.");
            return;
        }

        //Debug.Log(globalTeapot.currProgress);
        //Debug.Log(selectedDoorIndex);
        if(selectedDoorIndex == 0){
            hasEntered = true;
            StartCoroutine(FadeToBlack());
        }

        if(selectedDoorIndex == 1){
            if(globalTeapot.currProgress == GlobalTeapot.TeaType.Dungeon_F2){
                hasEntered = true;
                StartCoroutine(FadeToBlack());
            }else{
                Debug.Log("Sorry, you need to finish Dungeon 1 first");
            }    
        }
        /*if(selectedDoorIndex != 1){
            Debug.Log(globalTeapot.currProgress);
        }*/
            //Loader.Load(Loader.Scene.Dungeon_F1);    
       
        //hasEntered = true;
        //StartCoroutine(FadeToBlack());
    }

    //function taken from VilllageNavigationManager.cs, modified
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

        loadDungeon = false;

        /*if(selectedDoorIndex == 0){
            Loader.Load(Loader.Scene.Dungeon_F1);    
        }else if(selectedDoorIndex == 1 && DungeonManager.instance.finishLevel == true){
            Debug.Log("SECOND DOOR HAS BEEN SELECTED");
            Loader.Load(Loader.Scene.Dungeon_F2);    
        }else{
            Debug.Log("Sorry, you haven't completed Dungeon 1 yet.");
        }*/
        
    
        switch (selectedDoorIndex)
        {
            case 0:
                //DungeonManager.instance.finishLevel = true;
                Debug.Log("Case 0");
                Loader.Load(Loader.Scene.Dungeon_F1);
                //Loader.Load(Loader.Scene.Dungeon_F2);
                break;
            case 1:
                Debug.Log("Case 1");
                //DungeonManager.instance.finishLevel = true;
                Loader.Load(Loader.Scene.Dungeon_F2);
                break;
            default: //defaults to floor 1 if something went wrong
                Debug.Log("Something must have happened");
                Loader.Load(Loader.Scene.Dungeon_F1);
                break;
        }

        //StartCoroutine(FadeToClear()); //FadeToClear not needed as no option would result in diaogue, only scene changes
    }
}

