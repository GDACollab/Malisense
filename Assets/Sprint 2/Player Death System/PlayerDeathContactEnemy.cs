using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathContactEnemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player died due to contact to enemy");
        }
    }
}