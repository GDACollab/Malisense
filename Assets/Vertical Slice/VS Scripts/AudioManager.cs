using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    //public static AudioManager instance;
    private Dictionary<string, EventInstance> playingInstances = new Dictionary<string, EventInstance>();
    public EventInstance currentPlaying;
    //int monsterAlertState;
    protected FMOD.Studio.PLAYBACK_STATE playbackState;

    [Header("Volumes (sliders)")]
    [Range(0f, 1f)]
    public float masterVolume = 0.5f;
    //[Range(0f, 1f)]
    /*public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.5f;*/

    [Header("FMOD OST")]
    [Tooltip("FMOD Event Path for the menu music")]
    public string menuOST = "event:/VS_Malisense/Menu";

    [Tooltip("FMOD Event Path for the dungeon ambience")]
    public string dungeonOST = "event:/VS_Malisense/Dun_Ambience";

    [Header("FMOD DUNGEON(SFX) Event Path Strings")]
    public string monsterScream = "event:/VS_Malisense/Monster_Scream";
    [Header("FMOD UI(SFX) Event Path Strings")]
    public string selectUI = "event:/SFX/UI & Menu/UI Select";
    public string sliderUI = "event:/SFX/UI & Menu/UI Slider Feedback";

    void Start()
    {
        Play(menuOST); // Play menu music when AudioManager starts
    }

    public void Play(string path)
    {
        EventDescription eventDescription;
        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out eventDescription);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogWarning("FMOD event path does not exist: " + path);
            return;
        }

        var instance = RuntimeManager.CreateInstance(path);
        instance.start();
        instance.release();

    }
    public void PlayOST(string path){
        Debug.Log("[Audio Manager] Playing Song: " + path);
        if (currentPlaying.isValid()) {
            Debug.Log("Current playing is valid");
            StartCoroutine(RestOfPlayOST(path));
        }
        else
        {
            EventDescription eventDescription;
            FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out eventDescription);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogWarning("[Audio Manager] FMOD SONG event path does not exist: " + path);
                return;
            }

            EventInstance song = RuntimeManager.CreateInstance(path);
            currentPlaying = song;
            Debug.Log(currentPlaying);
            song.start();
            song.release();
        }
    }
    public IEnumerator RestOfPlayOST(string path){
        Debug.Log(currentPlaying + " (rest of Play OST)");
        //currentPlaying.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        currentPlaying.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentPlaying.getPlaybackState(out playbackState);
        while(playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED){
            currentPlaying.getPlaybackState(out playbackState);
            yield return null;
        }

        EventDescription eventDescription;
        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out eventDescription);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogWarning("[Audio Manager] FMOD SONG event path does not exist: " + path);
        }
        else
        {
            yield return new WaitForSeconds(1);
            EventInstance song = RuntimeManager.CreateInstance(path);
            currentPlaying = song;
            song.start();
            song.release();
        }
    }

    public void PlayUISelect()
    {
        Play(selectUI);
    }

    public void PlayUISlider()
    {
        Play(sliderUI);
    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MasterVolume", masterVolume);
        //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicVolume", musicVolume);
        //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SFXVolume", sfxVolume);
    }
}
