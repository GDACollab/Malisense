using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private GameObject player;
    private AudioSource speaker;
    private Rigidbody2D rb;
    
    private int lifespan;
    [SerializeField] 
    bool isThrowable;
    
    [SerializeField, Tooltip("How far the object will get thrown")]
    float throwStrength = 100;

    [SerializeField] 
    EffectData effectData;
    
    [SerializeField]
    float effectRadius = 50;
    
    private void Start()
    {
        // ref to player, always good to have
        player = GameObject.FindGameObjectWithTag("Player");
        
        speaker = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        // idk if yall want this 
        if (isThrowable)
        {
            rb.velocity = player.transform.up * throwStrength;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If object can receive effects
        var effectable = other.GetComponent<IEffectable>();
        Debug.Log($"Hit {other.gameObject.name}");

        if (effectable != null)
        {
            // just an example :)
            Effect temp = new Effects.CloudBootsEffect(effectData);
            effectable.AddEffect(temp);
        }
        Destroy(this.gameObject);
    }
}
