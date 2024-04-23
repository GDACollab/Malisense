using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public EventInstance currentPlaying;
    protected PLAYBACK_STATE playbackState;

    // Test
    public Vector2 screamDistance = new Vector2(10, 50);

    // Regions here to collapse code
    #region VOLUME CONTROL
    [Header("Volumes (sliders)")]
    [Range(0f, 1f)]
    public float masterVolume = 0.5f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.5f;
    [Range(0f, 1f)]
    public float ambienceVolume = 0.5f;

    [Header("Bus Paths")]
    public string masterBusPath = "bus:/";
    public string musicBusPath = "bus:/Music";
    public string sfxBusPath = "bus:/SFX";
    public string ambienceBusPath = "bus:/Ambience";

    //BUSES
    public Bus masterBus;
    public Bus musicBus;
    public Bus sfxBus;
    public Bus ambienceBus;
    #endregion

    #region OST PATHS
    [Header("FMOD OST")]
    [Tooltip("FMOD Event Path for the menu music")]
    public string menuOST = "event:/VS_Malisense/Menu";
    #endregion

    #region SFX PATHS
    [Header("FMOD DUNGEON(SFX) Event Path Strings")]
    public string monsterScream = "event:/VS_Malisense/Monster_Scream";
    [Header("FMOD UI(SFX) Event Path Strings")]
    public string selectUI = "event:/SFX/UI & Menu/UI Select";
    public string sliderUI = "event:/SFX/UI & Menu/UI Slider Feedback";
    #endregion

    #region AMBIENCE PATHS
    [Tooltip("FMOD Event Path for the dungeon ambience")]
    public string dungeonOST = "event:/VS_Malisense/Dun_Ambience";
    #endregion

    void Awake()
    {
        masterBus = RuntimeManager.GetBus(masterBusPath);
        musicBus = RuntimeManager.GetBus(musicBusPath);
        sfxBus = RuntimeManager.GetBus(sfxBusPath);
        ambienceBus = RuntimeManager.GetBus(ambienceBusPath);
    }

    // Update is called once per frame
    void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        ambienceBus.setVolume(ambienceVolume);
    }

    /// <summary>
    /// Play any sfx or other sound that isn't music.
    /// </summary>
    /// <param name="path">The path to the sound.</param>
    public void Play(string path)
    {
        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out _);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogWarning("FMOD event path does not exist: " + path);
            return;
        }

        EventInstance instance = RuntimeManager.CreateInstance(path);
        instance.start();
        instance.release();
    }

    /// <summary>
    /// Play a song from the OST.
    /// </summary>
    /// <param name="path">The path to the song.</param>
    public void PlayOST(string path)
    {
        Debug.Log("[Audio Manager] Playing Song: " + path);
        if (currentPlaying.isValid())
        {
            Debug.Log("Current playing is valid");
            StartCoroutine(RestOfPlayOST(path));
        }
        else
        {
            FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out _);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogWarning("[Audio Manager] FMOD SONG event path does not exist: " + path);
                return;
            }

            EventInstance song = RuntimeManager.CreateInstance(path);
            currentPlaying = song;
            song.start();
            song.release();
        }
    }
    public IEnumerator RestOfPlayOST(string path)
    {
        Debug.Log("Stopping" + currentPlaying + " (rest of Play OST)");
        currentPlaying.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentPlaying.getPlaybackState(out playbackState);
        while (playbackState != PLAYBACK_STATE.STOPPED)
        {
            currentPlaying.getPlaybackState(out playbackState);
            yield return null;
        }

        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out _);
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

    /// <summary>
    /// Stops the song currently playing
    /// </summary>
    public void StopCurrentSong()
    {
        StartCoroutine(StopCurrentSongRoutine());
    }

    public IEnumerator StopCurrentSongRoutine()
    {
        Debug.Log("Stopping" + currentPlaying + " (rest of Play OST)");
        currentPlaying.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentPlaying.getPlaybackState(out playbackState);
        while (playbackState != PLAYBACK_STATE.STOPPED)
        {
            currentPlaying.getPlaybackState(out playbackState);
            yield return null;
        }
    }

    /// <summary>
    /// Plays a monster scream
    /// </summary>
    public void PlayScream()
    {
        Play(monsterScream);
    }

    /// <summary>
    /// Plays a scream in the given emitter
    /// </summary>
    /// <param name="emitter">The emitter to play the scream at</param>
    public void PlayScream(StudioEventEmitter emitter)
    {
        emitter.EventReference = RuntimeManager.PathToEventReference(monsterScream);
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }

    /// <summary>
    /// Plays the dungeon ost
    /// </summary>
    public void DungeonOST()
    {
        PlayOST(dungeonOST);
    }

    /// <summary>
    /// Plays the select sfx
    /// </summary>
    public void PlayUISelect()
    {
        Play(selectUI);
    }

    /// <summary>
    /// Plays the slide sfx
    /// </summary>
    public void PlayUISlider()
    {
        Play(sliderUI);
    }
}
