using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // ### START TEMP VARIABLES ### DELETE BEFORE FINAL BUILD
    [Header("Temp Variables (Remove before Final Build)")]
    public GameObject controlMessage;
    public GameObject[] enemies;
    // ### END TEMP VARIABLES ###
    
    // Movement
    [Header("Movement")]
    [SerializeField] float walkingSpeed = 6;
    [SerializeField] float sprintRatio = 2;
    [SerializeField] float sneakRatio = 1;
    float adjustedSpeed; // the speed the player moves at, accounting for sprinting or sneaking
    
    // Stamina
    [Header("Stamina")]
    [SerializeField] float maxStamina = 15;
    [Tooltip("n stamina per second")][SerializeField] float staminaRegen = 2;
    [Tooltip("n stamina per second")][SerializeField] float staminaDepletion = 4;
    [Tooltip("percentage of stamina required to sprint again after becoming exhausted")][Range(0.00f, 1.00f)][SerializeField] float minimumToSprint = 0.25f;
    float currentStamina;
    
    // Inventory
    [Header("Inventory")]
    [Tooltip("Percentage of speed reduction while carrying an object")][SerializeField] float objectSlowdownRatio = 0.4f;
    [HideInInspector] public InventoryBase newInventory;

    // Maybe Seperate UI Script?
    // UI Elements
    [Header("UI Elements")]
    public Image StaminaBar;
    
    // Public States
    // Movement States
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isSneaking;
    [HideInInspector] public bool canMove = true;
    // Stamina States
    [HideInInspector] public bool isExhausted = false; // makes it so player can't run; true when stamina is 0, false when currentStamina >= minimumToSprint
    bool isReading = false;
    
    // Input System
    private PlayerInput playerInput;
    private InputAction moveAction, sprintAction, sneakAction, hideMessage, setEnemies, interactAction, setDownAction;
    // Rigid Body
    private Rigidbody2D rb;
    private Transform interactBody;
    private InteractionSelector interactArea;
    private Animator playerAnimation;
    private float animationXScale;
    private float animationXPos;
    
    void Start()
    {
        // Set input system variables
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
        sprintAction = playerInput.actions.FindAction("Sprint");
        sneakAction = playerInput.actions.FindAction("Sneak");
        hideMessage = playerInput.actions.FindAction("Hide Message");
        setEnemies = playerInput.actions.FindAction("Set Enemies");
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
        
        playerAnimation = GetComponentInChildren<Animator>();
        animationXScale = playerAnimation.transform.localScale.x;
        animationXPos = playerAnimation.transform.localPosition.x;
        
        // Get the enemy to deactivate
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
        if(canMove){
            StaminaManager();
            InventoryManager();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Death Check
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player died due to contact to enemy");
            Loader.Load(Loader.Scene.DeathScene);
        }
    }
    
    // Temporary Input Options DELETE BEFORE FINAL BUILD
    private void TempInputOptions(){
        if(hideMessage.triggered){
            controlMessage.SetActive(!controlMessage.activeSelf);
        }
        // Deactivate/Reactivate Enemies
        if(setEnemies.triggered){
            foreach(var enemy in enemies){
                enemy.SetActive(!enemy.activeSelf);
            }
        }
    }
    
    private void BasicMovement(){
        if(canMove){
            // For moving and rotating
            // Movement
            float moveX = moveAction.ReadValue<Vector2>().x;
            float moveY = moveAction.ReadValue<Vector2>().y;
            Vector2 direction = new Vector2(moveX, moveY).normalized;
            rb.velocity = direction * adjustedSpeed;
            float flip = animationXScale;
            float flop = animationXPos;
            if(moveX<0){
                flip = animationXScale;
                flop = animationXPos;
                playerAnimation.transform.localScale = new Vector3(flip,playerAnimation.transform.localScale.y,playerAnimation.transform.localScale.z);
                playerAnimation.transform.localPosition = new Vector3(flop, playerAnimation.transform.localPosition.y,playerAnimation.transform.localPosition.z);
            } 
            else if(moveX>0){
                flip = -animationXScale;
                flop = -animationXPos;
                playerAnimation.transform.localScale = new Vector3(flip,playerAnimation.transform.localScale.y,playerAnimation.transform.localScale.z);
                playerAnimation.transform.localPosition = new Vector3(flop, playerAnimation.transform.localPosition.y,playerAnimation.transform.localPosition.z);
            }
            
            

            // Set isMoving
            if (moveX != 0 || moveY != 0){
                isMoving = true;
                playerAnimation.Play("Base Layer.New Animation", 0);
            }
            else{
                isMoving = false;
                // playerAnimation.StopPlayback();
                playerAnimation.Play("Base Layer.Idle Animation", 0);
            }

            // Rotation
            if (direction.magnitude > 0){
                interactBody.up = direction;
            }
        }
        else{
            rb.velocity = Vector2.zero;
        }
    }
    
    private void InputManager(){
        // For sprinting and sneaking
        // Walking (default)
        adjustedSpeed = walkingSpeed;

        // Sprinting
        if ((sprintAction.ReadValue<float>() > 0f) && (isMoving == true) && (!isExhausted))
        {
            isSprinting = true; 
            adjustedSpeed *= sprintRatio;   // Adjust Speed
            currentStamina -= staminaDepletion * Time.deltaTime;        // deplete stamina
            if (currentStamina < 0f){                                   // check if player should now be exhausted
                isExhausted = true;
            }
        }
        else
        {
            isSprinting = false;
            if (currentStamina < maxStamina){                            // regen stamina
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
        if(interactAction.triggered){
            InteractionManager();
        }
        
        // Slow player when carrying an object 
        if (newInventory.carriedObject != null)
        {
            adjustedSpeed *= objectSlowdownRatio;
        }
    }
    
    private void StaminaManager(){
        // Make sure currentStamina doesn't go over maxStamina
        if (currentStamina > maxStamina){
            currentStamina = maxStamina;
        }

        // Check if player shouldn't be exhausted anymore
        if (currentStamina > maxStamina * minimumToSprint){
            isExhausted = false;
        }
        
        // Update the stamina bar
        if (StaminaBar != null)
        {
            StaminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
    
    private void InventoryManager(){
        if (!newInventory.carriedObject)
        {
            return;
        }
        
        if (setDownAction.ReadValue<float>() > 0f)
        {
            if (newInventory.carriedObject)
            {
                newInventory.carriedObject.transform.parent = null;
                newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                newInventory.carriedObject = null;
            }
        }

        if (newInventory.carriedObject)
        {
            newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            newInventory.carriedObject.transform.parent = interactBody.transform.parent;
            newInventory.carriedObject.transform.position = interactBody.transform.position;
            newInventory.carriedObject.transform.rotation = interactBody.transform.rotation;
        }
    }
    
    private void InteractionManager(){
        if(interactArea.isInteractable){
            
            var other = interactArea.other;
            var item = other.GetComponent<ItemPickup>();
            var heavyItem = other.GetComponent<HeavyItem>();
            var door = other.GetComponent<Door>();
            var doorSwitch = other.GetComponent<SwitchController>();
            var note = other.GetComponent<FloorNote>();

            if(isReading){
                note.DeactivateNote();
                isReading = false;
                canMove = true;
            }
            else if (door && newInventory.carriedObject)
            {
                Destroy(newInventory.carriedObject.gameObject);
                newInventory.carriedObject = null;
                Destroy(other.gameObject);
            }
            else if (newInventory.carriedObject)
            {
                newInventory.carriedObject.transform.parent = null;
                newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                newInventory.carriedObject = null;
            }
            else if(note){
                canMove = false;
                isReading = true;
                note.ActivateNote();
            }
            else if (doorSwitch)
            {
                doorSwitch.ActivateSwitch();
            }
            else if (heavyItem)
            {
                newInventory.carriedObject = heavyItem;
            }
            else if (item)
            {
                bool success = newInventory.AddItem(item.item, 1);
                if (success)
                {
                    Destroy(other.gameObject);
                }
            }
        }
        else if (newInventory.carriedObject)
        {
            newInventory.carriedObject.transform.parent = null;
            newInventory.carriedObject.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            newInventory.carriedObject = null;
        }
    }
    
    // EDIT REGION #######################################################################
    
}