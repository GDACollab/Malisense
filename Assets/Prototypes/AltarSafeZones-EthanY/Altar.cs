using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Altar : MonoBehaviour
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
                StateMachine_Updated enemy = eObj.GetComponent<StateMachine_Updated>();
                // Remove if alert when alert goes to patrol automatically
                if(enemy.currentState==StateMachine_Updated.State.Chasing || enemy.currentState==StateMachine_Updated.State.Alert){ 
                    // enemy.switchState(StateMachine_Updated.State.Alert); 
                    // Replace with just setting to alert when alert goes to patrol automatically
                    StartCoroutine(CooldownEnemy(enemy)); 
                }
            }
        }
    }
    
    // Remove once alert goes to patrol automatically
    private IEnumerator CooldownEnemy(StateMachine_Updated enemy){
        enemy.switchState(StateMachine_Updated.State.Alert);
        
        yield return new WaitForSeconds(alertCooldown);
        
        enemy.switchState(StateMachine_Updated.State.Patrolling);
    }
}
