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

    // Start is called before the first frame update
    void Start()
    {
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
        //Below lines written by Tristyn. TY Tristyn!
        // Set selectedDoor to the door associated with selectedDoorIndex
        selectedDoor = doors[selectedDoorIndex];

        // Turn on the light of the selected building 
        doors[selectedDoorIndex].light.SetActive(true);

        thisObject = gameObject;
    }

    public void selectBuilding()
    {
        //uh oh! idk what to do here! Im going to return when I know what to do, and hopefully won't completely forget!

        //debug check taken from VilllageNavigationManager.cs
        if (selectedDoor == null)
        {
            Debug.LogError("No selected GameObject.");
            return;
        }

        //SceneManager.LoadScene(doors[selectedDoorIndex].levelSceneName);
        hasEntered = true;
        StartCoroutine(FadeToBlack());
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
        switch (selectedDoorIndex)
        {
            case 0:
                DungeonManager.instance.finishLevel = true;
                Loader.Load(Loader.Scene.Dungeon_F1);
                break;
            case 1:
                DungeonManager.instance.finishLevel = true;
                Loader.Load(Loader.Scene.Dungeon_F2);
                break;
            default: //defaults to floor 1 if something went wrong
                Loader.Load(Loader.Scene.Dungeon_F1);
                break;
        }

        //StartCoroutine(FadeToClear()); //FadeToClear not needed as no option would result in diaogue, only scene changes
    }
}
