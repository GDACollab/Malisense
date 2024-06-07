using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
using UnityEngine;

public class ScentDetection : MonoBehaviour
{
    Player player;

    private StateMachine stateMac;

    // when either of these thresholds are met, it changes the state
    [Header("Distance Thresholds")]
    public float distT_Patrol2Alert = 20;
    public float distT_Alert2Chase = 10;

    [Header("Scent Thresholds")]
    public float scentT_Patrol2Alert = 50;
    public float scentT_Alert2Chase = 90;

    [Header("Scent Buildup Values")]
    public float scentPerSecond;
    static public float scentPerSecond_Patrol = 0.15f;
    static public float scentPerSecond_Alert = 0.35f;
    static public float scentPerSecond_Chase = 2;
    
    [Header("Behavior Modifiers")]
    public bool usedIncense = false;
    [SerializeField] private float incenseDecrease = 40;
    public float incenseDuration = 3;
    [SerializeField] private float safeZoneDecrease = 0.75f;
    [Tooltip("The minimum scent value reached through modification")]
    [SerializeField] private float minScentValue = 30;
    private float incenseTime;
    private bool inSafeZone => player.activeSafeZones.Count > 0;

    [Header("Audio")]
    private AudioManager audioManager;
    
    [Range(0, 600)]
    [Tooltip("minimum range for delay between idle audio sounds (in seconds)")]
    public float idleAudioDelayMin;
    [Range(0, 600)]
    [Tooltip("maximum range for delay between idle audio sounds (in seconds)")]
    public float idleAudioDelayMax;
    
    [Range(0, 600)]
    [Tooltip("minimum range for delay between idle audio sounds (in seconds)")]
    public float alertAudioDelayMin;
    [Range(0, 600)]
    [Tooltip("maximum range for delay between idle audio sounds (in seconds)")]
    public float alertAudioDelayMax;
    
    [Range(0, 600)]
    [Tooltip("minimum range for delay between idle audio sounds (in seconds)")]
    public float chaseAudioDelayMin;
    [Range(0, 600)]
    [Tooltip("maximum range for delay between idle audio sounds (in seconds)")]
    public float chaseAudioDelayMax;

    private StudioEventEmitter soundEmitter;

    [Header("Read-only")]

    // player scent should always be between 0 and 100
    [SerializeField]
    private float playerScent = 0;
    [SerializeField]
    private float playerDist = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (idleAudioDelayMax < idleAudioDelayMin) UnityEngine.Debug.LogWarning("ScentDetection: Start(): Idle Audio Delay values are incorrect");
        soundEmitter = GetComponent<StudioEventEmitter>();
        scentPerSecond = scentPerSecond_Patrol;
        stateMac = GetComponent<StateMachine>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerDist = Vector2.Distance(player.transform.position, transform.position);
        incenseTime = 0;
        audioManager = GameObject.FindWithTag("Global Teapot").GetComponent<AudioManager>();

