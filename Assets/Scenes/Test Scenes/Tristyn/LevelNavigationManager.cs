using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LevelDoor
{
    public string levelNumber;
    public GameObject light;
}

public class LevelNavigationManager : MonoBehaviour
{
    enum LevelDoors { ONE, TWO }
    [Header("Door Selection")]      // Looked at Justin Lam's VillageNavigationManager.cs
    [Tooltip("0: LevelOne, 1: LevelTwo")]
    [SerializeField] LevelDoor[] doors = new LevelDoor[2];        // we know that there's only going to be 5 buildings, so we can use an array
    [Tooltip("Determines which door is selected at the start of the scene.")]
    [SerializeField] LevelDoors initialSelectedDoor;
    public int selectedDoorIndex;
    LevelDoor selectedDoor;

    //Each "Buildings/NPC"
    [Header("Selection States")]
    [SerializeField] public bool currentlySelected = false;
    //[SerializeField] public bool hasEntered = false;
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

        // Set selectedDoor to the door associated with selectedDoorIndex
        selectedDoor = doors[selectedDoorIndex];

        // Turn on the light of the selected building 
        doors[selectedDoorIndex].light.SetActive(true);

        thisObject = gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
