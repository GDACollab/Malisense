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
    
    [Header("Story Variables")]
    public TeaType currProgress = TeaType.Intro;
    /// <summary>
    /// True if player has this
    /// </summary>
    public bool hasDied = false, hasMayorNote1 = false, hasMayorNote2 = false, hasFinalMayorNote = false;
    public Loader.Scene currentScene = Loader.Scene.DeathScene;
    public int villageInk = 0; // DELETE ONCE EVERYTHING IS CONNECTED PROPERLY
    
    [Header("Note Variables")]
    public int numNotesObtained = 0;
    public int numStoreCredits = 0;
    
    [Header("Scripts Referenced")]
    public AudioManager audioManager;
    public Journal journal;
    
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
        audioManager = GetComponent<AudioManager>();
        journal.CreateFloorNotes();
    }
    
    private void Update() {
        currentScene = Loader.GetCurrentScene();
    }
    
    /// <summary>
    /// Obtain floor note given its ID
    /// </summary>
    /// <param name="id">The ID of the note</param>
    public void ObtainFloorNote(string id){
        if(id.Length<=0){
            return;
        }
        if(id == "DN_F2_Mayor1"){
            hasMayorNote1 = true;
        }
        else if(id == "DN_F2_Mayor2"){
            hasMayorNote2 = true;
        }
        else if(id == "MAYOR_FINAL_NOTE"){
            hasFinalMayorNote = true;
        }
        if(journal.ObtainFloorNote(id)){
            numNotesObtained++;
            numStoreCredits++;
        }
    }
}