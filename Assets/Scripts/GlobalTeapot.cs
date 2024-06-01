using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Very Special Teapot
/// </summary>
public class GlobalTeapot : MonoBehaviour
{
    /// <summary>
    /// A Very Special Teapot
    /// </summary>
    public static GlobalTeapot Instance { get; private set; }

    /// <summary>
    /// Story progress variables
    /// </summary>
    public enum TeaType
    {
        Intro,
        Dungeon_F1,
        Dungeon_F2,
        End
    }

    /// <summary>
    /// Connected Device Type
    /// </summary>
    public enum TeaCup
    {
        KEYBOARD,
        XINPUT,
        DUALSHOCK,
        SWITCH
    }

    [Header("Ink File")]
    [Tooltip("The master ink file.")]
    public TextAsset masterInk;
    public Ink.Runtime.Story currentStory;

    [Header("Story Variables")]
    public TeaType currProgress = TeaType.Intro;
    /// <summary>
    /// True if player has this
    /// </summary>
    public bool hasDied = false, toldCKAboutHighPriest = false;
    public bool mayorWasIntroduced = false, scholarWasIntroduced = false, highPriestWasIntroduced = false;
    public bool hasMayorNote1 = false, hasMayorNote2 = false, hasFinalMayorNote = false;
    public int deathCount = 0, stickHappiness = 0;
    public Loader.Scene currentScene => Loader.GetCurrentScene();

    [Header("Note Variables")]
    public int numNotesObtained = 0;
    public int numStoreCredits = 0;

    [Header("Scripts Referenced")]
    public AudioManager audioManager;
    public Fader fader;
    public Journal journal;
    public DeviceInputs deviceInputs;
    [SerializeField] private PlayerInventory inventory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        if (!journal) { journal = Resources.Load<Journal>("Journal"); }
        if (!deviceInputs) { deviceInputs = Resources.Load<DeviceInputs>("DeviceInputs"); }
        if (!inventory) { inventory = Resources.Load<PlayerInventory>("Player_Inventory"); }
        deviceInputs.Init();
        audioManager = GetComponent<AudioManager>();
        fader = GetComponent<Fader>();
        fader.Init();
        journal.CreateFloorNotes();

        currentStory = new Ink.Runtime.Story(masterInk.text);
        // could add error handling: currentStory.onError += ~~~~;
    }

    public void BrewNewTea()
    {
        currProgress = TeaType.Intro;
        currentStory.ResetState();
        journal.CreateFloorNotes();
        inventory.ResetInventory();

        hasDied = false;
        toldCKAboutHighPriest = false;
        mayorWasIntroduced = false;
        scholarWasIntroduced = false;
        highPriestWasIntroduced = false;
        hasMayorNote1 = false;
        hasMayorNote2 = false;
        hasFinalMayorNote = false;
        deathCount = 0;
        stickHappiness = 0;
        numNotesObtained = 0;
        numStoreCredits = 0;
    }

    /// <summary>
    /// Obtain floor note given its ID
    /// </summary>
    /// <param name="id">The ID of the note</param>
    public void ObtainFloorNote(string id)
    {
        if (id.Length <= 0)
        {
            return;
        }
        if (id == "DN_F2_Mayor1")
        {
            hasMayorNote1 = true;
        }
        else if (id == "DN_F2_Mayor2")
        {
            hasMayorNote2 = true;
        }
        else if (id == "MAYOR_FINAL_NOTE")
        {
            hasFinalMayorNote = true;
        }
        if (journal.ObtainFloorNote(id))
        {
            numNotesObtained++;
            numStoreCredits++;
        }
    }

    private void OnDisable()
    {
        deviceInputs.Deactivate();
    }
}