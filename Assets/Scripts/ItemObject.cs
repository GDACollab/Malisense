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

    private void Update()
    {
        
    }
}
