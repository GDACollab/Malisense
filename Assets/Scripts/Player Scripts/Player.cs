using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // ### START TEMP VARIABLES ### DELETE BEFORE FINAL BUILD
    [Header("Temp Variables (Remove before Final Build)")]
    public GameObject controlMessage;
    public GameObject[] enemies;
    public UnityEvent testFunctions;
    // ### END TEMP VARIABLES ###

    // Movement
    [Header("Movement")]
    [Tooltip("The player's walking speed")][SerializeField] float walkingSpeed = 6;
    [Tooltip("Speed while sprinting. Walking speed x n")][SerializeField] float sprintRatio = 2;
    [Tooltip("Speed while sneaking. Walking speed x n")][SerializeField] float sneakRatio = 1;
    float adjustedSpeed; // the speed the player moves at, accounting for sprinting or sneaking

    // Stamina
    [Header("Stamina")]
    [Tooltip("Amount of stamina the player has")][SerializeField] float maxStamina = 15;
    [Tooltip("Regen n stamina per second while not sprinting")][SerializeField] float staminaRegen = 2;
    [Tooltip("Deplete n stamina per second while sprinting")][SerializeField] float staminaDepletion = 4;
    [Tooltip("percentage of stamina required to sprint again after becoming exhausted")][Range(0.00f, 1.00f)][SerializeField] float minimumToSprint = 0.25f;
    float currentStamina;

    // Inventory
    [Header("Inventory")]
    [Tooltip("Percentage of speed reduction while carrying an object")][SerializeField] float objectSlowdownRatio = 0.4f;
    [HideInInspector] public InventoryBase newInventory;
    
    // Sound 
    [Header("Sound")]
    [SerializeField] float NoiseFrequency = 0.2f;
    [SerializeField] float walkLoudness = 3f;
    [SerializeField] float sprintLoudness = 12f;
    //public float sneakLoudness;
    private float timeCheck = 0;
    private scr_noise noiseSystem;
    
    // Maybe Seperate UI Script?
    // UI Elements
    [Header("UI Elements")]
    [Tooltip("UI image for the stamina bar")] public Image StaminaBar;

    // Public States
    // Movement States
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isSneaking;
    [HideInInspector] public bool canMove = true;
    // Stamina States
    [HideInInspector] public bool isExhausted = false; // makes it so player can't run; true when stamina is 0, false when currentStamina >= minimumToSprint
    bool isReading = false;
    
    // Global Teapot
    private GlobalTeapot globalTeapot;
    // Player Sprite
    private SpriteRenderer playerSprite;
    // Input System
    private PlayerInput playerInput;
    private InputAction moveAction, sprintAction, sneakAction, interactAction, setDownAction;
    private InputAction hideMessage, setEnemies, hideFootsteps, activateFunctions; // Test inputs remove before final build please
    // Rigid Body and interaction variables
    private Rigidbody2D rb;
    private Transform interactBody;
    private InteractionSelector interactArea;

    void Start()
    {
        // Set input system variables
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
        sprintAction = playerInput.actions.FindAction("Sprint");
        sneakAction = playerInput.actions.FindAction("Sneak");
        interactAction = playerInput.actions.FindAction("Interact");
        setDownAction = playerInput.actions.FindAction("SetDown");

        // Get Rotating Body for Interactions
        interactBody = transform.GetChild(0);
        interactArea = interactBody.GetComponent<InteractionSelector>();

        // Set rigid body variables
        rb = GetComponent<Rigidbody2D>();

        // Set initial stamina
        currentStamina = maxStamina;

        // Get inventory
        newInventory = GetComponent<InventoryBase>();
        
        // Get sound object
        noiseSystem = GetComponent<scr_noise>();
        
        // Get player sprite
        playerSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        
        // Get Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        
        // Get temp input options
        hideMessage = playerInput.actions.FindAction("Hide Message");
        setEnemies = playerInput.actions.FindAction("Set Enemies");
        hideFootsteps = playerInput.actions.FindAction("Hide Footsteps");
        activateFunctions = playerInput.actions.FindAction("Activate Functions");
        
        // Get the enemies to deactivate
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void FixedUpdate()
    {
        BasicMovement();
    }

    void Update()
    {
        TempInputOptions(); // Temporary Input Options DELETE BEFORE FINAL BUILD
        InputManager();
        if (canMove)
        {
            StaminaManager();
            InventoryManager();
            SoundManager();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Death Check
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player died due to contact to enemy");
            globalTeapot.villageInk = 2;
            Loader.Load(Loader.Scene.DeathScene);
        }
    }

    // Temporary Input Options DELETE BEFORE FINAL BUILD
    private void TempInputOptions()
    {
        // Hide text containing controls
        if (hideMessage.triggered)
        {
            controlMessage.SetActive(!controlMessage.activeSelf);
        }
        // Deactivate/Reactivate Enemies
        if (setEnemies.triggered)
        {
            foreach (var enemy in enemies)
            {
                enemy.SetActive(!enemy.activeSelf);
            }
        }
        if (hideFootsteps.triggered)
        {
            noiseSystem.noiseObject.GetComponent<SpriteRenderer>().enabled = !noiseSystem.noiseObject.GetComponent<SpriteRenderer>().enabled;
        }
        if(activateFunctions.triggered){
            if (testFunctions.GetPersistentEventCount()>0){
                testFunctions.Invoke();
            }
        }
    }

    private void BasicMovement()
    {
        if (canMove)
        {
            // For moving and rotating
            // Movement
            float moveX = moveAction.ReadValue<Vector2>().x;
            float moveY = moveAction.ReadValue<Vector2>().y;
            Vector2 direction = new Vector2(moveX, moveY).normalized;
            rb.velocity = direction * adjustedSpeed;

            // Set isMoving
            if (moveX != 0 || moveY != 0)
                isMoving = true;
            else
                isMoving = false;
            
            // Flip Sprite
            if(moveX > 0){
                playerSprite.flipX = true;
            }
            else if(moveX < 0){
                playerSprite.flipX = false;
            }
            
            // Rotation
            if (direction.magnitude > 0)
                interactBody.up = direction;
        }
        else
        {
            rb.velocity = Vector2.zero; // Zero out movement
        }
    }

    private void InputManager()
    {
        // For sprinting and sneaking
        // Walking (default)
        adjustedSpeed = walkingSpeed;

        // Sprinting
        if ((sprintAction.ReadValue<float>() > 0f) && (isMoving == true) && (!isExhausted))
        {
            isSprinting = true;
            adjustedSpeed *= sprintRatio;   // Adjust Speed
            currentStamina -= staminaDepletion * Time.deltaTime;        // deplete stamina
            if (currentStamina < 0f)
            {                                   // check if player should now be exhausted
                isExhausted = true;
            }
        }
        else
        {
            isSprinting = false;
            if (currentStamina < maxStamina)
            {                            // regen stamina
                currentStamina += staminaRegen * Time.deltaTime;
            }
        }

        // Sneaking
        // if (sneakAction.ReadValue<float>() > 0f && !isSprinting)
        // {
        // 	adjustedSpeed *= sneakRatio;
        // 	isSneaking = true;
        // }
        // else
        // {
        // 	isSneaking = false;
        // }

        // Interaction
        if (interactAction.triggered)
        {
            InteractionManager();
        }

        // Slow player when carrying an object 
        if (newInventory.carriedObject != null)
        {
            adjustedSpeed *= objectSlowdownRatio;
        }
    }

    private void StaminaManager()
    {
        // Make sure currentStamina doesn't go over maxStamina
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        // Check if player shouldn't be exhausted anymore
        if (currentStamina > maxStamina * minimumToSprint)
        {
            isExhausted = false;
        }

        // Update the stamina bar
        if (StaminaBar != null)
        {
            StaminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    private void InventoryManager()
    {
        // Check if the player is carrying an object
        if (!newInventory.carriedObject)
        {
            return;
        }
        else
        {
            newInventory.carriedObject.transform.rotation = interactBody.transform.rotation;
        }

        // if (setDownAction.ReadValue<float>() > 0f)
        // {
        //     if (newInventory.carriedObject)
        //     {
        //         newInventory.carriedObject.transform.parent = null;
        //         newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        //         newInventory.carriedObject = null;
        //     }
        // }
    }

    private void InteractionManager()
    {
        // get list of all interactable objects, in order of priority
        List<GameObject> interactions = interactArea.getInteractables();
        if (interactions == null) {
            Debug.Log("no interactions found");
            interactArea.removeInteracts();
            return;
        }
        
        // Put down carried object
        if (!interactArea.isInteractable() && newInventory.carriedObject)
        {
            newInventory.carriedObject.transform.parent = null;
            newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            newInventory.carriedObject = null;
            interactArea.removeInteracts();
            return;
        }
        
        // run through each object in the list until we find the highest priority interaction we can do
        foreach (GameObject other in interactions) {
            

            // Find what object of item it is
            var item = other.GetComponent<ItemPickup>();
            var heavyItem = other.GetComponent<HeavyItem>();
            var door = other.GetComponent<Door>();
            var doorSwitch = other.GetComponent<SwitchController>();
            var note = other.GetComponent<FloorNote>();

            // INTERACTIONS ELSE IF LIST
            // NOTE - all entries must begin by calling interactArea.removeInteracts() and break at the end

            // Stop reading
            if (isReading)
            {
                // if we are reading, we want to make sure that we can deactivate the note, 
                // the if statement makes sure that while reading we loop through until we find the note
                if (note == null) continue;

                interactArea.removeInteracts();
                note.DeactivateNote();
                isReading = false;
                canMove = true;
                break;
            }
            // Open door
            else if (door && newInventory.carriedObject)
            {
                interactArea.removeInteracts();
                Destroy(newInventory.carriedObject.gameObject);
                newInventory.carriedObject = null;
                Destroy(door.gameObject);
                break;
            }
            // Put down carried object
            else if (newInventory.carriedObject)
            {
                interactArea.removeInteracts();
                newInventory.carriedObject.transform.parent = null;
                newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                newInventory.carriedObject = null;
                break;
            }
            // Read note
            else if (note)
            {
                interactArea.removeInteracts();
                if(note.name == "End Artifact"){
                    globalTeapot.villageInk = 3;
                    Loader.Load(Loader.Scene.Village);
                }
                canMove = false;
                isReading = true;
                note.ActivateNote();
                break;
            }
            // Activate switch
            else if (doorSwitch)
            {
                interactArea.removeInteracts();
                doorSwitch.ActivateSwitch();
                break;
            }
            // Pick up heavy item
            else if (heavyItem)
            {
                interactArea.removeInteracts();
                newInventory.carriedObject = heavyItem;
                newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                newInventory.carriedObject.transform.parent = interactBody.transform.parent;
                newInventory.carriedObject.transform.position = interactBody.transform.position;
                break;
            }
            // Pick up regular item
            else if (item)
            {
                interactArea.removeInteracts();
                bool success = newInventory.AddItem(item.item, 1);
                if (success)
                {
                    Destroy(item.gameObject);
                }
                break;
            }
        }
        interactArea.removeInteracts();
        
    }
    
    private void SoundManager(){
        if (isMoving) //When the player is moving
        {
            timeCheck += Time.deltaTime; //increment time
            while (timeCheck >= NoiseFrequency) //When enough time has passed
            {                                   //(and repetition if there has been too much lag)
                float size = walkLoudness; // Decide size of noiceObject based on current state
                if (isSprinting) { size = sprintLoudness; }
                //else if(player.GetComponent<PlayerControl>().isSneaking) { size = sneakLoudness; }
                noiseSystem.MakeSound(transform.position,size); //Send command to create sound object
                timeCheck -= NoiseFrequency; //decrement timeCheck to prevent infinite loop!
            }
        }
        else //When not moving
        {
            //Make sure time is increased such that when the player moves again, they instantly make sound
            timeCheck = Mathf.Min(NoiseFrequency, timeCheck += Time.deltaTime);
        }
    }
}