using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

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
        Dungeon_F2,
        End
    }
    
    public bool hasDied = false, hasMayorNote1 = false, hasMayorNote2 = false, hasFinalMayorNote = false;
    public string mayorNote1ID, mayorNote2ID, finalMayorNoteID;
    
    public TeaType currProgress = TeaType.Intro;
    
    public int villageInk = 0;
    public AudioManager audioManager;
    public Loader.Scene currentScene = Loader.Scene.DeathScene;
    
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
    
    public void ObtainFloorNote(string id){
        if(id.Length<=0){
            return;
        }
        if(id == mayorNote1ID){
            hasMayorNote1 = true;
        }
        else if(id == mayorNote2ID){
            hasMayorNote2 = true;
        }
        else if(id == finalMayorNoteID){
            hasFinalMayorNote = true;
        }
        journal.ObtainFloorNote(id);
    }
}