using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("Other Scripts")]
    public soundManager sound_manage;
    public UI_manager ui_manage;
    public keyManager key_manage;
    public batteryManager battery_manage;
    public Flashlight _flashlight_script;
    
    [Header("MOVEMENT SETTINGS")]
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
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
    
    [Header("PICKING UP SETTINGS")]
    public Transform holdPosition; // Position where the picked-up object will be held
    public Transform holsterPosition; //Position where the holstered object will be held
    private GameObject _heldObject; // Reference to the currently held object
    private GameObject _holsterObject = null; // Reference to currently holstered object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    [SerializeField] private bool holdingObject = false;
    [SerializeField] private bool objectInHolster = false;
    
    [Header("CROUCH SETTINGS")]
    public float crouchHeight = 1f; //make short
    public float standingHeight = 2f; //make normal
    public float crouchSpeed = 1.5f; //short speed
    public bool isCrouching = false; //if short or normal

    /*[Header("INTERACT SETTINGS")]
    [Space(5)]
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color
    */
    
    //GUN and FLASHLIGHT
    [Header("GUN n FLASH n CROW")]
    private bool _holdingGun = false;
    private bool _holdingFlashlight = false;
    public bool flashlightOn = false;
    private GameObject _heldFlashlight;
    public GameObject spriteMask;
    //Got em
    public bool gotCrowbar = false;
    public bool gotNotebook = false;
    
    //PAUSE?
    private bool isPaused;
    
    //public GameObject switchText;
    
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
       
        //playerInput.Player.LookAround.performed += ctx => currentScheme = ctx.control;
        playerInput.Player.LookAround.canceled += ctx => _lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed
        
        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed
        
        // Subscribe to the Flashlight input event
        playerInput.Player.FlashlightSwitch.performed += ctx => FlashlightSwitch(); // Call the FlashlightSwitch method when shoot input is performed
        
        // Subscribe to the HolsternSwitch input event
        playerInput.Player.HolsterandSwitchheld.performed += ctx => HolsterOrSwitchObject(); // Call the Crouch method when crouch input is performed
        
        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the Crouch method when crouch input is performed
       
        
        
        /*
        // Subscribe to the notebook input event
        playerInput.Player.Notebook.performed += ctx => Notebook(); // open notebook

        // Subscribe to the PreviousPage input event
        playerInput.Player.PreviousPage.performed += ctx => PreviousPage(); // turn to the previous page
        
        // Subscribe to the NextPage input event
        playerInput.Player.NextPage.performed += ctx => NextPage(); // turn to the previous page
        */
        
        
        
        //Subscribe to the Pause
        playerInput.Player.Pause.performed += ctx => Pause(); // pause the game
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAround();
        ApplyGravity();
        checkForPickup();
    }

    private void Pause()
    {
        ui_manage.Pause();
    }
    
    private void Move()
    {
        if (isPaused)
        {
            return;
        }
        else
        {
            // Create a movement vector based on the input
            Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);

            // Transform direction from local to world space
            move = transform.TransformDirection(move);

            var currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

            // Move the character controller based on the movement vector and speed
            _characterController.Move(move * currentSpeed * Time.deltaTime);
        }
    }

    private void LookAround()
    {
        if (isPaused)
        {
            return;
        }

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
    
    private void checkForPickup()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        
        //perform raycast to detect objects
        /*if (Physics.Raycast(ray, out hit, pickUpRange)) 
        { 
            if (hit.collider.CompareTag("Key"))
            {
                pickupText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Door"))
            {
                openText.SetActive(true);
            }
            
            else if (hit.collider.CompareTag("Battery"))
            {
                pickupText.SetActive(true);
            }
            
            else if (hit.collider.CompareTag("Gun"))
            {
                pickupText.SetActive(true);
                safeText.SetActive(false);
            }
            
            else if (hit.collider.CompareTag("Flashlight"))
            {
                pickupText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Radio"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Knife"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Note1"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Note2"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Note3"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("noteOneCombination"))
            {
                noteOneCombinationText.SetActive(true);
                noteTwoCombinationText.SetActive(false);
                noteThreeCombinationText.SetActive(false);
                safeText.SetActive(false);
            }

            else if (hit.collider.CompareTag("noteTwoCombination"))
            {
                noteTwoCombinationText.SetActive(true);
                noteOneCombinationText.SetActive(false);
                noteThreeCombinationText.SetActive(false);
                safeText.SetActive(false);
            }
            
            else if (hit.collider.CompareTag("noteThreeCombination"))
            {
                noteThreeCombinationText.SetActive(true);
                noteTwoCombinationText.SetActive(false);
                noteOneCombinationText.SetActive(false);
                safeText.SetActive(false);
            }
            
            else if (hit.collider.CompareTag("Safe"))
            {
                noteThreeCombinationText.SetActive(false);
                noteTwoCombinationText.SetActive(false);
                noteOneCombinationText.SetActive(false);
                safeText.SetActive(true);
            }
            
            else if (hit.collider.CompareTag("Notebook"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Crowbar"))
            {
                collectText.SetActive(true);
            }
            
            else if (hit.collider.CompareTag("Plank"))
            {
                openText.SetActive(true);
            }
        }
        else
        {
            pickupText.SetActive(false);
            collectText.SetActive(false);
            openText.SetActive(false);
            noteOneCombinationText.SetActive(false);
            noteTwoCombinationText.SetActive(false);
            noteThreeCombinationText.SetActive(false);
            safeText.SetActive(false);
        }*/
    }

    private void Jump()
    {
        if (_characterController.isGrounded && isPaused == false)
        {
            // Calculate the jump velocity
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Shoot()
    {
        if (!_holdingGun) return;
        // Instantiate the projectile at the fire point
        if (isPaused != false) return;
        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        sound_manage.playStunGunSFX();

        // Get the Rigidbody component of the projectile and set its velocity
        var rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * projectileSpeed;

        // Destroy the projectile after 3 seconds
        Destroy(projectile, 0.5f);
    }

    private void FlashlightSwitch()
    {
        if (isPaused) return;
        
        var heldFlashlightLight = _heldFlashlight.GetComponent<Light>();
        
        if (heldFlashlightLight.enabled)
        {
            heldFlashlightLight.enabled = false;
            spriteMask.SetActive(false);
            flashlightOn = false;
        }
        else
        {
            if (_holdingFlashlight != true || !(battery_manage.batteryLevel > 0.99))
            {
                return;
            }
            
            heldFlashlightLight.enabled = true;
            battery_manage.decreaseBatteryLevel();
            flashlightOn = true;
            spriteMask.SetActive(true);
            sound_manage.playFlashlightSFX();
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

    //nothing in holster something in hand
    // holster what is in hand > nothing in hand
    private void Holster()
    {
        if (isPaused != false) return;
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

            ui_manage.DisplayUsingStunGun();

        }
        else if (_holdingFlashlight)
        {
            _holdingFlashlight = false;
            _holdingGun = true;

            ui_manage.DisplayUsingFlashlight();
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
    private void UpdateNotebook()
    {
        if (gotNotebook)
        {
            ui_manage.NotebookUpdated();
        }
    }
    
    private void PickUpObject()
    {
        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            /*if (hit.collider.CompareTag("Switch")) // Assuming the switch has this tag
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
            }*/
            
            if (hit.collider.CompareTag("Key"))
            {
                Destroy(hit.collider.gameObject);
                ui_manage.StartCoroutine(ui_manage.ReceivedKey());
                key_manage.addKeyLevel();
                sound_manage.playKeySFX();

                //hasUnlockedPageOne = true;
            }
            else if (hit.collider.CompareTag("Battery"))
            {
                Destroy(hit.collider.gameObject);
                ui_manage.StartCoroutine(ui_manage.ReceivedBattery());
                battery_manage.addBatteryLevel();
                sound_manage.playBatterySFX();
                
                //hasUnlockedPageTwo = true;
            }
            
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

                if (_holdingGun)

                {
                    ui_manage.DisplayUsingStunGun();
                }
                else
                {
                    ui_manage.DisplayUsingFlashlight();
                }

                //ui pick up text
                ui_manage.StartCoroutine(ui_manage.ReceivedBattery());
                sound_manage.playEvidenceSFX();
                //hasUnlockedPageFour = true;
                UpdateNotebook();
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

                if(_holdingFlashlight)

                {
                    ui_manage.DisplayUsingFlashlight();
                }
                else
                {
                    ui_manage.DisplayUsingStunGun();
                }
               
                //ui pick up text
                ui_manage.StartCoroutine(ui_manage.FlashlightText()); 
                sound_manage.playEvidenceSFX();

                //hasUnlockedPageThree = true;
                UpdateNotebook();
            }
            
            else if (hit.collider.CompareTag("Notebook"))
            {
                Destroy(hit.collider.gameObject);
                gotNotebook = true;
                ui_manage.StartCoroutine(ui_manage.NotebookText());
                sound_manage.playEvidenceSFX();

            }
            
            else if (hit.collider.CompareTag("Door"))
            {
                //Got key & Can open
                if (key_manage.keyLevel > 0.99)
                {
                    hit.collider.gameObject.GetComponent<Animator>().Play("Open", 0, 0.0f);
                    key_manage.decreaseKeyLevel();
                    sound_manage.playDoorSFX();
                }
                //No key & cant open
                else
                {
                    ui_manage.StartCoroutine(ui_manage.LockedDoor());
                    sound_manage.playLockedDoorSFX();
                }
            }
            
            else if (hit.collider.CompareTag("Knife"))
            {
                Destroy(hit.collider.gameObject);
                ui_manage.StartCoroutine(ui_manage.CollectedEvidence()); 
                sound_manage.playEvidenceSFX();
                
                //hasUnlockedPageFive = true;
                
                UpdateNotebook();
            }
            
            else if (hit.collider.CompareTag("Crowbar"))
            {
                gotCrowbar = true;
                Destroy(hit.collider.gameObject);
                ui_manage.StartCoroutine(ui_manage.CollectedEvidence());
                sound_manage.playEvidenceSFX();
                
                //hasUnlockedPageSix = true;
                
                UpdateNotebook();
            }
            
            else if (hit.collider.CompareTag("Plank"))
            {
                if (gotCrowbar)
                {
                    Destroy(hit.collider.gameObject);
                    sound_manage.playPlankSFX();
                }
                else
                {
                    ui_manage.StartCoroutine(ui_manage.BlockedDoor());
                    sound_manage.playPlankSFX();
                }
            }

            else if (hit.collider.CompareTag("Note1"))
            {
                Destroy(hit.collider.gameObject);
                //noteOneCombination.SetActive(true);
                
                ui_manage.StartCoroutine(ui_manage.CollectedEvidence());
                sound_manage.playEvidenceSFX();
                
                //hasUnlockedPageSeven = true;
                
                UpdateNotebook();
            }

            else if (hit.collider.CompareTag("Note2"))
            {
                Destroy(hit.collider.gameObject);
                //noteTwoCombination.SetActive(true);
                
                ui_manage.StartCoroutine(ui_manage.CollectedEvidence());
                sound_manage.playEvidenceSFX();
                
                //hasUnlockedPageEight = true;
                
                UpdateNotebook();
            }

            else if (hit.collider.CompareTag("Note3"))
            {
                Destroy(hit.collider.gameObject);
                //noteThreeCombination.SetActive(true);
                
                ui_manage.StartCoroutine(ui_manage.CollectedEvidence());
                sound_manage.playEvidenceSFX();
                
                //hasUnlockedPageNine = true;
                
                UpdateNotebook();
            }
            
            /*else if (hit.collider.CompareTag("noteOneCombination"))
            {
                wrongCombination.SetActive(true);
                StartCoroutine(WrongCombination());    
                noteThreeCombinationText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("noteTwoCombination"))
            {
                wrongCombination.SetActive(true);
                StartCoroutine(WrongCombination());
                noteTwoCombinationText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("noteThreeCombination"))
            {
                safeDoor.Play("SafeDoor", 0, 0.0f);
                rightCombination.SetActive(true);
                StartCoroutine(RightCombination());
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                Destroy(safeText);
            }*/
            
               
            /*else if (hit.collider.CompareTag("Radio"))
            {
                Destroy(hit.collider.gameObject);
                StartCoroutine(CollectedEvidence());
                StartCoroutine(EndChapter());
                collectedEvidence.SetActive(true);
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageTen = true;

               if(gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }
            }*/
        }
    }
}
