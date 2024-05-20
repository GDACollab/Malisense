using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicHandScript : MonoBehaviour
{
    public Artifact MagicHand;

    PlayerInput playerInput;
    InputAction moveAction, interactAction;

    SpriteRenderer sprite;
    public Color hoverTint;

    Collider2D currentHover;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        playerInput = player.GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");

        interactAction = playerInput.actions.FindAction("Interact");

        if (MagicHand.cooldown > 0.0f) {
            player.GetComponent<Player>().canMove = true;
            Destroy(gameObject);
            return;
        }
        else
        {
            player.GetComponent<Player>().canMove = false;
        }

        if(MagicHand.duration == 1)
        {
            Destroy(gameObject);
            return;
        }

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmartCamera>().SetTarget(gameObject);
        MagicHand.duration = 1;

        currentHover = null;

        // Disable player movement

        if (player == null) Debug.LogError("No player found in scene, Magic hand script");

       

        // Sync movement
    }

    private void Update()
    {
        // Check for movement
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;

        Vector2 direction = new Vector2(moveX, moveY).normalized;

        transform.Translate(direction * Time.deltaTime * 10f);

        if (interactAction.triggered && currentHover != null)
        {
            if (currentHover.GetComponent<StateMachine>()) // If Enemy
            {
                currentHover.GetComponent<StateMachine>().currentState = StateMachine.State.Distracted;

                end(60f);
            }
            else if (currentHover.GetComponent<SwitchController>()) // If Switch
            {
                currentHover.GetComponent<SwitchController>().ActivateSwitch();
            }
            else if (currentHover.GetComponent<ItemPickup>()) // If Item
            {
                ItemPickup x = currentHover.GetComponent<ItemPickup>();
                bool success = player.GetComponent<Player>().newInventory.AddItem(x.item, 1);
                if (success)
                {
                    Destroy(x.gameObject);
                }
            }
        }

        if (interactAction.triggered && currentHover == null)
        {
            end(5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        StopAllCoroutines();

        if (collision.GetComponent<StateMachine>()) // If Enemy
        {
            currentHover = collision;
            sprite.color = hoverTint;
        } else if (collision.GetComponent<SwitchController>()) // If Switch
        {
            currentHover = collision;
            sprite.color = hoverTint;
        } else if (collision.GetComponent<ItemPickup>()) // If Item
        {
            currentHover = collision;
            sprite.color = hoverTint;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine("Grace");
        }
    }

    IEnumerator Grace()
    {
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        currentHover = null;
    }

    private void end(float cooldown)
    {
        MagicHand.duration = 0;
        player.GetComponent<Player>().canMove = true;
        MagicHand.cooldown = cooldown;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmartCamera>().SetTarget(player);
        Destroy(gameObject);
    }



    /*//Magic hand can be controlled to move around ala the player, so it needs to spawn a new object that you control instead of the player for a short period

    //This script is the one run when the artifact is used, and spawns (and despawns) the hand that the player controls

    public float MagicHandCooldown;
    public float MagicHandDuration;
    public GameObject controllableHand;
    public Artifact MagicHand;

    GameObject spawnedObject;
    GameObject player;
    //float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("in the hand");
        //below line (just the one) literally copied and pasted from WhisperingBellArtifact.cs
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
        if (MagicHand.duration > 0.0f || MagicHand.cooldown > 0.0f)
        {
            if (MagicHand.duration > 0.0f)
            {
                //Get rid of the prev hand
                GameObject prevHand = GameObject.FindGameObjectWithTag("Magic Hand");
                Destroy(prevHand);
                MagicHand.cooldown = MagicHandCooldown * (1 - (MagicHand.duration / MagicHandDuration)); //cooldown inversely proportional to how long hand has been out
                MagicHand.duration = 0.0f;
            }
            player.GetComponent<Player>().canMove = true;
            Destroy(gameObject);
        }

    }

    //(modified from whispering bell artifact code for consistency)
    // When using editor, clears all previous runtime values of the object
    private void OnValidate()
    {
        MagicHand.duration = 0.0f;
        MagicHand.cooldown = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (MagicHand.duration == 0 && MagicHand.cooldown == 0) //first frame of existing
        {
            //Debug.Log("starting");
            stealPlayerInput();
            MagicHand.duration = MagicHandDuration;
        }
        else if (MagicHand.duration > 0)
        {
            //Debug.Log("decr duration");
            *//*
            timer += Time.deltaTime;
            if(timer > MagicHandDuration)
            {
                timer = 0;
                returnPlayerInput();
            }
            *//*
            MagicHand.duration -= Time.deltaTime;
            if (MagicHand.duration <= 0)
            {
                MagicHand.duration = 0;
                returnPlayerInput();
            }
        }
        else if (MagicHand.cooldown >= 0)
        {
            //Debug.Log("decr cooldown");
            MagicHand.cooldown -= Time.deltaTime;
            if (MagicHand.cooldown <= 0)
            {
                MagicHand.cooldown = 0;
                Destroy(gameObject);
            }
        }
        //Debug.Log("why is cooldown not saying it's >= 0");
    }

    void stealPlayerInput()
    {
        Debug.Log("Stealing Player input");
        player.GetComponent<Player>().canMove = false;
        spawnedObject = Instantiate(controllableHand, player.transform.position, Quaternion.identity);
    }

    void returnPlayerInput()
    {
        Debug.Log("Returning Player input");
        player.GetComponent<Player>().canMove = true;
        MagicHand.cooldown = MagicHandCooldown;
        Destroy(spawnedObject);
        //Destroy(gameObject);
    }*/
}