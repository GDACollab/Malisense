using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Linq;
using UnityEngine.SceneManagement;
using Buildings = VillageNavigationManager.Buildings;

public class AudioManager : MonoBehaviour
{
    private enum MusicTrack
    {
        MENU = 0,
        VILLAGE = 1,
        CRYPT = 2,
        DUNGEON = 3,
        CHASE = 4,
        ENDING = 5,
        STOP = 6
    }
    
    
    public EventInstance currentPlaying;
    public EventInstance movementSFX;
    MusicTrack currentTrack = MusicTrack.MENU;
    protected PLAYBACK_STATE playbackState;
    protected PLAYBACK_STATE movementState;

    // Scream Distance
    // Scream Distance
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
    public float movementsfxVolume = 0.3f;
    [Range(0f, 1f)]
    public float ambienceVolume = 0.5f;

    // [Header("Bus Paths")]
    private string masterBusPath = "bus:/";
    private string musicBusPath = "bus:/Music";
    private string sfxBusPath = "bus:/SFX";
    private string ambienceBusPath = "bus:/Ambience";

    //BUSES
    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    private Bus ambienceBus;
    #endregion

    #region OST PATHS
    // [Header("FMOD OST")]
    // [Header("FMOD OST")]
    [Tooltip("FMOD Event Path for the menu music")]
    private string masterOST = "event:/OST/Game Scene OST";
    private string menuOST = "event:/OST/Menu";
    private string villageOST = "event:/OST/Village";
    private string cryptOST = "event:/OST/Crypt";
    private string dungeonOST = "event:/OST/Dungeon";
    private string chaseOST = "event:/OST/Chase";
    private string endingOST = "event:/OST/Ending";
    #endregion

    #region SFX PATHS
    // Dungeon SFX Paths
    // [Header("FMOD DUNGEON(SFX) Event Path Strings")]
    private string objectMoveSFX = "event:/Game SFX/Dungeon/ObjectMove";
    private string pickupSFX = "event:/Game SFX/Dungeon/Pickup";
    private string switchSFX = "event:/Game SFX/Dungeon/SwitchPull";
    
    // Item SFX
    private string bellSFX = "event:/Game SFX/Items/Artifacts/Bell";
    private string dynamiteBoomSFX = "event:/Game SFX/Items/Consumables/Dynamite/Boom";
    private string dynamiteLightSFX = "event:/Game SFX/Items/Consumables/Dynamite/Light";
    private string elixirSFX = "event:/Game SFX/Items/Consumables/Elixir";
    private string flashDustSFX = "event:/Game SFX/Items/Consumables/FlashLantern";
    
    // Monster SFX
    private string scentAlertSFX = "event:/Game SFX/Monster/Scent/Alert";
    private string scentIdleSFX = "event:/Game SFX/Monster/Scent/Idle";
    private string sightAlertSFX = "event:/Game SFX/Monster/Sight/Alert";
    private string sightIdleSFX = "event:/Game SFX/Monster/Sight/Idle";
    private string soundAlertSFX = "event:/Game SFX/Monster/Sound/Alert";
    private string soundIdleSFX = "event:/Game SFX/Monster/Sound/Idle";
    private string fakeMonsterSFX = "event:/Game SFX/Monster/FakeMonster";
    private string monsterScream = "event:/VS_Malisense/Monster_Scream";
    
    // Player SFX Paths
    private string playerScream = "event:/Game SFX/Player/Die";
    private string puddleSplash = "event:/Game SFX/Player/PuddleSplish";
    private string reviveSFX = "event:/Game SFX/Player/Revive";
    private string lowStaminaSFX = "event:/Game SFX/Player/StaminaLow";
    private string playerStepSFX = "event:/Game SFX/Player/Walk";
    
    // Village SFX Paths
    private string villageBark = "event:/Game SFX/Village/VillagerBark";
    
    // UI SFX Paths
    // [Header("FMOD UI(SFX) Event Path Strings")]
    private string hoverUISFX = "event:/UI SFX/Menu Hover SFX (4)";
    private string selectUISFX = "event:/UI SFX/Menu_button_select_with_bell_and_tail";
    #endregion

    // #region AMBIENCE PATHS
    // [Tooltip("FMOD Event Path for the dungeon ambience")]
    // public string dungeonOST = "event:/OST/Dungeon";
    // #endregion

    // #region AMBIENCE PATHS
    // [Tooltip("FMOD Event Path for the dungeon ambience")]
    // public string dungeonOST = "event:/OST/Dungeon";
    // #endregion

