using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScentDetection : MonoBehaviour
{
    GameObject player;

    public float patrolThreshold = 0;
    public float alertThreshold = 0;
    public float chaseThreshold = 0;

    private float playerScent = 0;
    private float playerDist = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerDist = Vector2.Distance(player.transform.position, transform.position);
        if (!player.GetComponent<Stinker>()) {
            Debug.Log("ERROR: Player does not have a Stinker script for scent beast to read");
        }
        playerScent = player.GetComponent<Stinker>().getStink();

        StartCoroutine("sniffCheck");

    }

    private void checkPlayer() {
        playerScent = player.GetComponent<Stinker>().getStink();
        playerDist  = Vector2.Distance(player.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // checks if monster is close enough to player to smell them
    IEnumerator sniffCheck() {

        while (true) {
            checkPlayer();
            switch (GetComponent<StateMachine>().currentState) {
                case StateMachine.State.Patrolling:
                    // if crossed threshold switch state
                    break;
                case StateMachine.State.Alert:
                    // if crossed threshold switch state
                    break;
                case StateMachine.State.Chasing:
                    // if crossed threshold switch state
                    break;
            }
            // checks scent/dist for threshold corssings            
            yield return new WaitForSeconds(0.25f);
            
        }
    }
}
