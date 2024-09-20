using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControls : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
                                   // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
    

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    public Transform holsterPosition; //Position where the holstered object will be held
    private GameObject heldObject; // Reference to the currently held object
    private GameObject holsterObject = null; // Reference to currently holstered object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    [SerializeField] private bool holdingObject = false;
    private bool holdingGun = false;
    private bool holdingFlashlight = false;
    [SerializeField] private bool objectInHolster = false;
    private GameObject heldFlashlight;

    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1f; //make short
    public float standingHeight = 2f; //make normal
    public float crouchSpeed = 1.5f; //short speed
    public bool isCrouching = false; //if short or normal

    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color

    public SpriteMask spriteMask;

    //Battery Stuff

    public GameObject Battery;
    public batteryManager batteryManager;
   
    //purple upgrade stuff
    public bool hasPurpleUpgrade = false;
    public GameObject purpleUpgrade;

    //red upgrade stuff
    public bool hasRedUpgrade = false;
    public GameObject redUpgrade;

    //hostage stuff
    public GameObject hostage;
    public Hostages hostages;

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed
        
        // Subscribe to the Flashlight input event
        playerInput.Player.FlashlightSwitch.performed += ctx => FlashlightSwitch(); // Call the FlashlightSwitch method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the Crouch method when crouch input is performed

        // Subscribe to the crouch input event
        playerInput.Player.HolsterandSwitchheld.performed += ctx => HolsterOrSwitchObject(); // Call the Crouch method when crouch input is performed

        // Subscribe to the interact input event
        playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch

    }
      
    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        float currentSpeed;
        if(isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }
    
    public void FlashlightSwitch()
    {
        if (holdingFlashlight == true)
        {
            Light heldFlashlightLight = heldFlashlight.GetComponent<Light>();
            if (heldFlashlightLight.enabled)
            {
                heldFlashlightLight.enabled = false;
                spriteMask.enabled = false;
            }
            else
            {
                heldFlashlightLight.enabled = true;
                spriteMask.enabled = true;

                //spriteMask.transform.position = heldFlashlight.transform.position;
                
                /*RaycastHit UVtorchHit;

                if(Physics.Raycast(heldFlashlightLight.transform.position, heldFlashlightLight.transform.forward, out UVtorchHit, pickUpRange))
                {
                    if (UVtorchHit.collider.CompareTag("blood"))
                    {
                        UVtorchHit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }

                }*/




            }
        }
    }
    

    public void HolsterOrSwitchObject()
    {
        //nothing in holster something in hand
        // holster what is in hand > nothing in hand
        if (!objectInHolster && holdingObject)
        {
            // Holster the object
            holsterObject = heldObject;
            holsterObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
            
            heldObject = null;

            // Attach the object to the holster position
            holsterObject.transform.position = holsterPosition.position;
            holsterObject.transform.rotation = holsterPosition.rotation;
            holsterObject.transform.parent = holsterPosition;

            holdingGun = false;
            holdingFlashlight = false;
            
            objectInHolster = true;
            holdingObject = false;
        }
        else
        //nothing held something in holster
        //put holster object in hand
        if (!holdingObject && objectInHolster)
        {
            heldObject = holsterObject;
            heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
            holsterObject = null;
                    
            heldObject.transform.position = holdPosition.position;
            heldObject.transform.rotation = holdPosition.rotation;
            heldObject.transform.parent = holdPosition;
                
            if (heldObject.CompareTag("Gun"))
            {
                holdingGun = true;
                holdingFlashlight = false;
            }
                
            if (heldObject.CompareTag("Flashlight"))
            {
                holdingFlashlight = true;
                holdingGun = false;
            }

            holdingObject = true;
            objectInHolster = false;

        }
        else
        //something held something in holster
        //swap the two
        if (objectInHolster && holdingObject)
        {
           
                var temp = heldObject;
                heldObject = holsterObject;
                holsterObject = temp;

                holsterObject.transform.position = holsterPosition.position;
                holsterObject.transform.rotation = holsterPosition.rotation;
                holsterObject.transform.parent = holsterPosition;

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                if (holdingGun)
                {
                    holdingGun = false;
                    holdingFlashlight = true;
                }
                else if (holdingFlashlight)
                {
                    holdingFlashlight = false;
                    holdingGun = true;
                }
            
        }
    }
    
    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            return;
        }
        
        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                holdingObject = true;
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                holdingObject = true;
                holdingGun = true;
            }
            else if (hit.collider.CompareTag("Flashlight"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                holdingObject = true;
                heldFlashlight = heldObject;
                
                holdingFlashlight = true;
            }
        }
    }

    public void Interact()
    {
        // Perform a raycast to detect the lightswitch
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Switch")) // Assuming the switch has this tag
            {
                // Change the material color of the objects in the array
                foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color; // Set the color to match the switch material color
                    }
                }
            }

            else if (hit.collider.CompareTag("Door")) // Check if the object is a door
            {
                // Start moving the door upwards
                StartCoroutine(RaiseDoor(hit.collider.gameObject));
            }

            else if (hit.collider.CompareTag("Key"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private IEnumerator RaiseDoor(GameObject door)
    {
        float raiseAmount = 5f; // The total distance the door will be raised
        float raiseSpeed = 2f; // The speed at which the door will be raised
        Vector3 startPosition = door.transform.position; // Store the initial position of the door
        Vector3 endPosition = startPosition + Vector3.up * raiseAmount; // Calculate the final position of the door after raising

        // Continue raising the door until it reaches the target height
        while (door.transform.position.y < endPosition.y)
        {
            // Move the door towards the target position at the specified speed
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, raiseSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }
    }

    public void ToggleCrouch()
    {
        if(isCrouching)
        {
            //stand up
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
           characterController.height = crouchHeight;
            isCrouching = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Battery")
        {
            Battery = other.gameObject;
            Debug.Log("added 1 to battery level");
            Destroy(Battery);
            batteryManager.addBatteryLevel();
        }

       

        if(other.tag == "PurpleUpgrade")
        {
            Debug.Log("got the purple upgrade");
            Destroy(purpleUpgrade);
            hasPurpleUpgrade = true;
        }

        if (other.tag == "RedUpgrade")
        {
            Debug.Log("got the red upgrade");
            Destroy(redUpgrade);
            hasRedUpgrade = true;
        }

        if(other.tag == "Hostage")
        {
            hostage = other.gameObject;
            Debug.Log("saved a hostage!");
            Destroy(hostage);
            hostages.addHostageNumber();
        }

        
    }
}


