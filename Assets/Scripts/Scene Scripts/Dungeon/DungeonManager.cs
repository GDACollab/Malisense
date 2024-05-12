using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using TMPro;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] List<StateMachine> enemies = new List<StateMachine>();
    [SerializeField] bool isChasing = false;
    
    [Header("Floor Note UI")]
    [SerializeField] GameObject floorNoteDisplay;
    TextMeshProUGUI floorNoteText;
    
    GlobalTeapot globalTeapot;
    AudioManager audioManager;
    
    List<FloorNote> floorNotes = new List<FloorNote>();
    
    // Start is called before the first frame update
    void Start()
    {
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        audioManager = globalTeapot.audioManager;
        floorNotes = FindObjectsOfType<FloorNote>().ToList();
        floorNoteText = floorNoteDisplay.GetComponentInChildren<TextMeshProUGUI>();
        
        SetFloorNotes();
    }
    
    /// <summary>
    /// Add a chasing enemy
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(StateMachine enemy){
        if(enemy.currentState == StateMachine.State.Chasing){
            enemies.Append(enemy);
            audioManager.PlayScream(enemy.GetComponent<StudioEventEmitter>());
            UpdateMusic();
        }
    }
    
    /// <summary>
    /// Remove an enemy from the list
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemy(StateMachine enemy){
        enemies.Remove(enemy);
        UpdateMusic();
    }
    
    /// <summary>
    /// Update the music based on 
    /// </summary>
    void UpdateMusic(){
        if(!isChasing && enemies.Count > 0){
            isChasing = true;
            audioManager.ChaseOST();
        }
        else if(isChasing && enemies.Count==0){
            isChasing = false;
            audioManager.DungeonOST();
        }
    }
    
    void SetFloorNotes(){
        foreach(FloorNote note in floorNotes){
            if(note.noteID.Length>0){
                if(!globalTeapot.journal.CheckFloorNote(note.noteID)){
                    note.noteTitle = "";
                    note.noteBody = globalTeapot.journal.ReadFloorNote(note.noteID);
                }
                else{
                    Destroy(note.gameObject);
                }
            }
        }
    }
    
    public void ActivateNote(FloorNote note){
        globalTeapot.ObtainFloorNote(note.noteID);
        floorNoteDisplay.SetActive(true);
        floorNoteText.text = note.noteBody;
        Destroy(note.gameObject);
        if (Time.timeScale != 0f) 
        {
            Time.timeScale = 0f;
        }
    }
    
    public void DeactivateNote(){
        floorNoteDisplay.SetActive(false);
        floorNoteText.text = "";
        if (Time.timeScale == 0f) 
        {
            Time.timeScale = 1f;
        }
    }
    
    public void KillPlayer(){
        EndDungeon(true, false);
    }
    
    public void EndDungeon(bool death=false, bool artifact=false){
        globalTeapot.hasDied = death;
        if(artifact){
            switch (globalTeapot.currProgress)
            {
                case GlobalTeapot.TeaType.Intro:
                    globalTeapot.currProgress = GlobalTeapot.TeaType.Dungeon_F2;
                    break;
                case GlobalTeapot.TeaType.Dungeon_F2:
                    globalTeapot.currProgress = GlobalTeapot.TeaType.End;
                    break;
                default:
                    globalTeapot.currProgress = GlobalTeapot.TeaType.Intro;
                    break;
            }
        }
        if(death){
            Loader.Load(Loader.Scene.DeathScene);
        }
        else{
            Loader.Load(Loader.Scene.Village);
        }
    }
}