        StartCoroutine("sniffCheck");

    }

    // updates the players current position values
    private void checkPlayer() {
        //playerScent = player.GetComponent<Stinker>().getStink();
        playerDist  = Vector2.Distance(player.transform.position, transform.position);
    }

    // changes the stink value by mod_stink as long as it doesn't cross max/min stink
    public void modScent(float amount) {
        playerScent = playerScent + amount;
        if (playerScent < 0) playerScent = 0;
        else if (playerScent > 100) playerScent = 100;
    }

    void FixedUpdate()
    {
        if(usedIncense){
            UseIncense();
        }
        else if (inSafeZone){
            if(playerScent > minScentValue){
                playerScent -= Time.deltaTime * safeZoneDecrease;
            }
        }
        else if (playerScent < 100 && !(stateMac.currentState == StateMachine.State.Distracted)) {
            modScent(scentPerSecond * Time.fixedDeltaTime);
            incenseTime = 0;
        }
    }
    
    void UseIncense(){
        if(incenseTime < incenseDuration){
            if(playerScent > minScentValue){
                playerScent -= Time.deltaTime * incenseDecrease/incenseDuration;
            }
            incenseTime += Time.deltaTime;
        }
        else {
            usedIncense = false;
        }
    }

    // checks if monster is close enough to player to smell them
    IEnumerator sniffCheck() {
        float idleDelay = Random.Range(idleAudioDelayMin,idleAudioDelayMax);
        float idleTimer = Time.time;
        float alertDelay = Random.Range(alertAudioDelayMin,alertAudioDelayMax);
        float alertTimer = Time.time;
        float chaseDelay = Random.Range(chaseAudioDelayMin,chaseAudioDelayMax);
        float chaseTimer = Time.time;
        while (true) {
            checkPlayer();
            switch (GetComponent<StateMachine>().currentState) {
                case StateMachine.State.Patrolling:
                    idleDelay -= Time.time - idleTimer;
                    idleTimer = Time.time;
                    UnityEngine.Debug.Log("delay = " + idleDelay);
                    if (idleDelay <= 0) {
                        idleDelay = Random.Range(idleAudioDelayMin,idleAudioDelayMax);
                        audioManager.PlayScentIdleSFX(soundEmitter);
                    }
                    if (scentPerSecond != scentPerSecond_Patrol) scentPerSecond = scentPerSecond_Patrol;
                    // if crossed threshold switch state
                    if (playerScent > scentT_Patrol2Alert || playerDist < distT_Patrol2Alert) {
                        stateMac.currentState = StateMachine.State.Alert;
                        audioManager.PlayScentAlertSFX(soundEmitter);
                        UnityEngine.Debug.Log("Scent Beast switching from Patrol -> Alert");
                    }
                    break;
                case StateMachine.State.Alert:
                    alertDelay -= Time.time - alertTimer;
                    alertTimer = Time.time;
                    UnityEngine.Debug.Log("delay = " + alertDelay);
                    if (alertDelay <= 0) {
                        alertDelay = Random.Range(alertAudioDelayMin,alertAudioDelayMax);
                        audioManager.PlayScentAlertSFX(soundEmitter);
                    }
                    if (scentPerSecond != scentPerSecond_Alert) scentPerSecond = scentPerSecond_Alert;
                    // if crossed threshold switch state
                    if (playerScent < scentT_Patrol2Alert && playerDist > distT_Patrol2Alert) {
                        stateMac.currentState = StateMachine.State.Patrolling;
                        UnityEngine.Debug.Log("Scent Beast switching from Alert -> Patrol");
                    } else if (playerScent > scentT_Alert2Chase || playerDist < distT_Alert2Chase) {
                        stateMac.currentState = StateMachine.State.Chasing;
                        UnityEngine.Debug.Log("Scent Beast switching from Alert -> Chase");
                    }
                    break;
                case StateMachine.State.Chasing:
                    chaseDelay -= Time.time - chaseTimer;
                    chaseTimer = Time.time;
                    UnityEngine.Debug.Log("delay = " + chaseDelay);
                    if (chaseDelay <= 0) {
                        chaseDelay = Random.Range(chaseAudioDelayMin,chaseAudioDelayMax);
                        audioManager.PlayScentAlertSFX(soundEmitter);
                    }
                    if (scentPerSecond != scentPerSecond_Chase) scentPerSecond = scentPerSecond_Chase;
                    // if crossed threshold switch state
                    // Chasing doesn't end untill scent is off the player
                    if (playerScent < scentT_Alert2Chase && playerDist > distT_Alert2Chase) {
                        stateMac.currentState = StateMachine.State.Alert;
                        UnityEngine.Debug.Log("Scent Beast switching from Chase -> Alert");
                    }
                    break;
            }
            // checks scent/dist for threshold corssings            
            yield return new WaitForSeconds(0.25f);
        }
    }
    
    /// <summary>
    /// Gets the player's scent value
    /// </summary>
    /// <returns>The player's scent value</returns>
    public float GetScent(){
        return playerScent;
    }
}
