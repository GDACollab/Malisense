using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    int monsterAlertState;

    [Header("Volumes (sliders)")]
    [Range(0f, 1f)]
    public float masterVolume = 0.5f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.5f;

    [Header("FMOD Music")]
    [Tooltip("FMOD Event Path for the menu music")]
    public string menuMusic = "event:/Music/MenuMusic";

    [Header("FMOD UI(SFX) Event Path Strings")]
    public string selectUI = "event:/SFX/UI & Menu/UI Select";
    public string sliderUI = "event:/SFX/UI & Menu/UI Slider Feedback";

    private EventInstance currentMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }

        Loader.Initialize(); // Start loading when AudioManager is initialized
    }

    void Start()
    {
        Play(menuMusic); // Play menu music when AudioManager starts
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

        if (path == menuMusic)
        {
            currentMusic = instance;
        }
    }

    public void StopCurrentMusic()
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
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
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicVolume", musicVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SFXVolume", sfxVolume);
    }
}
