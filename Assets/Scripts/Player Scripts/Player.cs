using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region TEMP VARS REMOVE
    // ### START TEMP VARIABLES ### DELETE BEFORE FINAL BUILD
    [Header("Temp Variables (Remove before Final Build)")]
    public GameObject controlMessage;
    public GameObject[] enemies;
    public UnityEvent testFunctions;
    // ### END TEMP VARIABLES ###
    #endregion

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

    // Animation
    [Header("Animations")]
    // Player Animator
    [SerializeField] Animator playerAnimator;
    // Sprite Scale
    private Vector3 rigScale;


    // Inventory
    [Header("Inventory")]
    [Tooltip("Percentage of speed reduction while carrying an object")][SerializeField] float objectSlowdownRatio = 0.4f;
    [HideInInspector] public OLD_INVENTORY inventory;
    [SerializeField] public PlayerInventory newInventory;

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
    // Dungeon Manager
    private DungeonManager dungeonManager;
    // Audio Manager
    private AudioManager audioManager;
    // Input System
    private PlayerInput playerInput;
    private InputAction moveAction, sprintAction, sneakAction, interactAction, setDownAction;
    private InputAction hideMessage, setEnemies, hideFootsteps, activateFunctions; // Test inputs remove before final build please
    // Rigid Body and interaction variables
    private Rigidbody2D rb;
    // Interaction Area
    [Header("Interaction Area")]
    [SerializeField] private Transform interactBody;
    private InteractionSelector interactArea;

    public List<GameObject> activeSafeZones = new List<GameObject>();

    public List<Artifact> Artifacts = new List<Artifact>();

    private enum movementSFXState
    {
        WALKING,
        RUNNING,
        EXHAUSTED,
        STOPALL
    };
    private movementSFXState movementSFX; // Indicates what movement sfx is being played
    private bool stopSFX = false; // Indicates that we need to stop current movement sfx

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
        interactArea = interactBody.GetComponent<InteractionSelector>();

        // Set rigid body variables
        rb = GetComponent<Rigidbody2D>();

        // Set initial stamina
        currentStamina = maxStamina;

        // Get inventory
        newInventory = Resources.Load<PlayerInventory>("Player_Inventory");

        // Get sound object
        noiseSystem = GetComponent<scr_noise>();

        // Get scale of player rig
        rigScale = playerAnimator.gameObject.transform.localScale;

        // Get Global Teapot
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        // Get Dungeon Manager
        dungeonManager = FindObjectOfType<DungeonManager>();
        // Get Audio Manager
        audioManager = globalTeapot.audioManager;
        #region TEMP INPUTS
        // Get temp input options
        hideMessage = playerInput.actions.FindAction("Hide Message");
        setEnemies = playerInput.actions.FindAction("Set Enemies");
        hideFootsteps = playerInput.actions.FindAction("Hide Footsteps");
        activateFunctions = playerInput.actions.FindAction("Activate Functions");
        #endregion

        // Get the enemies to deactivate
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void FixedUpdate()
    {
        BasicMovement();

        foreach(Artifact x in Artifacts)
        {
            if (x.cooldown != 0)
            {
                x.cooldown = Mathf.Max(0, x.cooldown - Time.deltaTime);
            }
        }

        // For all Artifacts decreate cooldowns
    }

    void Update()
    {
        #region TEMP INPUT UPDATE
        TempInputOptions(); // Temporary Input Options DELETE BEFORE FINAL BUILD
        #endregion
        InputManager();
        if (canMove)
        {
            StaminaManager();
            InventoryManager();
            SoundManager();
            SFXManager();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Death Check
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player died due to contact to enemy");
            newInventory.inventory.Clear();
            playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            playerAnimator.SetTrigger("die");
            StartCoroutine(ZoomCamera());
            dungeonManager.KillPlayer();
        }
    }
    
    IEnumerator ZoomCamera(){
        var camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        while (camera.orthographicSize > 5){
            camera.orthographicSize -= Time.unscaledDeltaTime * 2;
            yield return null;
        }
    }

    #region TEMP INPUT FUNC
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
        if (activateFunctions.triggered)
        {
            if (testFunctions.GetPersistentEventCount() > 0)
            {
                testFunctions.Invoke();
            }
        }
    }

    public void ToggleCheats(int stamdepl)
    {
        var collider = GetComponent<Collider2D>();
        collider.enabled = !collider.enabled;
        staminaDepletion = collider.enabled ? stamdepl : 0;
    }
    #endregion

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
            {
                isMoving = true;
                playerAnimator.SetBool("moving",true);
            }
            else
            {
                isMoving = false;
                playerAnimator.SetBool("moving", false);
            }
                

            // Flip Sprite
            if (moveX > 0)
            {
                playerAnimator.gameObject.transform.localScale = new Vector3(-rigScale.x, rigScale.y, rigScale.z);
            }
            else if (moveX < 0)
            {
                playerAnimator.gameObject.transform.localScale = new Vector3(rigScale.x, rigScale.y, rigScale.z);
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
            if(!stopSFX && movementSFX != movementSFXState.RUNNING)
            {
                movementSFX = movementSFXState.RUNNING;
                stopSFX = true;
            }
            playerAnimator.SetBool("sprinting", true);
            adjustedSpeed *= sprintRatio;   // Adjust Speed
            currentStamina -= staminaDepletion * Time.deltaTime;        // deplete stamina
            if (currentStamina < 0f)
            {                                   // check if player should now be exhausted
                isExhausted = true;
                stopSFX = true;
                movementSFX = movementSFXState.EXHAUSTED;
                
                StaminaBar.GetComponent<Animator>().SetBool("isExhausted", isExhausted);
            }
        }
        else
        {
            isSprinting = false;
            playerAnimator.SetBool("sprinting", false);
            if(canMove && !stopSFX && movementSFX != movementSFXState.WALKING && isMoving && !isExhausted) // If walking sfx is not playing already and the player is walking, play it
            {
                movementSFX = movementSFXState.WALKING;
                stopSFX = true;
            }
            else if(!stopSFX && !isMoving && movementSFX != movementSFXState.STOPALL && !isExhausted) // If player has stopped and music hasn't been stopped, stop it 
            {
                movementSFX = movementSFXState.STOPALL;
                stopSFX = true;
            }
            if (currentStamina < maxStamina)
            {                            // regen stamina
                currentStamina += staminaRegen * Time.deltaTime;
                if (!stopSFX && !isExhausted && movementSFX == movementSFXState.EXHAUSTED) // Player is no longer exhausted so switch to appropriate sfx
                {
                    stopSFX = true;
                    if (!isMoving || !canMove) // Player isn't moving
                        movementSFX = movementSFXState.STOPALL;
                    else if(canMove)// Player is moving
                        movementSFX = movementSFXState.WALKING;
                }
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
            // TEST DEATH SCRIPT playerAnimator.SetTrigger("die");
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
            StaminaBar.GetComponent<Animator>().SetBool("isExhausted", isExhausted);
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
        // if (!newInventory.carriedObject)
        // {
        //     return;
        // }
        // else
        // {
        //     newInventory.carriedObject.transform.rotation = interactBody.transform.rotation;
        // }

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


        // Stop reading
        if (isReading)
        {   if(!stopSFX && movementSFX != movementSFXState.STOPALL)
            {
                stopSFX = true;
                movementSFX = movementSFXState.STOPALL;
            }
            interactArea.removeInteracts();
            dungeonManager.DeactivateNote();
            isReading = false;
            canMove = true;
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

        if (interactions == null)
        {
            Debug.Log("no interactions found");
            interactArea.removeInteracts();
            return;
        }

        // run through each object in the list until we find the highest priority interaction we can do
        foreach (GameObject other in interactions)
        {


            // Find what object of item it is
            var item = other.GetComponent<ItemPickup>();
            var heavyItem = other.GetComponent<HeavyItem>();
            var door = other.GetComponent<Door>();
            var doorSwitch = other.GetComponent<SwitchController>();
            var note = other.GetComponent<FloorNote>();

            // INTERACTIONS ELSE IF LIST
            // NOTE - all entries must begin by calling interactArea.removeInteracts() and break at the end

            // Open door
            if (door && newInventory.carriedObject)
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
                if(!stopSFX && movementSFX != movementSFXState.STOPALL)
                {
                    stopSFX = true;
                    movementSFX = movementSFXState.STOPALL;
                    SFXManager();
                }
                interactArea.removeInteracts();
                canMove = false;
                isReading = true;
                dungeonManager.ActivateNote(note);
                break;
            }
            // Activate switch
            else if (doorSwitch)
            {
                Debug.Log("activating switch");
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

    private void SFXManager()
    {
        if(stopSFX && movementSFX == movementSFXState.WALKING ) // Play Walk SFX
        {
            stopSFX = false;
            audioManager.PlayStepSFX(1.0f);
            Debug.Log("Switching to Walk SFX");
        }
        else if(stopSFX && movementSFX == movementSFXState.RUNNING) // Play Sprint SFX
        {
            stopSFX = false;
            audioManager.PlayStepSFX(0.0f);
            Debug.Log("Switching to Running SFX");
        }
        else if(stopSFX && movementSFX == movementSFXState.EXHAUSTED) // Play Exhausted SFX
        {
            stopSFX = false;
            audioManager.PlayLowStaminaSFX();
            Debug.Log("Switching to Exhausted SFX");
        }
        else if(stopSFX && movementSFX == movementSFXState.STOPALL) // Player is not moving so stop all SFX
        {
            stopSFX = false;
            audioManager.StopStepSound(false);
            Debug.Log("Stopping all SFX");
        }   
    }

    private void SoundManager()
    {
        if (isMoving) //When the player is moving
        {
            timeCheck += Time.deltaTime; //increment time
            while (timeCheck >= NoiseFrequency) //When enough time has passed
            {                                   //(and repetition if there has been too much lag)
                float size = walkLoudness; // Decide size of noiceObject based on current state
                if (isSprinting) { size = sprintLoudness; }
                //else if(player.GetComponent<PlayerControl>().isSneaking) { size = sneakLoudness; }
                noiseSystem.MakeSound(transform.position, size); //Send command to create sound object
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