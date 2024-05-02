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
    GameObject playerObj;

    private Collider2D safeCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        safeCollider = GetComponent<Collider2D>();
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
        Player playerScript = playerObj.GetComponent<Player>();
        // Need to check intersection this way as collision between Safe Zone and Player has been disabled
        if (safeCollider.bounds.Intersects(playerObj.GetComponent<Collider2D>().bounds))
        {
                
            if (!playerScript.activeSafeZones.Contains(this.gameObject))
            {
                playerScript.activeSafeZones.Add(this.gameObject);
            }
        } else
        {
            playerScript.activeSafeZones.Remove(this.gameObject);
        }
    }
}
