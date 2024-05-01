using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltarScript : MonoBehaviour
{
    // Remove once alert goes to patrol automatically
    [Header("Time")]
    [SerializeField][Tooltip("Time after entering altar until enemies return to patrol")] private float alertCooldown = 5f;
    
    GameObject[] enemyObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")){
            foreach(GameObject eObj in enemyObjects){
                StateMachine enemy = eObj.GetComponent<StateMachine>();
                if(enemy.currentState==StateMachine.State.Chasing){ 
                    enemy.currentState = StateMachine.State.Alert;
                }
            }
        }
    }
}
