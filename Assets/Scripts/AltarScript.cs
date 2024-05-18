using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltarScript : MonoBehaviour
{
    GameObject playerObj;

    private Collider2D safeCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        safeCollider = GetComponent<Collider2D>();
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