    void Awake()
    {
        masterBus = RuntimeManager.GetBus(masterBusPath);
        musicBus = RuntimeManager.GetBus(musicBusPath);
        sfxBus = RuntimeManager.GetBus(sfxBusPath);
        ambienceBus = RuntimeManager.GetBus(ambienceBusPath);
        SceneManager.sceneUnloaded += (_) => StopCurrentSong();
        SceneManager.sceneUnloaded += (_) => StopCurrentSong();
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
    /// Plays an SFX setting a given parameter to a given value
    /// </summary>
    /// <param name="path">The path to the sound</param>
    /// <param name="parameter">The the name of the parameter</param>
    /// <param name="value">The value to set the parameter at</param>
    public void PlayModified(string path, string parameter, float value){
        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out _);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogWarning("FMOD event path does not exist: " + path);
            return;
        }

        EventInstance instance = RuntimeManager.CreateInstance(path);
        instance.setParameterByName(parameter, value);
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
            SwitchSong(path);
            
            // StartCoroutine(RestOfPlayOST(path));
        }
        else
        {
            FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out _);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogWarning("[Audio Manager] FMOD SONG event path does not exist: " + path);
                return;
            }

            EventInstance song = RuntimeManager.CreateInstance(masterOST);
            currentPlaying = song;
            SwitchSong(path);
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
    
    private void SwitchSong(string path){
        string newMusic = path.Split("/").Last().ToLower();
        MusicTrack track = MusicTrack.MENU;
        switch (newMusic)
        {
            case "menu":
                track = MusicTrack.MENU;
                break;
            case "village":
                track = MusicTrack.VILLAGE;
                break;
            case "crypt":
                track = MusicTrack.CRYPT;
                break;
            case "dungeon":
                track = MusicTrack.DUNGEON;
                break;
            case "chase":
                track = MusicTrack.CHASE;
                break;
            case "ending":
                track = MusicTrack.ENDING;
                break;
            default:
                track = MusicTrack.ENDING;
                break;
        }
        
        currentPlaying.setParameterByName("GAME SCENE", (float)track);
        currentTrack = track;
        Debug.Log("Played: "+track);
    }
    
    /// <summary>
    /// Stops the song currently playing
    /// </summary>
    public void StopCurrentSong()
    {
        MusicTrack track = MusicTrack.STOP;
        currentPlaying.setParameterByName("GAME SCENE", (float)track);
        currentTrack = track;
        // StartCoroutine(StopCurrentSongRoutine());
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

    #region OST Functions
    public void PlayMenuOST(){
        PlayOST(menuOST);
    }
    
    public void PlayVillageOST(){
        PlayOST(villageOST);
    }
    
    public void PlayCryptKeeperOST(){
        PlayOST(cryptOST);
    }
    
    /// <summary>
    /// Transitions between Village OST and Shop OST
    /// </summary>
    /// <param name="play">Whether to play or stop the OST</param>
    /// <param name="transitionTime">The transition time</param>
    public void SetShopOST(bool play, float transitionTime){
        if(currentTrack == MusicTrack.VILLAGE){
            StartCoroutine(TransitionToShopOST(play, transitionTime));
        }
    }
    
    private IEnumerator TransitionToShopOST(bool play, float transitionTime){
        float start = play ? 0 : 1;
        float goal = play ? 1 : 0;
        float tTime = 0;
        
        while(tTime<transitionTime){
            tTime += Time.deltaTime;
            float transition = Mathf.Lerp(start, goal, tTime/transitionTime);
            currentPlaying.setParameterByName("SHOP", transition);
            yield return null;
        }
    }
    
    /// <summary>
    /// Plays the dungeon ost
    /// </summary>
    public void PlayDungeonOST()
    {
        PlayOST(dungeonOST);
    }
    
    /// <summary>
    /// Plays the chase ost
    /// </summary>
    public void PlayChaseOST(){
        PlayOST(chaseOST);
    }
    
    public void PlayEndingOST(){
        PlayOST(endingOST);
    }
    #endregion

    #region SFX Functions
    /// <summary>
    /// Plays a villager bark given a villager
    /// </summary>
    /// <param name="villager"></param>
    public void PlayVillageBark(Buildings villager){
        float value;
        switch(villager){
            case Buildings.SCHOLAR:
                value = 0;
                break;
            case Buildings.CHURCH:
                value = 1;
                break;
            case Buildings.MAYOR:
                value = 2;
                break;
            case Buildings.CRYPT_KEEPER:
                value = 3;
                break;
            default:
                return;
        }
        PlayModified(villageBark, "CurrentVillager", value);
    }
    
    public void PlayObjectMoveSFX(){
        Play(objectMoveSFX);
    }
    
    public void PlayPickupSFX(){
        Play(pickupSFX);
    }
    
    public void PlaySwitchSFX(){
        Play(switchSFX);
    }
    
    public void PlayBellSFX(){
        Play(bellSFX);
    }
    
    public void PlayDynamiteBoomSFX(){
        Play(dynamiteBoomSFX);
    }
    
    public void PlayDynamiteLightSFX(){
        Play(dynamiteLightSFX);
    }
    
    public void PlayElixirSFX(){
        Play(elixirSFX);
    }
    
    public void PlayFlashDustSFX(){
        Play(flashDustSFX);
    }
    
    public void PlayScentAlertSFX(){
        Play(scentAlertSFX);
    }
    public void PlayScentAlertSFX(StudioEventEmitter emitter)
    {
        emitter.EventReference = RuntimeManager.PathToEventReference(scentAlertSFX);
        emitter.ResetEvent();
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }
    public void PlayScentIdleSFX(){
        Play(scentIdleSFX);
    }
    public void PlayScentIdleSFX(StudioEventEmitter emitter)
    {
        emitter.EventReference = RuntimeManager.PathToEventReference(scentIdleSFX);
        emitter.ResetEvent();
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }
    
    public void PlaySightAlertSFX(){
        Play(sightAlertSFX);
    }
    
    //The above function doesn't work; call the below one instead!
    public void PlaySightAlertSFX(StudioEventEmitter emitter)
    {
        emitter.EventReference = RuntimeManager.PathToEventReference(sightAlertSFX);
        emitter.ResetEvent();
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }
    //Error with this one
    public void PlaySightIdleSFX(){
        Play(sightIdleSFX);
    }
    //End of Potential Error 

    //The above function doesn't work; call the below one instead!
    public void PlaySightIdleSFX(StudioEventEmitter emitter)
    {
        emitter.EventReference = RuntimeManager.PathToEventReference(sightIdleSFX);
        emitter.ResetEvent();
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }

    // public void PlaySoundAlertSFX(){
    //     Play(soundAlertSFX);
    // }
    public void PlaySoundAlertSFX(StudioEventEmitter emitter)
    {
        emitter.EventReference = RuntimeManager.PathToEventReference(soundAlertSFX);
        emitter.ResetEvent();
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }

    public void PlaySoundAlertSFX(){
        Play(soundAlertSFX);
    }
    
    // public void PlaySoundIdleSFX(){
    //     Play(soundIdleSFX);
    // }
    public void PlaySoundIdleSFX(StudioEventEmitter emitter)
    {
        emitter.EventReference = RuntimeManager.PathToEventReference(soundIdleSFX);
        emitter.ResetEvent();
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }
    
    public void PlayFakeMonsterSFX(){
        Play(fakeMonsterSFX);
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
        emitter.ResetEvent();
        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = screamDistance.x;
        emitter.OverrideMaxDistance = screamDistance.y;
        emitter.Play();
    }
    
    public void PlayDeathSFX(){
        Play(playerScream);
    }
    
    public void PlayPuddleSFX(){
        Play(puddleSplash);
    }
    
    public void PlayReviveSFX(){
        Play(reviveSFX);
    }
    private IEnumerator FadeOut()
    { 
        movementSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        movementSFX.getPlaybackState(out movementState);
        while (movementState != PLAYBACK_STATE.STOPPED)
        {
            movementSFX.getPlaybackState(out movementState);
            yield return null;
        }
    }

    public void PlayLowStaminaSFX(){
        if (movementSFX.isValid())
        {
            StartCoroutine(FadeOut());
        }
        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(lowStaminaSFX, out _);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogWarning("FMOD event path does not exist: " + lowStaminaSFX);
            return;
        }

        movementSFX = RuntimeManager.CreateInstance(lowStaminaSFX);
        movementSFX.setVolume(sfxVolume);
        movementSFX.start();
        
    }
    
    public void PlayStepSFX(float running=0.0f){
        if (movementSFX.isValid())
        {
            StartCoroutine(FadeOut());
        }
        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(playerStepSFX, out _);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogWarning("FMOD event path does not exist: " + playerStepSFX);
            return;
        }
        movementSFX = RuntimeManager.CreateInstance(playerStepSFX);
        movementSFX.setParameterByName("running", running);
        movementSFX.setVolume(movementsfxVolume);
        movementSFX.start();
    }

    public void StopStepSound(bool immediate)
    {
        if (movementSFX.isValid() && !immediate)
        {
            StartCoroutine(FadeOut());
        }else if (movementSFX.isValid())
        {
            movementSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            movementSFX.release();
        }
    }
    
    
    /// <summary>
    /// Plays the select sfx
    /// </summary>
    public void PlayUISelectSFX()
    {
        Play(selectUISFX);
        Play(selectUISFX);
    }

    /// <summary>
    /// Plays the slide sfx
    /// </summary>
    public void PlayUIHoverSFX()
    {
        Play(hoverUISFX);
        Play(hoverUISFX);
    }
    #endregion
}
