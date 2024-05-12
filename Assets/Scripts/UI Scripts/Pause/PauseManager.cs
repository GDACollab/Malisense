using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseParent, optionsMenu, journalDisplay;
    [SerializeField] Image journalTab, settingsTab;
    [SerializeField] Slider masterSlider, musicSlider, sfxSlider, ambianceSlider;
    [SerializeField] List<TextMeshProUGUI> notePages;
    [SerializeField] Animator rightArrow, leftArrow;
    [SerializeField] Button debug;
    public static bool isPaused;
    
    enum Submenu {SETTINGS, JOURNAL}
    Submenu currSubMenu = Submenu.SETTINGS;
    
    PlayerInput uiInput;
    List<PlayerInput> otherInputs;
    
    GlobalTeapot globalTeapot;
    List<string> floorNotes;
    int lastPageDisplayed = 0;

    void Start()
    {
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        uiInput = GetComponent<PlayerInput>();
        otherInputs = FindObjectsOfType<PlayerInput>().ToList();
        otherInputs.Remove(uiInput);
        isPaused = false;
        masterSlider.value = globalTeapot.audioManager.masterVolume;
        musicSlider.value = globalTeapot.audioManager.musicVolume;
        sfxSlider.value = globalTeapot.audioManager.sfxVolume;
        ambianceSlider.value = globalTeapot.audioManager.ambienceVolume;
    }
    
    public void UpdateVolume(){
        globalTeapot.audioManager.masterVolume = masterSlider.value;
        globalTeapot.audioManager.musicVolume = musicSlider.value;
        globalTeapot.audioManager.sfxVolume = sfxSlider.value;
        globalTeapot.audioManager.ambienceVolume = ambianceSlider.value;
    }
    
    public void OnMove(InputValue value)
    {
        if (currSubMenu==Submenu.JOURNAL && Mathf.Abs(value.Get<Vector2>().x)>0)
        {
            FlipJournalPage(value.Get<Vector2>().x>0);
        }
    }
    
    public void OnPause(){
        TogglePause();
    }
    
    public void OnJournalTab(){
        if(isPaused){OpenJournal();}
    }
    
    public void OnSettingsTab(){
        if(isPaused){OpenSettings();}
    }
    
    public void OnDebug(){
        if(isPaused){debug.onClick.Invoke();}
    }

    void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(!isPaused)
        {
            SetOtherInputs(false);
            pauseParent.SetActive(true);
            OpenSettings(true);
            floorNotes = globalTeapot.journal.GetObtainedFloorNotes();
            Time.timeScale = 0f;
            isPaused = true;
        }
    }
    
    public void ResumeGame()
    {
        pauseParent.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SetOtherInputs(true);
    }

    
    public void OpenSettings(bool force = false){
        if(currSubMenu == Submenu.SETTINGS && !force){return;}
        currSubMenu = Submenu.SETTINGS;
        optionsMenu.SetActive(true);
        journalDisplay.SetActive(false);
        settingsTab.color = Color.white;
        journalTab.color = Color.grey;
        masterSlider.Select();
    }
    
    public void OpenJournal(){
        if(currSubMenu == Submenu.JOURNAL){return;}
        currSubMenu = Submenu.JOURNAL;
        optionsMenu.SetActive(false);
        journalDisplay.SetActive(true);
        settingsTab.color = Color.grey;
        journalTab.color = Color.white;
        DisplayJournalPages(lastPageDisplayed);
    }
    
    void DisplayJournalPages(int ind){
        int count = ind;
        foreach(var page in notePages){
            if(count>=floorNotes.Count){
                page.text = "";
            }
            else{
                page.text = floorNotes[count];
                count++;
            }
        }
        lastPageDisplayed = ind;
    }
    
    public void FlipJournalPage(bool next){
        if(next){
            if(lastPageDisplayed+2>=floorNotes.Count){
                return;
            }
            rightArrow.SetTrigger("Pressed");
            DisplayJournalPages(lastPageDisplayed+2);
        }
        else{
            if(lastPageDisplayed-2<0){
                return;
            }
            leftArrow.SetTrigger("Pressed");
            DisplayJournalPages(lastPageDisplayed-2);
        }
    }
    
    public void GoToTitleScreen(){
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
    
    public void Load(int ind){
        ResumeGame();
        switch (ind)
        {
            case 0: 
                Loader.Load(Loader.Scene.Village);
                break;
            case 1: 
                Loader.Load(Loader.Scene.Dungeon);
                break;
            case 2: 
                Loader.Load(Loader.Scene.Dungeon);
                break;
            default:
                Loader.Load(Loader.Scene.MainMenu);
                break;
        }
    }
    
    void SetOtherInputs(bool enable){
        foreach(PlayerInput input in otherInputs){
            input.enabled = enable;
        }
    }
}