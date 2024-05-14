using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScentDetection : MonoBehaviour
{
    GameObject player;

    private StateMachine stateMac;

    // when either of these thresholds are met, it changes the state
    [Header("Distance Thresholds")]
    public float distT_Patrol2Alert = 20;
    public float distT_Alert2Chase = 10;

    [Header("Scent Thresholds")]
    public float scentT_Patrol2Alert = 50;
    public float scentT_Alert2Chase = 90;

    [Header("Scent Values")]

    public float scentPerSecond = 1;

    [Header("Read-only")]

    // player scent should always be between 0 and 100
    [SerializeField]
    private float playerScent = 0;
    [SerializeField]
    private float playerDist = 0;
    // Start is called before the first frame update
    void Start()
    {
        stateMac = GetComponent<StateMachine>();
        player = GameObject.FindWithTag("Player");
        playerDist = Vector2.Distance(player.transform.position, transform.position);

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
        if (playerScent < 100) modScent(scentPerSecond * Time.fixedDeltaTime);
    }

    // checks if monster is close enough to player to smell them
    IEnumerator sniffCheck() {

        while (true) {
            checkPlayer();
            switch (GetComponent<StateMachine>().currentState) {
                case StateMachine.State.Patrolling:
                    // if crossed threshold switch state
                    if (playerScent > scentT_Patrol2Alert || playerDist < distT_Patrol2Alert) {
                        stateMac.currentState = StateMachine.State.Alert;
                        Debug.Log("Scent Beast switching from Patrol -> Alert");
                    }
                    break;
                case StateMachine.State.Alert:
                    // if crossed threshold switch state
                    if (playerScent < scentT_Patrol2Alert && playerDist > distT_Patrol2Alert) {
                        stateMac.currentState = StateMachine.State.Patrolling;
                        Debug.Log("Scent Beast switching from Alert -> Patrol");
                    } else if (playerScent > scentT_Alert2Chase || playerDist < distT_Alert2Chase) {
                        stateMac.currentState = StateMachine.State.Chasing;
                        Debug.Log("Scent Beast switching from Alert -> Chase");
                    }
                    break;
                case StateMachine.State.Chasing:
                    // if crossed threshold switch state
                    // Chasing doesn't end untill scent is off the player
                    if (playerScent < scentT_Alert2Chase) {
                        //modScent(10); // jump up playerscent to increase difficulty
                        stateMac.currentState = StateMachine.State.Alert;
                        Debug.Log("Scent Beast switching from Chase -> Alert");
                    }
                    break;
            }
            // checks scent/dist for threshold corssings            
            yield return new WaitForSeconds(0.25f);
            
        }
    }
}