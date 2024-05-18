using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseParent, optionsMenu, journalDisplay, controlsDisplay;
    [SerializeField] Image journalTab, settingsTab;
    [SerializeField] Slider masterSlider, musicSlider, sfxSlider, ambianceSlider;
    [SerializeField] List<TextMeshProUGUI> notePages;
    [SerializeField] Animator rightArrow, leftArrow;
    [SerializeField] Button debug;
    public static bool isPaused;
    
    enum Submenu {SETTINGS, JOURNAL, CONTROLS}
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
        DisableUIInput();
        
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
            EnableUIInput();
        }
    }
    
    public void ResumeGame()
    {
        pauseParent.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        DisableUIInput();
        SetOtherInputs(true);
    }

    public void OpenSettings(bool force = false){
        OpenMenu(Submenu.SETTINGS);
        if(force){masterSlider.Select();}
    }
    
    public void OpenJournal(){
        OpenMenu(Submenu.JOURNAL);
    }
    
    public void OpenControls(){
        OpenMenu(Submenu.CONTROLS);
    }
    
    void OpenMenu(Submenu submenu){
        if(currSubMenu == submenu){return;}
        currSubMenu = submenu;
        optionsMenu.SetActive(false);
        journalDisplay.SetActive(false);
        controlsDisplay.SetActive(false);
        settingsTab.color = Color.grey;
        journalTab.color = Color.grey;
        
        switch(submenu){
            case Submenu.SETTINGS:
                optionsMenu.SetActive(true);
                settingsTab.color = Color.white;
                masterSlider.Select();
                break;
            case Submenu.JOURNAL:
                journalDisplay.SetActive(true);
                journalTab.color = Color.white;
                DisplayJournalPages(lastPageDisplayed);
                break;
            case Submenu.CONTROLS:
                controlsDisplay.SetActive(true);
                break;
            default:
                optionsMenu.SetActive(true);
                settingsTab.color = Color.white;
                masterSlider.Select();
                break;
        }
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
    
    void EnableUIInput(){
        uiInput.currentActionMap.Enable();
    }
    
    void DisableUIInput(){
        uiInput.currentActionMap.Disable();
        uiInput.currentActionMap.FindAction("Pause").Enable();
    }
}