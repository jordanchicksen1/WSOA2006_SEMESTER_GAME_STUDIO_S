using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;
using Debug = System.Diagnostics.Debug;

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

    public enum ControlScheme{
        Gamepad,
        Keyboard
    }
    

    public ControlScheme currentScheme;
    
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
    public GameObject gunUiText;
    public GameObject flashlightUiText;

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
   
    //key text
    public GameObject gotKey;

    //battery text
    public GameObject gotBattery;

    //sound effects general
    public AudioSource worldSounds;
    public AudioClip keySFX;
    public AudioClip batterySFX;
    public AudioClip flashlightSFX;
    public AudioClip doorSFX;
    public AudioClip stungunSFX;
    public AudioClip evidenceSFX;
    public AudioClip lockedDoorSFX;
    public AudioClip pageSFX;
    public AudioClip plankSFX;
    public AudioClip blockedDoorSFX;

    //pick up text
    public GameObject pickupText;
    public GameObject collectText;
    public GameObject openText;

    //evidence related stuff
    public bool gotEvidence1 = false;
    public bool gotEvidence2 = false;
    public GameObject collectedEvidence;

    //locked door stuff
    public GameObject lockedDoor;
    public GameObject blockedDoor;

    //scream trigger stuff
    public AudioSource radioBox;
    public AudioClip scream1;

    //controls text
    public GameObject moveLookTMP;
    public GameObject jumpCrouchTMP;

    //ui images
    public GameObject flashlightUI;
    public GameObject stungunUI;

    //notebook stuff
    public GameObject notebookText;
    public bool gotNotebook = false;
    public GameObject NotebookUIPages;
    public GameObject crosshair;
    public bool openedNotebook = false;
    public GameObject startPage;
    public bool onStartPage = false;
    public GameObject firstPageLocked;
    public bool onFirstPage = false;
    public GameObject secondPageLocked;
    public bool onSecondPage = false;
    public GameObject thirdPageLocked;
    public bool onThirdPage = false;
    public GameObject fourthPageLocked;
    public bool onFourthPage = false;
    public GameObject fifthPageLocked;
    public bool onFifthPage = false;
    public GameObject sixthPageLocked;
    public bool onSixthPage = false;
    public GameObject seventhPageLocked;
    public bool onSeventhPage = false;
    public GameObject eighthPageLocked;
    public bool onEighthPage = false;
    public GameObject ninethPageLocked;
    public bool onNinthPage = false;
    public GameObject tenthPageLocked;
    public bool onTenthPage = false;

    public GameObject previousPageText;
    public GameObject nextPageText;

    //unlocked pages
    public GameObject unlockedPageOne;
    public GameObject unlockedPageTwo;
    public GameObject unlockedPageThree;
    public GameObject unlockedPageFour;
    public GameObject unlockedPageFive;
    public GameObject unlockedPageSixth;
    public GameObject unlockedPageSeventh;
    public GameObject unlockedPageEighth;
    public GameObject unlockedPageNineth;
    public GameObject unlockedPageTenth;
   // public notebookManager notebookManager;
    public bool isOnUnlockedPageOne = false;
    public bool hasUnlockedPageOne = false;
    public bool isOnUnlockedPageTwo = false;
    public bool hasUnlockedPageTwo = false;
    public bool isOnUnlockedPageThree = false;
    public bool hasUnlockedPageThree = false;
    public bool isOnUnlockedPageFour = false;
    public bool hasUnlockedPageFour = false;
    public bool isOnUnlockedPageFive = false;
    public bool hasUnlockedPageFive = false;
    public bool isOnUnlockedPageSix = false;
    public bool hasUnlockedPageSix = false;
    public bool isOnUnlockedPageSeven = false;
    public bool hasUnlockedPageSeven = false;
    public bool isOnUnlockedPageEight = false;
    public bool hasUnlockedPageEight = false;
    public bool isOnUnlockedPageNine = false;
    public bool hasUnlockedPageNine = false;
    public bool isOnUnlockedPageTen = false;
    public bool hasUnlockedPageTen = false;
    public GameObject notebookUpdateText;

    //switch text
    public GameObject switchText;

    //crowbarStuff
    public bool gotCrowbar = false;

    //safe stuff
    public GameObject noteOneCombination;
    public GameObject noteTwoCombination;
    public GameObject noteThreeCombination;

    public GameObject noteOneCombinationText;
    public GameObject noteTwoCombinationText;
    public GameObject noteThreeCombinationText;

    public GameObject safeText;

    public GameObject wrongCombination;
    public GameObject rightCombination;

    [SerializeField]
    private Animator safeDoor = null;

    public InputControl currentControl;
    
   
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

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the Crouch method when crouch input is performed

        // Subscribe to the crouch input event
        playerInput.Player.HolsterandSwitchheld.performed += ctx => HolsterOrSwitchObject(); // Call the Crouch method when crouch input is performed

        // Subscribe to the notebook input event
        playerInput.Player.Notebook.performed += ctx => Notebook(); // open notebook

        // Subscribe to the PreviousPage input event
        playerInput.Player.PreviousPage.performed += ctx => PreviousPage(); // turn to the previous page


        // Subscribe to the NextPage input event
        playerInput.Player.NextPage.performed += ctx => NextPage(); // turn to the previous page


    }
      
    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        checkForPickup();
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
        print(currentControl);
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

            flashlightUI.SetActive(true);
            stungunUI.SetActive(false);

        }
        else if (_holdingFlashlight)
        {
            _holdingFlashlight = false;
            _holdingGun = true;

            flashlightUI.SetActive(false);
            stungunUI.SetActive(true);
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
        
        
        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        //Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


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


            else if (hit.collider.CompareTag("Key"))
            {
                Destroy(hit.collider.gameObject);
                gotKey.SetActive(true);
                StartCoroutine(ReceivedKey());
                keyManager.addKeyLevel();
                worldSounds.clip = keySFX;
                worldSounds.Play();

                hasUnlockedPageOne = true;
               
              

            }

            else if (hit.collider.CompareTag("Battery"))
            {
                Destroy(hit.collider.gameObject);
                gotBattery.SetActive(true);
                batteryManager.addBatteryLevel();
                StartCoroutine(ReceivedBattery());
                worldSounds.clip = batterySFX;
                worldSounds.Play();
                hasUnlockedPageTwo = true;


            }

            else if (hit.collider.CompareTag("Door") && keyManager.keyLevel > 0.99)
            {
                Destroy(hit.collider.gameObject);
                keyManager.decreaseKeyLevel();
                worldSounds.clip = doorSFX;
                worldSounds.Play();
            }

            else if (hit.collider.CompareTag("Door") && keyManager.keyLevel == 0)
            {
                lockedDoor.SetActive(true);
                StartCoroutine(LockedDoor());
                worldSounds.clip = lockedDoorSFX;
                worldSounds.Play();
            }


            else if (hit.collider.CompareTag("Radio"))
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

            }

            else if (hit.collider.CompareTag("Knife"))
            {
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());  
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageFive = true;
                if(gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }

            }

            else if (hit.collider.CompareTag("Crowbar"))
            {
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageSix = true;
                if(gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }

            }
            else if (hit.collider.CompareTag("Plank") && hasUnlockedPageSix == true)
            {
                Destroy(hit.collider.gameObject);
                worldSounds.clip = plankSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("Plank") && hasUnlockedPageSix == false)
            {
                blockedDoor.SetActive(true);
                StartCoroutine(BlockedDoor());
                worldSounds.clip = blockedDoorSFX;
                worldSounds.Play();
            }

            else if (hit.collider.CompareTag("Note1"))
            {
                Destroy(hit.collider.gameObject);
                noteOneCombination.SetActive(true);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageSeven = true;
                if(gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }

            }

            else if (hit.collider.CompareTag("Note2"))
            {
                Destroy(hit.collider.gameObject);
                noteTwoCombination.SetActive(true);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageEight = true;
                if(gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }

            }

            else if (hit.collider.CompareTag("Note3"))
            {
                Destroy(hit.collider.gameObject);
                noteThreeCombination.SetActive(true);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageNine = true;
                if(gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }

            }
            else if (hit.collider.CompareTag("noteOneCombination"))
            {
                wrongCombination.SetActive(true);
                StartCoroutine(WrongCombination());    
                noteThreeCombinationText.SetActive(false);
            }
            else if (hit.collider.CompareTag("noteTwoCombination"))
            {
                wrongCombination.SetActive(true);
                StartCoroutine(WrongCombination());
                noteTwoCombinationText.SetActive(false);
            }
            else if (hit.collider.CompareTag("noteThreeCombination"))
            {
                safeDoor.Play("SafeDoor", 0, 0.0f);
                rightCombination.SetActive(true);
                StartCoroutine(RightCombination());
            }


            else if (hit.collider.CompareTag("Notebook"))
            {
                Destroy(hit.collider.gameObject);
                gotNotebook = true;
                notebookText.SetActive(true);
                StartCoroutine(NotebookText());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();

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

                if (_holdingGun == true)

                {
                    flashlightUI.SetActive(false);
                    stungunUI.SetActive(true);
                    switchText.SetActive(true);
                }
                else
                {
                    flashlightUI.SetActive(true);
                    stungunUI.SetActive(false);
                    switchText.SetActive(true);
                }

                //ui pick up text
                gunUiText.SetActive(true);
                StartCoroutine(StunGunText());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageFour = true;
                if(gotNotebook)
                {
                    notebookUpdateText.SetActive(true);
                }
                

              
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

                if(_holdingFlashlight == true)

                {
                    flashlightUI.SetActive(true);
                    stungunUI.SetActive(false);
                }
                else
                {
                    flashlightUI.SetActive(false);
                    stungunUI.SetActive(true);
                }
               
                //ui pick up text
                flashlightUiText.SetActive(true);   
                StartCoroutine(FlashlightText());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();

                hasUnlockedPageThree = true;
                if(gotNotebook)
                {
                    notebookUpdateText.SetActive(true);
                }
               
              
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

    private void Notebook()
    {
        if (gotNotebook == true && openedNotebook == false)
        {
            NotebookUIPages.SetActive(true);
            crosshair.SetActive(false);
            openedNotebook = true;
            startPage.SetActive(true);
            onStartPage = true;
            worldSounds.clip = pageSFX;
            worldSounds.Play();
            nextPageText.SetActive(true);
            notebookText.SetActive(false);
            notebookUpdateText.SetActive(false);
        }

        else if(openedNotebook == true) 
        {
            NotebookUIPages.SetActive(false);
            crosshair.SetActive(true);

            startPage.SetActive(false);
            firstPageLocked.SetActive(false);
            secondPageLocked.SetActive(false);
            thirdPageLocked.SetActive(false);
            fourthPageLocked.SetActive(false);
            fifthPageLocked.SetActive(false);
            sixthPageLocked.SetActive(false);
            seventhPageLocked.SetActive(false);
            eighthPageLocked.SetActive(false);
            ninethPageLocked.SetActive(false);
            tenthPageLocked.SetActive(false);

            unlockedPageOne.SetActive(false);
            unlockedPageTwo.SetActive(false);
            unlockedPageThree.SetActive(false);
            unlockedPageFour.SetActive(false);
            unlockedPageFive.SetActive(false);
            unlockedPageSixth.SetActive(false);
            unlockedPageSeventh.SetActive(false);
            unlockedPageEighth.SetActive(false);
            unlockedPageNineth.SetActive(false);    
            unlockedPageTenth.SetActive(false);

            onStartPage = false;
            onFirstPage = false;
            onSecondPage = false;
            onThirdPage = false;
            onFourthPage = false;
            onFifthPage = false;
            onSixthPage = false;
            onSeventhPage = false;
            onEighthPage = false;
            onNinthPage = false;
            onTenthPage = false;
            openedNotebook = false;

            previousPageText.SetActive(false);
            nextPageText.SetActive(false);


           
        }
    }

    private void PreviousPage()
    {
        if(openedNotebook == true) 
        {   
            //locked pages segment if you don't have the previous page
            if(onFirstPage == true) 
            {
                startPage.SetActive(true);
                onFirstPage = false;
                firstPageLocked.SetActive(false);
                onStartPage = true;
                previousPageText.SetActive(false);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if(onSecondPage == true && hasUnlockedPageOne == false)
            {
                firstPageLocked.SetActive(true);
                onSecondPage = false;
                secondPageLocked.SetActive(false);
                onFirstPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if(onThirdPage == true && hasUnlockedPageTwo == false)
            {
                secondPageLocked.SetActive(true);
                onThirdPage = false;
                thirdPageLocked.SetActive(false);
                onSecondPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFourthPage == true && hasUnlockedPageThree == false)
            {
                thirdPageLocked.SetActive(true);
                onFourthPage = false;
                fourthPageLocked.SetActive(false);
                onThirdPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFifthPage == true && hasUnlockedPageFour == false)
            {
                fourthPageLocked.SetActive(true);
                onFifthPage = false;
                fifthPageLocked.SetActive(false);
                onFourthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSixthPage == true && hasUnlockedPageFive == false)
            {
                fifthPageLocked.SetActive(true);
                onSixthPage = false;
                sixthPageLocked.SetActive(false);
                onFifthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSeventhPage == true && hasUnlockedPageSix == false)
            {
                sixthPageLocked.SetActive(true);
                onSeventhPage = false;
                seventhPageLocked.SetActive(false);
                onSixthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onEighthPage == true && hasUnlockedPageSeven == false)
            {
                seventhPageLocked.SetActive(true);
                onEighthPage = false;
                eighthPageLocked.SetActive(false);
                onSeventhPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onNinthPage == true && hasUnlockedPageEight == false)
            {
                eighthPageLocked.SetActive(true);
                onNinthPage = false;
                ninethPageLocked.SetActive(false);
                onEighthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onTenthPage == true && hasUnlockedPageNine == false)
            {
                ninethPageLocked.SetActive(true);
                onTenthPage = false;
                tenthPageLocked.SetActive(false);
                onNinthPage = true;
                nextPageText.SetActive(true);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            //unlocked pages segment if you do have the previous page
            else if (isOnUnlockedPageOne == true)
            {
                startPage.SetActive(true);
                isOnUnlockedPageOne = false;
                unlockedPageOne.SetActive(false);
                onStartPage = true;
                previousPageText.SetActive(false);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageTwo == true && hasUnlockedPageOne == true)
            {
                unlockedPageOne.SetActive(true);
                isOnUnlockedPageOne = true;
                unlockedPageTwo.SetActive(false);
                isOnUnlockedPageTwo = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageThree == true && hasUnlockedPageTwo == true)
            {
                unlockedPageTwo.SetActive(true);
                isOnUnlockedPageTwo = true;
                unlockedPageThree.SetActive(false);
                isOnUnlockedPageThree = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageFour == true && hasUnlockedPageThree == true)
            {
                unlockedPageThree.SetActive(true);
                isOnUnlockedPageThree = true;
                unlockedPageFour.SetActive(false);
                isOnUnlockedPageFour = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageFive == true && hasUnlockedPageFour == true)
            {
                unlockedPageFour.SetActive(true);
                isOnUnlockedPageFour = true;
                unlockedPageFive.SetActive(false);
                isOnUnlockedPageFive = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSix == true && hasUnlockedPageFive == true)
            {
                unlockedPageFive.SetActive(true);
                isOnUnlockedPageFive = true;
                unlockedPageSixth.SetActive(false);
                isOnUnlockedPageSix = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSeven == true && hasUnlockedPageSix == true)
            {
                unlockedPageSixth.SetActive(true);
                isOnUnlockedPageSix = true;
                unlockedPageSeventh.SetActive(false);
                isOnUnlockedPageSeven = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageEight == true && hasUnlockedPageSeven == true)
            {
                unlockedPageSeventh.SetActive(true);
                isOnUnlockedPageSeven = true;
                unlockedPageEighth.SetActive(false);
                isOnUnlockedPageEight = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageNine == true && hasUnlockedPageEight == true)
            {
                unlockedPageEighth.SetActive(true);
                isOnUnlockedPageEight = true;
                unlockedPageNineth.SetActive(false);
                isOnUnlockedPageNine = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageTen == true && hasUnlockedPageNine == true)
            {
                unlockedPageNineth.SetActive(true);
                isOnUnlockedPageNine = true;
                unlockedPageTenth.SetActive(false);
                isOnUnlockedPageTen = false;
                nextPageText.SetActive(true);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            //locked pages segment if you do have the previous page
            else if (onSecondPage == true && hasUnlockedPageOne == true)
            {
                unlockedPageOne.SetActive(true);
                onSecondPage = false;
                secondPageLocked.SetActive(false);
                isOnUnlockedPageOne = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onThirdPage == true && hasUnlockedPageTwo == true)
            {
                unlockedPageTwo.SetActive(true);
                onThirdPage = false;
                thirdPageLocked.SetActive(false);
                isOnUnlockedPageTwo = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFourthPage == true && hasUnlockedPageThree == true)
            {
                unlockedPageThree.SetActive(true);
                onFourthPage = false;
                fourthPageLocked.SetActive(false);
                isOnUnlockedPageThree = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFifthPage == true && hasUnlockedPageFour == true)
            {
                unlockedPageFour.SetActive(true);
                onFifthPage = false;
                fifthPageLocked.SetActive(false);
                isOnUnlockedPageFour = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSixthPage == true && hasUnlockedPageFive == true)
            {
                unlockedPageFive.SetActive(true);
                onSixthPage = false;
                sixthPageLocked.SetActive(false);
                isOnUnlockedPageFive = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSeventhPage == true && hasUnlockedPageSix == true)
            {
                unlockedPageSixth.SetActive(true);
                onSeventhPage = false;
                seventhPageLocked.SetActive(false);
                isOnUnlockedPageSix = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onEighthPage == true && hasUnlockedPageSeven == true)
            {
                unlockedPageSeventh.SetActive(true);
                onEighthPage = false;
                eighthPageLocked.SetActive(false);
                isOnUnlockedPageSeven = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onNinthPage == true && hasUnlockedPageEight == true)
            {
                unlockedPageEighth.SetActive(true);
                onNinthPage = false;
                ninethPageLocked.SetActive(false);
                isOnUnlockedPageEight = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onTenthPage == true && hasUnlockedPageNine == true)
            {
                unlockedPageNineth.SetActive(true);
                onTenthPage = false;
                tenthPageLocked.SetActive(false);
                isOnUnlockedPageNine = true;
                nextPageText.SetActive(true);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }

            //unlocked pages segement if you don't have the previous page
            else if (isOnUnlockedPageTwo == true && hasUnlockedPageOne == false)
            {
                firstPageLocked.SetActive(true);
                onFirstPage = true;
                unlockedPageTwo.SetActive(false);
                isOnUnlockedPageTwo = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageThree == true && hasUnlockedPageTwo == false)
            {
                secondPageLocked.SetActive(true);
                onSecondPage = true;
                unlockedPageThree.SetActive(false);
                isOnUnlockedPageThree = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageFour == true && hasUnlockedPageThree == false)
            {
                thirdPageLocked.SetActive(true);
                onThirdPage = true;
                unlockedPageFour.SetActive(false);
                isOnUnlockedPageFour = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageFive == true && hasUnlockedPageFour == false)
            {
                fourthPageLocked.SetActive(true);
                onFourthPage = true;
                unlockedPageFive.SetActive(false);
                isOnUnlockedPageFive = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSix == true && hasUnlockedPageFive == false)
            {
                fifthPageLocked.SetActive(true);
                onFifthPage = true;
                unlockedPageSixth.SetActive(false);
                isOnUnlockedPageSix = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSeven == true && hasUnlockedPageSix == false)
            {
                sixthPageLocked.SetActive(true);
                onSixthPage = true;
                unlockedPageSeventh.SetActive(false);
                isOnUnlockedPageSeven = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageEight == true && hasUnlockedPageSeven == false)
            {
                seventhPageLocked.SetActive(true);
                onSeventhPage = true;
                unlockedPageEighth.SetActive(false);
                isOnUnlockedPageEight = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageNine == true && hasUnlockedPageEight == false)
            {
                eighthPageLocked.SetActive(true);
                onEighthPage = true;
                unlockedPageNineth.SetActive(false);
                isOnUnlockedPageNine = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageTen == true && hasUnlockedPageNine == false)
            {
                ninethPageLocked.SetActive(true);
                onNinthPage = true;
                unlockedPageTenth.SetActive(false);
                isOnUnlockedPageTen = false;
                nextPageText.SetActive(true);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }



        }
    }

    private void NextPage()
    {
        if(openedNotebook== true)
        {   
            //locked pages segment if you don't have the next page
            if(onStartPage == true && hasUnlockedPageOne == false) 
            {
            startPage.SetActive(false);
            onFirstPage = true;
            firstPageLocked.SetActive(true);
            onStartPage = false;
            previousPageText.SetActive(true);
            worldSounds.clip = pageSFX;
            worldSounds.Play();
            }
            else if(onFirstPage == true && hasUnlockedPageTwo == false) 
            {
                firstPageLocked.SetActive(false);
                onSecondPage = true;
                secondPageLocked.SetActive(true);
                onFirstPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSecondPage == true && hasUnlockedPageThree == false)
            {
                secondPageLocked.SetActive(false);
                onThirdPage = true;
                thirdPageLocked.SetActive(true);
                onSecondPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onThirdPage == true && hasUnlockedPageFour == false)
            {
                thirdPageLocked.SetActive(false);
                onFourthPage = true;
                fourthPageLocked.SetActive(true);
                onThirdPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFourthPage == true && hasUnlockedPageFive == false)
            {
                fourthPageLocked.SetActive(false);
                onFifthPage = true;
                fifthPageLocked.SetActive(true);
                onFourthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFifthPage == true && hasUnlockedPageSix == false)
            {
                fifthPageLocked.SetActive(false);
                onSixthPage = true;
                sixthPageLocked.SetActive(true);
                onFifthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSixthPage == true && hasUnlockedPageSeven == false)
            {
                sixthPageLocked.SetActive(false);
                onSeventhPage = true;
                seventhPageLocked.SetActive(true);
                onSixthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }

            else if (onSeventhPage == true && hasUnlockedPageEight == false)
            {
                seventhPageLocked.SetActive(false);
                onEighthPage = true;
                eighthPageLocked.SetActive(true);
                onSeventhPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onEighthPage == true && hasUnlockedPageNine == false)
            {
                eighthPageLocked.SetActive(false);
                onNinthPage = true;
                ninethPageLocked.SetActive(true);
                onEighthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onNinthPage == true && hasUnlockedPageTen == false)
            {
                ninethPageLocked.SetActive(false);
                onTenthPage = true;
                tenthPageLocked.SetActive(true);
                onNinthPage = false;
                nextPageText.SetActive(false);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            //unlocked pages segment if you have the next page
            else if (onStartPage == true && hasUnlockedPageOne == true)
            {
                startPage.SetActive(false);
                isOnUnlockedPageOne = true;
                unlockedPageOne.SetActive(true);
                onStartPage = false;
                previousPageText.SetActive(true);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageOne == true && hasUnlockedPageTwo == true)
            {
                unlockedPageOne.SetActive(false);
                isOnUnlockedPageOne = false;
                unlockedPageTwo.SetActive(true);
                isOnUnlockedPageTwo = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageTwo == true && hasUnlockedPageThree == true)
            {
                unlockedPageTwo.SetActive(false);
                isOnUnlockedPageTwo = false;
                unlockedPageThree.SetActive(true);
                isOnUnlockedPageThree = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageThree == true && hasUnlockedPageFour == true)
            {
                unlockedPageThree.SetActive(false);
                isOnUnlockedPageThree = false;
                unlockedPageFour.SetActive(true);
                isOnUnlockedPageFour = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageFour == true && hasUnlockedPageFive == true)
            {
                unlockedPageFour.SetActive(false);
                isOnUnlockedPageFour = false;
                unlockedPageFive.SetActive(true);
                isOnUnlockedPageFive = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            
            else if (isOnUnlockedPageFive == true && hasUnlockedPageSix == true)
            {
                unlockedPageFive.SetActive(false);
                isOnUnlockedPageFive = false;
                unlockedPageSixth.SetActive(true);
                isOnUnlockedPageSix = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSix == true && hasUnlockedPageSeven == true)
            {
                unlockedPageSixth.SetActive(false);
                isOnUnlockedPageSix = false;
                unlockedPageSeventh.SetActive(true);
                isOnUnlockedPageSeven = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSeven == true && hasUnlockedPageEight == true)
            {
                unlockedPageSeventh.SetActive(false);
                isOnUnlockedPageSeven = false;
                unlockedPageEighth.SetActive(true);
                isOnUnlockedPageEight = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageEight == true && hasUnlockedPageNine == true)
            {
                unlockedPageEighth.SetActive(false);
                isOnUnlockedPageEight = false;
                unlockedPageNineth.SetActive(true);
                isOnUnlockedPageNine = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageNine == true && hasUnlockedPageTen == true)
            {
                unlockedPageNineth.SetActive(false);
                isOnUnlockedPageNine = false;
                unlockedPageTenth.SetActive(true);
                isOnUnlockedPageTen = true;
                nextPageText.SetActive(false);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            //locked pages segment if you do have the next page
            else if (onFirstPage == true && hasUnlockedPageTwo == true)
            {
                firstPageLocked.SetActive(false);
                isOnUnlockedPageTwo = true;
                unlockedPageTwo.SetActive(true);
                onFirstPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSecondPage == true && hasUnlockedPageThree == true)
            {
                secondPageLocked.SetActive(false);
                isOnUnlockedPageThree = true;
                unlockedPageThree.SetActive(true);
                onSecondPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onThirdPage == true && hasUnlockedPageFour == true)
            {
                thirdPageLocked.SetActive(false);
                isOnUnlockedPageFour = true;
                unlockedPageFour.SetActive(true);
                onThirdPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFourthPage == true && hasUnlockedPageFive == true)
            {
                fourthPageLocked.SetActive(false);
                isOnUnlockedPageFive = true;
                unlockedPageFive.SetActive(true);
                onFourthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onFifthPage == true && hasUnlockedPageSix == true)
            {
                fifthPageLocked.SetActive(false);
                isOnUnlockedPageSix = true;
                unlockedPageSixth.SetActive(true);
                onFifthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onSixthPage == true && hasUnlockedPageSeven == true)
            {
                sixthPageLocked.SetActive(false);
                isOnUnlockedPageSeven = true;
                unlockedPageSeventh.SetActive(true);
                onSixthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }

            else if (onSeventhPage == true && hasUnlockedPageEight == true)
            {
                seventhPageLocked.SetActive(false);
                isOnUnlockedPageEight = true;
                unlockedPageEighth.SetActive(true);
                onSeventhPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onEighthPage == true && hasUnlockedPageNine == true)
            {
                eighthPageLocked.SetActive(false);
                isOnUnlockedPageNine = true;
                unlockedPageNineth.SetActive(true);
                onEighthPage = false;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (onNinthPage == true && hasUnlockedPageTen == true)
            {
                ninethPageLocked.SetActive(false);
                isOnUnlockedPageTen = true;
                unlockedPageTenth.SetActive(true);
                onNinthPage = false;
                nextPageText.SetActive(false);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }


            //unlocked pages segement if you don't have the next page
            else if (isOnUnlockedPageOne == true && hasUnlockedPageTwo == false)
            {
                unlockedPageOne.SetActive(false);
                isOnUnlockedPageOne = false;
                secondPageLocked.SetActive(true);
                onSecondPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageTwo == true && hasUnlockedPageThree == false)
            {
                unlockedPageTwo.SetActive(false);
                isOnUnlockedPageTwo = false;
                thirdPageLocked.SetActive(true);
                onThirdPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageThree == true && hasUnlockedPageFour == false)
            {
                unlockedPageThree.SetActive(false);
                isOnUnlockedPageThree = false;
                fourthPageLocked.SetActive(true);
                onFourthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageFour == true && hasUnlockedPageFive == false)
            {
                unlockedPageFour.SetActive(false);
                isOnUnlockedPageFour = false;
                fifthPageLocked.SetActive(true);
                onFifthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            
            else if (isOnUnlockedPageFive == true && hasUnlockedPageSix == false)
            {
                unlockedPageFive.SetActive(false);
                isOnUnlockedPageFive = false;
                sixthPageLocked.SetActive(true);
                onSixthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSix == true && hasUnlockedPageSeven == false)
            {
                unlockedPageSixth.SetActive(false);
                isOnUnlockedPageSix = false;
                seventhPageLocked.SetActive(true);
                onSeventhPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageSeven == true && hasUnlockedPageEight == false)
            {
                unlockedPageSeventh.SetActive(false);
                isOnUnlockedPageSeven = false;
                eighthPageLocked.SetActive(true);
                onEighthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageEight == true && hasUnlockedPageNine == false)
            {
                unlockedPageEighth.SetActive(false);
                isOnUnlockedPageEight = false;
                ninethPageLocked.SetActive(true);
                onNinthPage = true;
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
            else if (isOnUnlockedPageNine == true && hasUnlockedPageTen == false)
            {
                unlockedPageNineth.SetActive(false);
                isOnUnlockedPageNine = false;
                tenthPageLocked.SetActive(true);
                onTenthPage = true;
                nextPageText.SetActive(false);
                worldSounds.clip = pageSFX;
                worldSounds.Play();
            }
        }

    }



    public void Start()
    {
        StartCoroutine(StartControlsText());
    }

    private IEnumerator ReceivedKey()
    {
        yield return new WaitForSeconds(1.5f);
        gotKey.SetActive(false);
    }

    private IEnumerator WrongCombination()
    {
        yield return new WaitForSeconds(1.5f);
        wrongCombination.SetActive(false);
    }

    private IEnumerator RightCombination()
    {
        yield return new WaitForSeconds(1.5f);
        rightCombination.SetActive(false);
    }

    private IEnumerator ReceivedBattery()
    {
        yield return new WaitForSeconds(1.5f);
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

    private IEnumerator LockedDoor()
    {
        yield return new WaitForSeconds(1f);
        lockedDoor.SetActive(false);
    }

    private IEnumerator BlockedDoor()
    {
        yield return new WaitForSeconds(1f);
        blockedDoor.SetActive(false);
    }

    private IEnumerator FlashlightText()
    {
        yield return new WaitForSeconds(5f);
        flashlightUiText.SetActive(false);
    }

    private IEnumerator NotebookText()
    {
        yield return new WaitForSeconds(5f);
        notebookText.SetActive(false);
    }

    private IEnumerator StunGunText()
    {
        yield return new WaitForSeconds(4f);
        gunUiText.SetActive(false);
    }

    private IEnumerator StartControlsText() 
    {
        yield return new WaitForSeconds(1.5f);
        moveLookTMP.SetActive(true);
        StartCoroutine(StartControlsTwoText());
    }

    private IEnumerator StartControlsTwoText() 
    {
        yield return new WaitForSeconds(4f);
        moveLookTMP.SetActive(false);
        jumpCrouchTMP.SetActive(true);
        StartCoroutine (StartControlsThreeText());    
    }

    private IEnumerator StartControlsThreeText() 
    {
        yield return new WaitForSeconds(3.5f);
        jumpCrouchTMP.SetActive(false);
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

   

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "screamTrigger") 
        {
            radioBox.clip = scream1;
            radioBox.Play();
            //Debug.Log("entered trigger");
        }
    }
}


