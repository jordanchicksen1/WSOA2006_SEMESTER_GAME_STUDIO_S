using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    private Vector2 _moveInput; // Stores the movement input from the player
    private Vector2 _lookInput; // Stores the look input from the player
    private float _verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 _velocity; // Velocity of the player
    private CharacterController _characterController; // Reference to the CharacterController component

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
    

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    public Transform holsterPosition; //Position where the holstered object will be held
    private GameObject _heldObject; // Reference to the currently held object
    private GameObject _holsterObject = null; // Reference to currently holstered object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    [SerializeField] private bool holdingObject = false;
    [SerializeField] private bool objectInHolster = false;
    
    //GUN and FLASHLIGHT
    
    private bool _holdingGun = false;
    private bool _holdingFlashlight = false;
    private GameObject _heldFlashlight;
    public GameObject spriteMask;

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

    
    //Battery Stuff
    public batteryManager batteryManager;

    //Key stuff
    public keyManager keyManager;
   
    //purple upgrade stuff
    public bool hasPurpleUpgrade = false;
    public GameObject purpleUpgrade;

    //red upgrade stuff
    public bool hasRedUpgrade = false;
    public GameObject redUpgrade;

    //hostage stuff
    public GameObject hostage;
    public Hostages hostages;

    //key text
    public GameObject gotKey;

    //battery text
    public GameObject gotBattery;

    //ui text stuff
    public GameObject holdingGunText;
    public GameObject holdingFlashlightText;

    //sound effects general
    public AudioSource worldSounds;
    public AudioClip keySFX;
    public AudioClip batterySFX;
    public AudioClip flashlightSFX;
    public AudioClip doorSFX;
    public AudioClip stungunSFX;
    public AudioClip evidenceSFX;

    //pick up text
    public GameObject pickupText;

    //evidence related stuff
    public bool gotEvidence1 = false;
    public bool gotEvidence2 = false;
    public GameObject collectedEvidence;
    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        _characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => _moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => _lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => _lookInput = Vector2.zero; // Reset lookInput when look input is canceled

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
       // playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch

    }
      
    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }

    private void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        var currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

        // Move the character controller based on the movement vector and speed
        _characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    private void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        var lookX = _lookInput.x * lookSpeed;
        var lookY = _lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, lookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        _verticalLookRotation -= lookY;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(_verticalLookRotation, 0, 0);
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -0.5f; // Small value to keep the player grounded
        }

        _velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        _characterController.Move(_velocity * Time.deltaTime); // Apply the velocity to the character
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            // Calculate the jump velocity
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Shoot()
    {
        if (_holdingGun != true) return;
        // Instantiate the projectile at the fire point
        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        worldSounds.clip = stungunSFX;
        worldSounds.Play();

        // Get the Rigidbody component of the projectile and set its velocity
        var rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * projectileSpeed;

        // Destroy the projectile after 3 seconds
        Destroy(projectile, 0.5f);
    }

    private void FlashlightSwitch()
    {

        var heldFlashlightLight = _heldFlashlight.GetComponent<Light>();
        
     
        if (heldFlashlightLight.enabled)
        {
            heldFlashlightLight.enabled = false; 
            spriteMask.SetActive(false);
            
        }
        else
        {
            if (_holdingFlashlight != true || !(batteryManager.batteryLevel > 0.99)) 
            {
                return; 
            }
            heldFlashlightLight.enabled = true;
            batteryManager.decreaseBatteryLevel();
            spriteMask.SetActive(true);
            worldSounds.clip = flashlightSFX;
            worldSounds.Play();
        }
    }

    //nothing in holster something in hand
    // holster what is in hand > nothing in hand
    private void Holster()
    {
        // Holster the object
        _holsterObject = _heldObject;
        _holsterObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
            
        _heldObject = null;

        // Attach the object to the holster position
        _holsterObject.transform.position = holsterPosition.position;
        _holsterObject.transform.rotation = holsterPosition.rotation;
        _holsterObject.transform.parent = holsterPosition;

        _holdingGun = false;
        _holdingFlashlight = false;
            
        objectInHolster = true;
        holdingObject = false;
    }

    //nothing held something in holster
    //put holster object in hand
    private void UnHolster()
    {
        _heldObject = _holsterObject;
        _heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
        _holsterObject = null;
                    
        _heldObject.transform.position = holdPosition.position;
        _heldObject.transform.rotation = holdPosition.rotation;
        _heldObject.transform.parent = holdPosition;
                
        if (_heldObject.CompareTag("Gun"))
        {
            _holdingGun = true;
            _holdingFlashlight = false;
               
        }
                
        if (_heldObject.CompareTag("Flashlight"))
        {
            _holdingFlashlight = true;
            _holdingGun = false;
                
        }

        holdingObject = true;
        objectInHolster = false;
    }

    //something held something in holster
    //swap the two
    private void SwitchHolsterandHeld()
    {
        (_heldObject, _holsterObject) = (_holsterObject, _heldObject);

        _holsterObject.transform.position = holsterPosition.position;
        _holsterObject.transform.rotation = holsterPosition.rotation;
        _holsterObject.transform.parent = holsterPosition;

        _heldObject.transform.position = holdPosition.position;
        _heldObject.transform.rotation = holdPosition.rotation;
        _heldObject.transform.parent = holdPosition;

        if (_holdingGun)
        {
            _holdingGun = false;
            _holdingFlashlight = true;
//                holdingFlashlightText.SetActive(true);
//               holdingGunText.SetActive(false);


        }
        else if (_holdingFlashlight)
        {
            _holdingFlashlight = false;
            _holdingGun = true;
//                holdingFlashlightText.SetActive(false);
//               holdingGunText.SetActive(true);
        }

    }
    private void HolsterOrSwitchObject()
    {
        //nothing in holster something in hand
        // holster what is in hand > nothing in hand
        if (!objectInHolster && holdingObject)
        {
          Holster();
        }
        else
        //nothing held something in holster
        //put holster object in hand
        if (!holdingObject && objectInHolster)
        {
            UnHolster();
        }
        else
        //something held something in holster
        //swap the two
        if (objectInHolster && holdingObject)
        {
           SwitchHolsterandHeld();
        }
    }

    private void PickUpObject()
    {
        // Check if we are already holding an object
        /*if (heldObject != null)
        {
                HolsterOrSwitchObject();
        }*/
        
        
        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


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

            //else if (hit.collider.CompareTag("Door")) // Check if the object is a door
            //{
            //    // Start moving the door upwards
            //    StartCoroutine(RaiseDoor(hit.collider.gameObject));
            //}

            else if (hit.collider.CompareTag("Key"))
            {
                Destroy(hit.collider.gameObject);
                gotKey.SetActive(true);
                StartCoroutine(ReceivedKey());
                keyManager.addKeyLevel();
                worldSounds.clip = keySFX;
                worldSounds.Play();

            }

            else if (hit.collider.CompareTag("Battery"))
            {
                Destroy(hit.collider.gameObject);
                gotBattery.SetActive(true);
                batteryManager.addBatteryLevel();
                StartCoroutine(ReceivedBattery());
                worldSounds.clip = batterySFX;
                worldSounds.Play();


            }

            else if (hit.collider.CompareTag("Door") && keyManager.keyLevel > 0.99)
            {
                Destroy(hit.collider.gameObject);
                keyManager.decreaseKeyLevel();
                worldSounds.clip = doorSFX;
                worldSounds.Play();
            }


            else if (hit.collider.CompareTag("Radio"))
            {
                Destroy(hit.collider.gameObject);
                StartCoroutine(CollectedEvidence()); 
                StartCoroutine(EndChapter());
                collectedEvidence.SetActive(true);
                gotEvidence2 = true;
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();

            }

            else if (hit.collider.CompareTag("Knife"))
            {
                Destroy(hit.collider.gameObject);
                gotEvidence1 = true;
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());  
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();

            }
            // Check if the hit object has the tag "PickUp"
            /*if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                _heldObject = hit.collider.gameObject;
                _heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                _heldObject.transform.position = holdPosition.position;
                _heldObject.transform.rotation = holdPosition.rotation;
                _heldObject.transform.parent = holdPosition;
                holdingObject = true;
            }*/
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                if (!objectInHolster && holdingObject)
                {
                    Holster();
                }
                pickup_and_Hold(hit.collider.gameObject);
                holdingObject = true;
                _holdingGun = true;
                
               // holdingFlashlightText.SetActive(false);
             //   holdingGunText.SetActive(true);
            }
            else if (hit.collider.CompareTag("Flashlight"))
            {
                if (!objectInHolster && holdingObject)
                {
                    Holster();
                }
                // Pick up the object
               pickup_and_Hold(hit.collider.gameObject);
                
               _heldFlashlight = _heldObject;
                _holdingFlashlight = true;
               
              // holdingFlashlightText.SetActive(true);
              //  holdingGunText.SetActive(false);
            }
        }
    }

    private void pickup_and_Hold(GameObject objecttoHold)
    {
        _heldObject = objecttoHold;
        _heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
        // Attach the object to the hold position
        _heldObject.transform.position = holdPosition.position;
        _heldObject.transform.rotation = holdPosition.rotation;
        _heldObject.transform.parent = holdPosition;
        holdingObject = true;
    }

   /* private void Interact()
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

            //else if (hit.collider.CompareTag("Door")) // Check if the object is a door
            //{
            //    // Start moving the door upwards
            //    StartCoroutine(RaiseDoor(hit.collider.gameObject));
            //}

            else if (hit.collider.CompareTag("Key"))
            {
                Destroy(hit.collider.gameObject);
                gotKey.SetActive(true);
                StartCoroutine(receivedKey());
                keyManager.addKeyLevel();

            }

            else if (hit.collider.CompareTag("Battery"))
            {
                Destroy(hit.collider.gameObject);
                gotBattery.SetActive(true);
                StartCoroutine(receivedBattery());
                batteryAmount = batteryAmount + 1;
                batteryAmountText.text = batteryAmount.ToString();

            }

            else if (hit.collider.CompareTag("Door") && keyManager.keyLevel > 0.99)
            {
                Destroy(hit.collider.gameObject);
                keyManager.decreaseKeyLevel();
            }


            else if (hit.collider.CompareTag("Radio"))
            {
                Destroy(hit.collider.gameObject);
                SceneManager.LoadScene("End Screen");

            }
        }
    }*/

    private IEnumerator ReceivedKey()
    {
        yield return new WaitForSeconds(2);
        gotKey.SetActive(false);
    }

    private IEnumerator ReceivedBattery()
    {
        yield return new WaitForSeconds(2);
        gotBattery.SetActive(false);
    }

    private IEnumerator CollectedEvidence()
    {
        yield return new WaitForSeconds(2);
        collectedEvidence.SetActive(false);
    }

    private IEnumerator EndChapter()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("End Screen");

    }

    private void checkForPickup()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        //perform raycast to detect objects
        if (Physics.Raycast(ray, out hit, pickUpRange)) 
        { 
            if (hit.collider.CompareTag("Key"))
            {
                pickupText.SetActive(true);
            }
            else
            {
                pickupText.SetActive(false);
            }
        }
        else
        {
            pickupText.SetActive(false);
        }
    }
    private void ToggleCrouch()
    {
        if(isCrouching)
        {
            //stand up
            _characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
           _characterController.height = crouchHeight;
            isCrouching = true;
        }
    }

    /*public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Battery"))
        {
            Battery = other.gameObject;
            Debug.Log("added 1 to battery level");
            Destroy(Battery);
            
        }
        
        if(other.CompareTag("PurpleUpgrade"))
        {
            Debug.Log("got the purple upgrade");
            Destroy(purpleUpgrade);
            hasPurpleUpgrade = true;
        }

        if (other.CompareTag("RedUpgrade"))
        {
            Debug.Log("got the red upgrade");
            Destroy(redUpgrade);
            hasRedUpgrade = true;
        }

        if(other.CompareTag("Hostage"))
        {
            hostage = other.gameObject;
            Debug.Log("saved a hostage!");
            Destroy(hostage);
            hostages.addHostageNumber();
        }
    }*/
}


