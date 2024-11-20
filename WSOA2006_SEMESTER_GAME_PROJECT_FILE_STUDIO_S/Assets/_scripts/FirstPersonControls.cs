using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

using static UnityEngine.Rendering.DebugUI;
using Debug = UnityEngine.Debug;

public class FirstPersonControls : MonoBehaviour
{
    public batteryManager battMan;
    public UI_manager UIman;
    public Safe safeMan;

    public GameObject GotCrowbarText;
    public GameObject GotNoteText;

    private bool FoundFlashlight = false;
    private bool FirstBattery = false;
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

    public Animator Grab;
    
    //Battery Stuff
   // public batteryManager batteryManager;

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
    public AudioClip incorrectSFX;
    public AudioClip correctSFX;

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
    public GameObject crowbar;

    public Animator crowBarAnimator;
    //safe stuff
    public GameObject noteOneCombination;
    public GameObject noteTwoCombination;
    public GameObject noteThreeCombination;

    public GameObject noteOneCombinationText;
    public GameObject noteTwoCombinationText;
    public GameObject noteThreeCombinationText;

    public GameObject safeKeypad;
        
    public GameObject safeText;

    public GameObject wrongCombination;
    public GameObject rightCombination;

    
    [SerializeField]
    private Animator safeDoor;

    public InputControl currentControl;
    
    public GameObject flickeringLight1;
    public GameObject flickeringLight2;
    public GameObject flickeringLight3;
    public GameObject flickeringLight4;
    public GameObject flickeringLight5;

    //pause screen stuff
    public bool isPaused = false;
    public bool isOnMainScreen = false;
    public bool isOnControlsScreen = false;
    public bool isOnControlsScreenKeyboard = false;
    public GameObject pauseScreen;
    public GameObject mainScreen;
    public GameObject controlsScreen;
    public GameObject controlsScreenKeyboard;
    
    public GameObject flySound1;
    public GameObject flySound2;
    public GameObject flySound3;

    //flashlight minusing percentage over time stuff
    public bool flashlightOn = false;

    //level 2 items and stuff like that
    public bool gotWrench = false;
    public bool gotLever = false;
    public bool gotFuse = false;
    public bool gotCog = false;
    public bool gotScrewdriver = false;
    public bool boxFixed = false;
    public bool addedLever = false;
    public bool addedFuse = false;
    public bool addedCog = false;
    public bool powerOn = false;

    public AudioClip metalLocked;
    public AudioClip metalOpened;
    public AudioClip moveSFX;

    public GameObject fixText;
    public GameObject needPartsText;
    public GameObject partAddedText;
    public GameObject powerRestoredText;
    public GameObject fuse;
    public GameObject cog;
    public GameObject lever;
    public GameObject leverDown;
    
    public bool gotBigKey = false;
    public GameObject secondBookshelf;
    public GameObject moveText;
    public GameObject stealText;

    public GameObject wayOut;
    public GameObject wayOutOpened;
    public bool wayOutUnlocked = false;
    public GameObject openedWayOutText;


    public GameObject talkText;
    public GameObject brentText1;
    public GameObject victimText1;
    public GameObject brentText2;
    public GameObject victimText2;
    public GameObject brentText3;
    public GameObject victimText3;
    public GameObject victimText3cont;
    public GameObject brentTextPower; //i've turned on the power
    public GameObject victimTextPower; //great, now i need you to turn on the power
    public GameObject brentTextWayOut; // i've opened a way out
    public GameObject victimTextWayOut; //great, now i need you to find a way out
    public GameObject brentTextFinal; // everything is set up
    public GameObject victimTextFinal; //thank you, i'm gonna try to make a run for it
    public GameObject victimTextFinalTwo; //could you try to distract him, maybe find something that makes noise
    public GameObject brentTextFinalTwo; //i'm on it, if anything happens ill try protect you
    public GameObject victimeTextFinalThree;

    public bool isTalking = false;

    public GameObject victimHintText;

    public bool hasSaidSegmentOne;
    public bool hasSaidSegmentTwo;  
    public bool hasSaidSegmentThree;

    //vinyl stuff
    public GameObject hiddenVinyl;
    public GameObject placeText;
    public GameObject playText;
    public GameObject cantPlayText;
    public GameObject noRecordText;
    public AudioSource vinylPlayer;
    public AudioClip vinylSong;
    public GameObject brentCantPlayText;

    public bool hasRecord = false;
    public bool hasPlacedRecord = false;
    public bool canPlayRecord = false;

    public GameObject cainDummy;
    public GameObject realCain;
    public GameObject endCane;
    public bool safeFromCain = false;
    public bool reachedEnd = false;
    public bool beingHeldByCain = false;
    public AudioClip cainScreamSFX;

    public bool hasHeardCrying = false;

    //Player Camera Animator
    private Animator Pcamera;

    public GameObject eletricLights;
    public AudioClip powerDownSFX;
    public AudioClip electricZapSFX;
    public AudioSource powerOutageSounds;
    public int Severity = 5;
    public GameObject Killbox;

    public GameObject pickUpText2;

    public AudioClip cutsceneScream;
    public AudioSource cutscene;

    public GameObject UIcrosshair;
    public GameObject UIKeysTitle;
    public GameObject UIKeysnumbers;
    public GameObject UIBatteriesTitle;
    public GameObject UIBatteriesNumbers;
    public GameObject UIhour;
    public GameObject UIminutes;
    public GameObject UIdate;
    public GameObject UIScreenCrack;
    public AudioClip thumpSFX;
    public GameObject UIPM;
    public GameObject UIManagerThing;

    public GameObject theKnife;

    public GameObject flashlightThing1;
    public GameObject flashlightThing2;
    public GameObject flashlightThing3;
    
    private IEnumerator Crowbar()
    {
        crowbar.SetActive(true);
        crowBarAnimator.Play("crow", 0, 0.0f);
        yield return new WaitForSeconds(0.5f);
        crowbar.SetActive(false);   
    }
    private IEnumerator cutScene()
    {
        yield return new WaitForSeconds(4f);
        watchinCutsecene = false;
        StartCoroutine(PowerOutage());
        cutscene.Stop();
    }
    private IEnumerator FlickeringLight1()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight1.SetActive(false);
        flickeringLight5.SetActive(true);
        StartCoroutine(FlickeringLight2());
    }
    private IEnumerator FlickeringLight2()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight2.SetActive(false);
        flickeringLight1.SetActive(true);
        StartCoroutine(FlickeringLight3());
    }
    private IEnumerator FlickeringLight3()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight3.SetActive(false);
        flickeringLight2.SetActive(true);
        StartCoroutine(FlickeringLight4());
    }
    private IEnumerator FlickeringLight4()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight4.SetActive(false);
        flickeringLight3.SetActive(true);
        StartCoroutine(FlickeringLight5());
    }
    private IEnumerator FlickeringLight5()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight5.SetActive(false);
        flickeringLight4.SetActive(true);
        StartCoroutine(FlickeringLight1());
    }
    [SerializeField] private float raycastInterval = 0.5f;
    [SerializeField] private float sphereRadius = 5f;
    [SerializeField] private LayerMask raycastMask;
    private bool isRunning = false;

    private IEnumerator checkForCane()
    {
        if (_heldFlashlight == null)
        {
            Debug.LogError("Held flashlight is not assigned.");
            yield break;
        }

        while (flashlightOn)
        {
            // Create the ray from the flashlight's position and direction
            Ray ray = new Ray(_heldFlashlight.transform.position, _heldFlashlight.transform.forward);

            // Runtime visualization (approximate sphere using rays)
            DrawSphereVisualization(ray.origin, sphereRadius);

            // Perform a spherecast
            if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, 1000f, raycastMask))
            {
                Debug.Log($"Hit {hit.collider.name} at distance: {hit.distance}");

                if (hit.collider.CompareTag("Cane"))
                {
                    Debug.Log("CaneHit");
                    Severity = Mathf.Max(0, Severity - 1);
                }
            }
            else
            {
                Debug.Log("No hit detected.");
            }

            yield return new WaitForSeconds(raycastInterval);
        }
    }

// Approximate sphere visualization
    private void DrawSphereVisualization(Vector3 origin, float radius)
    {
        int segments = 12; // Number of rays to draw
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Debug.DrawRay(origin, offset, Color.green, 0.1f);
        }
    }

// Scene view visualization
    private void OnDrawGizmos()
    {
        if (_heldFlashlight != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_heldFlashlight.transform.position, sphereRadius);
        }
    }

    
    public void FixedUpdate()
    {
        if (Severity == 0)
        {
            Killbox.SetActive(true);
        }
    }

    private IEnumerator refreshtimer()
    {
        yield return new WaitForSeconds(20);
        Severity = 5;

    }
    
    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        _characterController = GetComponent<CharacterController>();
        StartCoroutine(FlickeringLight1());
          
        print("started flickering");
        StartCoroutine(checkForCane());

        FirstBattery = false;
    }
    
    public void Start()
    {
        StartCoroutine(StartControlsText());
        Cursor.visible = false;
        raycastMask = LayerMask.GetMask("Cane");
       
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

        //Subscribe to the Pause
        playerInput.Player.Pause.performed += ctx => Pause(); // pause the game

        // CameraShakeCompnent
        Pcamera = GetComponent<Animator>();
    }
    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        checkForPickup();
        cutTheFlashlight();
        

        if(isPaused == true)
        {
            Cursor.visible = true;
        }

        if(isPaused == false)
        {
            Cursor.visible = false;
        }
    }

    private void cutTheFlashlight()
    {
        if (battMan.InternalBatteryLevel <= 0)
        {
            heldFlashlightLight.enabled = false;
            flashlightOn = false;
            spriteMask.SetActive(false);
            UIman.DisplayFlashlightOFF();
            
            if (battMan.batteryLevel > 0)
            {
                battMan.fillInternalBatteryLevel();
                battMan.decreaseBatteryLevel();
            }
        }
    }

    private bool FlashlightWasON = false;
    private void pauseTheFlashlight()
    {
        heldFlashlightLight.enabled = false;
        flashlightOn = false;
        spriteMask.SetActive(false);
        UIman.DisplayFlashlightOFF();
    }

    private void unpauseTheFlashlight()
    {
        if (FlashlightWasON)
        {
            heldFlashlightLight.enabled = true;
            flashlightOn = true;
            spriteMask.SetActive(true);
            UIman.DisplayFlashlightON();
        }
    }
    public void Pause()
    {
        if(isPaused == false)
        {
            if (flashlightOn)
            {
                pauseTheFlashlight();
                FlashlightWasON = true;
            }
            
            isPaused = true;
            isOnMainScreen = true;
            pauseScreen.SetActive(true);
            mainScreen.SetActive(true);
            Time.timeScale = 0f;
            UnityEngine.Debug.Log("should pause");
            flySound1.SetActive(false);
            flySound2.SetActive(false);
            flySound3.SetActive(false);
            Cursor.visible = true;
        }
        
        else if(isPaused == true) 
        {
            if (FlashlightWasON)
            {
                unpauseTheFlashlight();
                FlashlightWasON = false;
            }
            isPaused = false;
            isOnMainScreen = false;
            isOnControlsScreen = false;
            pauseScreen.SetActive(false);
            controlsScreen.SetActive(false);
            mainScreen.SetActive(true);
            Time.timeScale = 1f;
            UnityEngine.Debug.Log("should unpause");
            flySound1.SetActive(true);
            flySound2.SetActive(true);
            flySound3.SetActive(true);
            Cursor.visible = false;
        }
    }
    public void Resume()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        controlsScreen.SetActive(false);
        mainScreen.SetActive(false);
        Time.timeScale = 1f;
        UnityEngine.Debug.Log("should unpause");
        flySound1.SetActive(true);
        flySound2.SetActive(true);
        flySound3.SetActive(true);
        Cursor.visible= false;
    }
    public void Controls()
    {
        if(isOnMainScreen == true)
        {
            isOnMainScreen = false;
            isOnControlsScreen= true;
            controlsScreen.SetActive(true);
            mainScreen.SetActive(false);
            Cursor.visible = true;
        }
    }

    public void Keyboard()
    {
        if(isOnControlsScreen == true)
        {
            isOnControlsScreen = false;
            isOnControlsScreenKeyboard = true;
            controlsScreen.SetActive(false);
            controlsScreenKeyboard.SetActive(true);
            Cursor.visible = true;
        }
    }

    public void Gamepad()
    {
        if(isOnControlsScreenKeyboard == true)
        {
            isOnControlsScreenKeyboard = false;
            isOnControlsScreen = true;
            controlsScreenKeyboard.SetActive(false);
            controlsScreen.SetActive(true);
            Cursor.visible = true;
        }
    }
    public void Back()
    {
        if(isOnControlsScreen == true)
        {
            isOnControlsScreen = false;
            controlsScreen.SetActive(false);
            mainScreen.SetActive(true);
            isOnMainScreen= true;
            mainScreen.SetActive(true);
            Cursor.visible=true;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    
    private void Move()
    { if (isPaused == false && beingHeldByCain == false && watchinCutsecene == false && isTalking == false)
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


    private Light heldFlashlightLight;

    private void LookAround()
    { if (isPaused == false && beingHeldByCain == false)
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
        if (_characterController.isGrounded && isPaused == false)
        {
            // Calculate the jump velocity
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Shoot()
    {
        if (_holdingGun != true) return;
        // Instantiate the projectile at the fire point
        else if (isPaused == false)
        {
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            worldSounds.clip = stungunSFX;
            worldSounds.Play();

            // Get the Rigidbody component of the projectile and set its velocity
            var rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 0.5f);
        }
    }

    private Coroutine batteryCoroutine;
    
    private void FlashlightSwitch()
    {
        if (isPaused) return;
    
        heldFlashlightLight = _heldFlashlight.GetComponent<Light>();
    
        if (flashlightOn)
        {
            if (batteryCoroutine != null)
            {
                StopCoroutine(batteryCoroutine);
                batteryCoroutine = null;
            }
            heldFlashlightLight.enabled = false;
            spriteMask.SetActive(false);
            flashlightOn = false;
        }
        else
        {
            if (!_holdingFlashlight || battMan.InternalBatteryLevel <= 0)
            {
                return;
            }
        
            heldFlashlightLight.enabled = true;
            //cubattMan.decreaseBatteryLevel();
            batteryCoroutine = StartCoroutine(battMan.depreciateInternalBatteryLevel());
            flashlightOn = true;
            spriteMask.SetActive(true);
            worldSounds.clip = flashlightSFX;
            worldSounds.Play();
        }
    
        FlashlightUIcontrol();
    }
    
    private void FlashlightUIcontrol()
    {
        if (flashlightOn)
        {
            UIman.DisplayFlashlightON();
        }
        else
        {
            UIman.DisplayFlashlightOFF();
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
        if (isPaused == false)
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
            flashlightThing1.SetActive(true);
            flashlightThing2.SetActive(true);
            flashlightThing3.SetActive(true);
            stungunUI.SetActive(false);

        }
        else if (_holdingFlashlight)
        {
            _holdingFlashlight = false;
            _holdingGun = true;

            flashlightUI.SetActive(false);
            flashlightThing1.SetActive(false);
            flashlightThing2.SetActive(false);
            flashlightThing3.SetActive(false);
            stungunUI.SetActive(true);
           
        }

    }
    public void CorrectSafeCombination()
    {
        rightCombination.SetActive(true);
        StartCoroutine(RightCombination());
        worldSounds.clip = correctSFX;
        worldSounds.Play();
        Destroy(safeText);
    }
    public void WrongSafeCombination()
    {
        wrongCombination.SetActive(true);
        StartCoroutine(WrongCombination());
        noteThreeCombinationText.SetActive(false);
        worldSounds.clip = incorrectSFX;
        worldSounds.Play();
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
                if (Grab != null)
                {
                    Grab.Play("Grab", 0, 0.0f);
                }

                Destroy(hit.collider.gameObject);
                //gotKey.SetActive(true);
                //StartCoroutine(ReceivedKey());
                keyManager.addKeyLevel();
                worldSounds.clip = keySFX;
                worldSounds.Play();

                hasUnlockedPageOne = true;
            }

            else if (hit.collider.CompareTag("RealKey"))
            {
                if (Grab != null)
                {
                    Grab.Play("Grab", 0, 0.0f);
                }

                Destroy(hit.collider.gameObject);
                //gotKey.SetActive(true);
                //StartCoroutine(ReceivedKey());
                keyManager.addKeyLevel();
                worldSounds.clip = keySFX;
                worldSounds.Play();

                //hasUnlockedPageOne = true;
            }

            else if (hit.collider.CompareTag("Battery"))
            {
                if (!FirstBattery)
                {
                    if (Grab != null)
                    {
                        Grab.Play("Grab", 0, 0.0f);
                    }
                    Destroy(hit.collider.gameObject);
                    //gotBattery.SetActive(true);
                    battMan.addBatteryLevel();

                    if (!_holdingGun && FoundFlashlight)
                    {
                        UIman.GotFirstBattery();
                    }

                    // StartCoroutine(ReceivedBattery());
                        worldSounds.clip = batterySFX;
                        worldSounds.Play();
                        hasUnlockedPageTwo = true;
                        FirstBattery = true;
                    
                    
                }
                else
                {
                    if (Grab != null)
                    {
                        Grab.Play("Grab", 0, 0.0f);
                    }
                    Destroy(hit.collider.gameObject);
                    //gotBattery.SetActive(true);
                    battMan.addBatteryLevel();

                    // StartCoroutine(ReceivedBattery());
                    worldSounds.clip = batterySFX;
                    worldSounds.Play();
                    hasUnlockedPageTwo = true;
                }
            }

            else if (hit.collider.CompareTag("RealBattery"))
            {
                if (!FirstBattery)
                {
                    if (Grab != null)
                    {
                        Grab.Play("Grab", 0, 0.0f);
                    }
                    Destroy(hit.collider.gameObject);
                    //gotBattery.SetActive(true);
                    battMan.addBatteryLevel();

                    if (!_holdingGun && FoundFlashlight)
                    {
                        UIman.GotFirstBattery();
                    }

                    // StartCoroutine(ReceivedBattery());
                    worldSounds.clip = batterySFX;
                    worldSounds.Play();
                    hasUnlockedPageTwo = true;
                    FirstBattery = true;
                    
                    
                }
                else
                {
                    if (Grab != null)
                    {
                        Grab.Play("Grab", 0, 0.0f);
                    }
                    Destroy(hit.collider.gameObject);
                    //gotBattery.SetActive(true);
                    battMan.addBatteryLevel();

                    // StartCoroutine(ReceivedBattery());
                    worldSounds.clip = batterySFX;
                    worldSounds.Play();
                    hasUnlockedPageTwo = true;
                }
            }
            else if (hit.collider.CompareTag("Door") && keyManager.keyLevel > 0.99)
            {
                hit.collider.gameObject.GetComponent<Animator>().Play("Open", 0, 0.0f);
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

            /*else if (hit.collider.CompareTag("Radio"))
            {
                Destroy(hit.collider.gameObject);
                StartCoroutine(CollectedEvidence());
                //StartCoroutine(EndChapter());
                collectedEvidence.SetActive(true);
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageTen = true;

                if (gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }
            }*/

            else if (hit.collider.CompareTag("Knife"))
            {
                if (Grab != null)
                {
                    Grab.Play("Grab", 0, 0.0f);
                }
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageFive = true;
                if (gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }
            }

            else if (hit.collider.CompareTag("Crowbar"))
            {
                if (Grab != null)
                {
                    Grab.Play("Grab", 0, 0.0f);
                }
                Destroy(hit.collider.gameObject);
                
                GotCrowbarText.SetActive(true);
                StartCoroutine(GotTheCrowbar());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                gotCrowbar = true;
                hasUnlockedPageSix = true;
                if (gotNotebook == true)
                {
                   notebookUpdateText.SetActive(true);
                }
            }

            else if (hit.collider.CompareTag("Plank") && gotCrowbar == true)
            {
                StartCoroutine(Crowbar());
                Destroy(hit.collider.gameObject);
                worldSounds.clip = plankSFX;
                worldSounds.Play();
            }

            else if (hit.collider.CompareTag("Plank") && gotCrowbar == false)
            {
                blockedDoor.SetActive(true);
                StartCoroutine(BlockedDoor());
                worldSounds.clip = blockedDoorSFX;
                worldSounds.Play();
            }

            else if (hit.collider.CompareTag("Note1"))
            {
                GotNoteText.SetActive(true);
                
                Destroy(hit.collider.gameObject);
                //noteOneCombination.SetActive(true);
                GotNoteText.SetActive(true);
                StartCoroutine(GotANote());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageSeven = true;
                if (gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }
            }

            else if (hit.collider.CompareTag("Note2"))
            {
                Destroy(hit.collider.gameObject);
                //noteTwoCombination.SetActive(true);
                GotNoteText.SetActive(true);
                StartCoroutine(GotANote());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageEight = true;
                if (gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }
            }

            else if (hit.collider.CompareTag("Note3"))
            {
                Destroy(hit.collider.gameObject);
                //noteThreeCombination.SetActive(true);
                GotNoteText.SetActive(true);
                StartCoroutine(GotANote());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageNine = true;
                if (gotNotebook == true)
                {
                    notebookUpdateText.SetActive(true);
                }
            }

            else if (hit.collider.CompareTag("Note4"))
            {
                Destroy(hit.collider.gameObject);
                GotNoteText.SetActive(true);
                StartCoroutine(GotANote());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageSix = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());

            }

            else if (hit.collider.CompareTag("Note5"))
            {
                Destroy(hit.collider.gameObject);
                GotNoteText.SetActive(true);
                StartCoroutine(GotANote());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageSeven = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());
            }

            else if (hit.collider.CompareTag("Note6"))
            {
                Destroy(hit.collider.gameObject);
                GotNoteText.SetActive(true);
                StartCoroutine(GotANote());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                //hasUnlockedPageEight = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());

            }

            else if (hit.collider.CompareTag("Note7"))
            {
                Destroy(hit.collider.gameObject);
                GotNoteText.SetActive(true);
                StartCoroutine(GotANote());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                hasUnlockedPageNine = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());
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
            }*/

            else if (hit.collider.CompareTag("Safe"))
            {
                safeMan.ShowKeypad();

                if (flashlightOn)
                {
                    pauseTheFlashlight();
                    FlashlightWasON = true;
                }
               //safeDoor.Play("SafeDoor", 0, 0.0f);
                /*rightCombination.SetActive(true);
                StartCoroutine(RightCombination());
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                Destroy(safeText);*/
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
                if (gotNotebook)
                {
                    notebookUpdateText.SetActive(true);
                }

            }
            else if (hit.collider.CompareTag("Flashlight"))
            {
                FoundFlashlight = true;
                if (!objectInHolster && holdingObject)
                {
                    Holster();
                }
                // Pick up the object
                pickup_and_Hold(hit.collider.gameObject);

                _heldFlashlight = _heldObject;
                _holdingFlashlight = true;

                if (_holdingFlashlight == true)

                {
                    if (battMan.batteryLevel < 1)
                    {
                        flashlightUI.SetActive(true);
                        UIman.DisplayJustGotFlashlight();
                        stungunUI.SetActive(false);
                    }
                    else
                    {
                        flashlightUI.SetActive(true);
                        UIman.DisplayFlashlightOFF();
                        stungunUI.SetActive(false);
                    }
                    
                    
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
                if (gotNotebook)
                {
                    notebookUpdateText.SetActive(true);
                }
            }

            //level 2 items

            else if (hit.collider.CompareTag("Wrench"))
            {
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                gotWrench = true;
                hasUnlockedPageThree = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());


            }

            else if (hit.collider.CompareTag("Lever"))
            {
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                gotLever = true;
                hasUnlockedPageFour = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());
            }
            else if (hit.collider.CompareTag("Fuse"))
            {
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                gotFuse = true;
                hasUnlockedPageTwo = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());
                
            }
            else if (hit.collider.CompareTag("Cog"))
            {
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                gotCog = true;
                hasUnlockedPageOne = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());
            }
            else if (hit.collider.CompareTag("Screwdriver"))
            {
                Destroy(hit.collider.gameObject);
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                gotScrewdriver = true;
                hasUnlockedPageFive = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());
            }
            else if (hit.collider.CompareTag("IronBars") && gotScrewdriver == true)
            {
                Destroy(hit.collider.gameObject);
                worldSounds.clip = metalOpened;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("IronBars") && gotScrewdriver == false)
            {
                blockedDoor.SetActive(true);
                StartCoroutine(BlockedDoor());
                worldSounds.clip = metalLocked;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotFuse == false && gotLever == false && gotCog == false && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotFuse == false && gotLever == false && gotCog == false && boxFixed == false && addedCog == false && addedFuse == false && addedLever == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotFuse == true && gotLever == false && gotCog == false && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotFuse == false && gotLever == true && gotCog == false && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotFuse == false && gotLever == false && gotCog == true && boxFixed == false )
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotFuse == true && gotCog == false && gotLever == false && boxFixed == false)
            {
                partAddedText.SetActive(true);
                StartCoroutine(PartAdded());
                fixText.SetActive(false);
                fuse.SetActive(true);
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                gotFuse = false;
                addedFuse = true;
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotLever == true && gotFuse == false && gotCog == false && boxFixed == false)
            {
                partAddedText.SetActive(true);
                StartCoroutine(PartAdded());
                fixText.SetActive(false);
                lever.SetActive(true);
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                gotLever = false;
                addedLever = true;
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotCog == true && gotFuse == false && gotLever == false && boxFixed == false)
            {
                partAddedText.SetActive(true);
                StartCoroutine(PartAdded());
                fixText.SetActive(false);
                cog.SetActive(true);
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                gotCog = false;
                addedCog = true;
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotCog == true && gotFuse == true && gotLever == true && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotCog == false && gotFuse == true && gotLever == true && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotCog == true && gotFuse == false && gotLever == true && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotCog == true && gotFuse == true && gotLever == false && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == false && gotCog == true && gotFuse == true && gotLever == true && boxFixed == false)
            {
                needPartsText.SetActive(true);
                StartCoroutine(NeedParts());
                fixText.SetActive(false);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotCog == false && gotFuse == true && gotLever == true && boxFixed == false)
            {
                partAddedText.SetActive(true);
                StartCoroutine(PartAdded());
                fixText.SetActive(false);
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                fuse.SetActive(true);
                lever.SetActive(true);
                gotFuse = false;
                gotLever = false;
                addedLever = true;
                addedFuse = true;
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotCog == true && gotFuse == false && gotLever == true && boxFixed == false)
            {
                partAddedText.SetActive(true);
                StartCoroutine(PartAdded());
                fixText.SetActive(false);
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                cog.SetActive(true);
                lever.SetActive(true);
                gotCog = false;
                gotLever = false;
                addedCog = true;
                addedLever = true;
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotCog == true && gotFuse == true && gotLever == false && boxFixed == false)
            {
                partAddedText.SetActive(true);
                StartCoroutine(PartAdded());
                fixText.SetActive(false);
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                fuse.SetActive(true);
                cog.SetActive(true);
                gotFuse = false;
                gotCog = false;
                addedFuse = true;
                addedCog = true;

            }


            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && gotCog == true && gotFuse == true && gotLever == true && boxFixed == false)
            {
                partAddedText.SetActive(true);
                StartCoroutine(PartAdded());
                fixText.SetActive(false);
                worldSounds.clip = correctSFX;
                worldSounds.Play();
                gotFuse = false;
                gotCog = false;
                gotLever = false;
                boxFixed = true;
                cog.SetActive(true);
                fuse.SetActive(true);
                lever.SetActive(true);
               
            }
            else if (hit.collider.CompareTag("FuseBox") && boxFixed == true && gotWrench == true && gotCog == false && gotFuse == false && gotLever == false)
            {
                
                Destroy(fixText);
                worldSounds.clip = electricZapSFX;
                worldSounds.Play();
                powerRestoredText.SetActive(true);
                partAddedText.SetActive(false);
                StartCoroutine (PowerRestored());
                lever.SetActive(false);
                leverDown.SetActive(true);
                powerOn = true;
                //switch on all lights
                eletricLights.SetActive(true);
                
            }
            else if (hit.collider.CompareTag("FuseBox") && gotWrench == true && addedCog == true && addedFuse == true && addedLever == true)
            {

                Destroy(fixText);
                partAddedText.SetActive(false);
                worldSounds.clip = electricZapSFX;
                worldSounds.Play();
                powerRestoredText.SetActive(true);
                StartCoroutine (PowerRestored());  
                lever.SetActive(false);
                leverDown.SetActive(true);
                powerOn = true;
                //switch on all lights
                eletricLights.SetActive (true);
            }

            else if (hit.collider.CompareTag("VictimDoor") && gotBigKey == false)
            {
                lockedDoor.SetActive(true);
                StartCoroutine(LockedDoor());
                worldSounds.clip = lockedDoorSFX;
                StartCoroutine(VictimHint());
                worldSounds.Play();
            }
            
            
            else if (hit.collider.CompareTag("VictimDoor") && gotBigKey == true)
            {
                Destroy(hit.collider.gameObject);
                worldSounds.clip = doorSFX;
                worldSounds.Play();
            }

            else if (hit.collider.CompareTag("BigKey"))
            {
                Destroy(hit.collider.gameObject);
                gotBigKey = true;
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());
                Destroy(stealText);
                hasUnlockedPageTen = true;
                notebookUpdateText.SetActive(true);
                StartCoroutine(NotebookUpdate());
            }

            else if (hit.collider.CompareTag("MoveableBookshelf"))
            {
                Destroy(hit.collider.gameObject);
                worldSounds.clip = moveSFX;
                worldSounds.Play();
                secondBookshelf.SetActive(true);
                Destroy(moveText);
                
            }
            
            else if(hit.collider.CompareTag("WayOut") && wayOutUnlocked == false)
            {
                Destroy(hit.collider.gameObject);
                worldSounds.clip = doorSFX;
                worldSounds.Play();
                wayOutOpened.SetActive(true);
                wayOutUnlocked = true;
                openedWayOutText.SetActive(true);
                StartCoroutine(openedWayOut());

            }

            else if (hit.collider.CompareTag("Victim") && hasSaidSegmentOne == false && hasSaidSegmentTwo == false && isTalking == false)
            {
                StartCoroutine(TextSegmentOne());
            }

            else if (hit.collider.CompareTag("Victim") && hasSaidSegmentOne == true && hasSaidSegmentTwo == false && isTalking == false)
            {
                StartCoroutine(TextSegmentTwo());
            }

            else if (hit.collider.CompareTag("Victim") && hasSaidSegmentTwo == true && powerOn == false && wayOutUnlocked == false && isTalking == false)
            {
                StartCoroutine(TextSegmentTwoRerun());
            }

            else if (hit.collider.CompareTag("Victim") && hasSaidSegmentTwo == true && powerOn == true && wayOutUnlocked == false && isTalking == false)
            {
                StartCoroutine(TextSegmentTwoRerunPower());
            }

            else if (hit.collider.CompareTag("Victim") && hasSaidSegmentTwo == true && powerOn == false && wayOutUnlocked == true && isTalking == false)
            {
                StartCoroutine(TextSegmentTwoRerunWayOut());
            }

            else if (hit.collider.CompareTag("Victim") && hasSaidSegmentTwo == true && powerOn == true && wayOutUnlocked == true && isTalking == false)
            { 
                StartCoroutine(FinalText());
            }

            else if (hit.collider.CompareTag("Disk"))
            {
                Destroy(hit.collider.gameObject);
                hasRecord = true;
                hasUnlockedPageEight = true;
                worldSounds.clip = evidenceSFX;
                worldSounds.Play();
                collectedEvidence.SetActive(true);
                StartCoroutine(CollectedEvidence());

            }

            else if(hit.collider.CompareTag("VinylPlayer") && hasRecord == false)
            {
                noRecordText.SetActive(true);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
                StartCoroutine(NoRecord());
            }
            else if(hit.collider.CompareTag("VinylPlayer") && canPlayRecord == false)
            {
                brentCantPlayText.SetActive(true);
                worldSounds.clip = incorrectSFX;
                worldSounds.Play();
                StartCoroutine(CantPlayRecord());
            }
            else if (hit.collider.CompareTag("VinylPlayer") && canPlayRecord == true)
            {
                hiddenVinyl.SetActive(true);
                vinylPlayer.Play();
                Destroy(placeText);
                Destroy(playText);
                realCain.SetActive(false);
                endCane.SetActive(true);
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

        if (objecttoHold.CompareTag("Gun"))
        {
            flashlightThing1.SetActive(false);
            flashlightThing2.SetActive(false);
            flashlightThing3.SetActive(false);
        }
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
    
    //private IEnumerator ReceivedKey()
    //{
    //    gotKey.SetActive(true);
    //    yield return new WaitForSeconds(1.5f);
    //    gotKey.SetActive(false);
    //}
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
    //private IEnumerator ReceivedBattery()
    //{
        
    //    yield return new WaitForSeconds(1.5f);
    //    gotBattery.SetActive(false);
    //    Debug.Log("coroutine started");    
    //}
    private IEnumerator CollectedEvidence()
    {
        yield return new WaitForSeconds(2);
        collectedEvidence.SetActive(false);
    }
    
    private IEnumerator GotANote()
    {
        yield return new WaitForSeconds(2);
        GotNoteText.SetActive(false);
    }
    
    private IEnumerator GotTheCrowbar()
    {
        yield return new WaitForSeconds(2);
        GotCrowbarText.SetActive(false);
    }
    
    private IEnumerator EndChapter()
    {
        yield return new WaitForSeconds(6);
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

    private IEnumerator NeedParts()
    {
        yield return new WaitForSeconds(1f);
        needPartsText.SetActive(false);
    }

    private IEnumerator PartAdded()
    {
        yield return new WaitForSeconds(1f);
        partAddedText.SetActive(false);
    }

    private IEnumerator PowerRestored()
    {
        yield return new WaitForSeconds(1f);
        powerRestoredText.SetActive(false);
    }

    private IEnumerator openedWayOut()
    {
        yield return new WaitForSeconds(1f);
        openedWayOutText.SetActive(false);
    }
    private IEnumerator TextSegmentOne()
    {
        talkText.SetActive(false);
        brentText1.SetActive(true);
        isTalking = true;
        yield return new WaitForSeconds(3f);
        brentText1.SetActive(false);
        victimText1.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        victimText1.SetActive(false);
        hasSaidSegmentOne = true;
        isTalking = false;
    }

    private IEnumerator TextSegmentTwo()
    {
        talkText.SetActive(false);
        victimText2.SetActive(true);
        isTalking = true;
        yield return new WaitForSeconds(3.5f);
        victimText2.SetActive(false);
        brentText2.SetActive(true);
        yield return new WaitForSeconds(3f);
        brentText2.SetActive(false);
        victimText3.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimText3.SetActive(false);
        victimText3cont.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimText3cont.SetActive(false);
        brentText3.SetActive(true);
        yield return new WaitForSeconds(3f);
        brentText3.SetActive(false);
        hasSaidSegmentTwo = true;
        isTalking = false;
    }

    private IEnumerator TextSegmentTwoRerun()
    {
        
        victimText3.SetActive(true);
        isTalking = true;
        yield return new WaitForSeconds(5f);
        victimText3.SetActive(false);
        victimText3cont.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimText3cont.SetActive(false);
        brentText3.SetActive(true);
        yield return new WaitForSeconds(3f);
        brentText3.SetActive(false);
        isTalking = false;
    }

    private IEnumerator TextSegmentTwoRerunPower()
    {

        brentTextPower.SetActive(true);
        isTalking = true;
        yield return new WaitForSeconds(3f);
        brentTextPower.SetActive(false);
        victimTextPower.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimTextPower.SetActive(false);
        brentText3.SetActive(true);
        yield return new WaitForSeconds(3f);
        brentText3.SetActive(false);
        isTalking = false;
    }

    private IEnumerator TextSegmentTwoRerunWayOut()
    {

        brentTextWayOut.SetActive(true);
        isTalking = true;
        yield return new WaitForSeconds(3f);
        brentTextWayOut.SetActive(false);
        victimTextWayOut.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimTextWayOut.SetActive(false);
        brentText3.SetActive(true);
        yield return new WaitForSeconds(3f);
        brentText3.SetActive(false);
        isTalking=false;
    }

    private IEnumerator FinalText()
    {
        brentTextFinal.SetActive(true);
        isTalking = true;
        yield return new WaitForSeconds(3f);
        brentTextFinal.SetActive(false);
        victimTextFinal.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimTextFinal.SetActive(false);
        victimTextFinalTwo.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimTextFinalTwo.SetActive(false);
        brentTextFinalTwo.SetActive(true);
        theKnife.SetActive(true);
        yield return new WaitForSeconds(4f);
        brentTextFinalTwo.SetActive(false);
        victimeTextFinalThree.SetActive(true);
        yield return new WaitForSeconds(5f);
        victimeTextFinalThree.SetActive(false);
        canPlayRecord = true;
        isTalking = false;
        realCain.SetActive(false);
    }

    private IEnumerator NoRecord()
    {
        yield return new WaitForSeconds(1f);
        noRecordText.SetActive(false);
    }

    private IEnumerator CantPlayRecord()
    {
        yield return new WaitForSeconds(3f);
        brentCantPlayText.SetActive(false);
    }

    private IEnumerator GameOver() 
    { 
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Game Over");
    }

    private IEnumerator NotebookUpdate()
    {
        yield return new WaitForSeconds(3f);
        notebookUpdateText.SetActive(false);
    }

    private IEnumerator VictimHint()
    {
        yield return new WaitForSeconds(2f);
        victimHintText.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        victimHintText.SetActive(false);
    }

    private IEnumerator PowerOutage()
    {
        yield return new WaitForSeconds(12f);
        eletricLights.SetActive(false);
        powerOutageSounds.clip = electricZapSFX;
        powerOutageSounds.Play();
        yield return new WaitForSeconds(0.7f);
        eletricLights.SetActive(true);
        yield return new WaitForSeconds(8f);
        eletricLights.SetActive(false);
        powerOutageSounds.clip = electricZapSFX;
        powerOutageSounds.Play();
        yield return new WaitForSeconds(0.5f);
        eletricLights.SetActive(true);
        yield return new WaitForSeconds(5f);
        eletricLights.SetActive(false);
        powerOutageSounds.clip = electricZapSFX;
        powerOutageSounds.Play();
        yield return new WaitForSeconds(1f);
        eletricLights.SetActive(true);
        yield return new WaitForSeconds(3f);
        eletricLights.SetActive(false);
        powerOutageSounds.clip = electricZapSFX;
        powerOutageSounds.Play();
        yield return new WaitForSeconds(0.5f);
        eletricLights.SetActive(true);
        yield return new WaitForSeconds(2f);
        eletricLights.SetActive(false);
        powerOutageSounds.clip = electricZapSFX;
        powerOutageSounds.Play();
        yield return new WaitForSeconds(0.6f);
        eletricLights.SetActive(true);
        yield return new WaitForSeconds(1f);
        eletricLights.SetActive(false);
        powerOutageSounds.clip = powerDownSFX;
        powerOutageSounds.Play();
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
                Debug.Log("key");
                pickupText.SetActive(true);
                pickUpText2.SetActive(true);
                StartCoroutine(TurnOffText());

            }

            if (hit.collider.CompareTag("RealKey"))
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

            else if (hit.collider.CompareTag("RealBattery"))
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

            else if (hit.collider.CompareTag("Note4"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Note5"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Note6"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Note7"))
            {
                collectText.SetActive(true);
            }

            /*else if (hit.collider.CompareTag("noteOneCombination"))
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
            }*/
            
            else if (hit.collider.CompareTag("Safe"))
            {
                //noteThreeCombinationText.SetActive(false);
                //noteTwoCombinationText.SetActive(false);
                //noteOneCombinationText.SetActive(false);
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

            else if (hit.collider.CompareTag("Wrench"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Lever"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Fuse"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("Cog"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("FuseBox"))
            {
                fixText.SetActive(true);
               // Tester.text = "Fixed";
                //Tester.text = "";
            }

            else if (hit.collider.CompareTag("Screwdriver"))
            {
                collectText.SetActive(true);
            }

            else if (hit.collider.CompareTag("IronBars"))
            {
                openText.SetActive(true);
            }
            else if (hit.collider.CompareTag("BigKey"))
            {
                stealText.SetActive(true);
                StartCoroutine(TurnOffText());
            }
            else if (hit.collider.CompareTag("VictimDoor"))
            {
                openText.SetActive(true);
            }
            else if (hit.collider.CompareTag("MoveableBookshelf"))
            {
               moveText.SetActive(true);
            }
            else if(hit.collider.CompareTag("WayOut") && wayOutUnlocked == false)
            {
                openText.SetActive(true);
            }
            else if (hit.collider.CompareTag("Victim"))
            {
                talkText.SetActive(true);
                StartCoroutine (TurnOffText());
            }
            else if (hit.collider.CompareTag("VinylPlayer"))
            {
                placeText.SetActive(true);
            }
            else if (hit.collider.CompareTag("VinylPlayer") && hasPlacedRecord == true)
            {
                playText.SetActive(true);
                placeText.SetActive(false);
            }
            else if (hit.collider.CompareTag("Disk"))
            {
                pickupText.SetActive(true);
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
                fixText.SetActive(false);
                moveText.SetActive(false);
                stealText.SetActive(false);
                talkText.SetActive(false);
                placeText.SetActive(false);
                playText.SetActive(false);
                pickUpText2.SetActive(false);

            }
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
    
    //ANIMATIONS
    [SerializeField] private Animator KillAnimator;
    [SerializeField] private Animator CameraAnimator;
    [SerializeField] private Animator CaneDrag;
    [SerializeField] private Animator VictemDrag;
    [SerializeField] private GameObject VictemRun;
    [SerializeField] private Animator RunAnimator;
    private bool watchinCutsecene = false;
    //[SerializeField] private GameObject Cain;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "screamTrigger" && hasHeardCrying == false) 
        {
            radioBox.clip = scream1;
            radioBox.Play();
            //Debug.Log("entered trigger
            hasHeardCrying = true;
        }

        if(other.tag == "FixTrigger")
        {
            fixText.SetActive(false);
        }

        if(other.tag == "doorTrigger")
        {
            Destroy(parentsRoom);
        }

        if (other.tag == "doorTrigger2")
        {
            Destroy(cainsRoom);
        }
        if(other.tag == "TalkTrigger")
        {
            talkText.SetActive(false);
            radioBox.Stop();
            hasHeardCrying = true;
        }
        if(other.tag == "VinylTrigger")
        {
            placeText.SetActive(false);
            playText.SetActive(false);
        }
        if(other.tag == "GetYourStuff")
        {
            gotNotebook = true;
            gotCrowbar = true;
            hasUnlockedPageOne = false;
            hasUnlockedPageTwo = false;
            hasUnlockedPageThree = false;
            hasUnlockedPageFour = false;
            hasUnlockedPageFive = false;
            hasUnlockedPageSix = false;
            hasUnlockedPageSeven = false;
            hasUnlockedPageEight = false;
            hasUnlockedPageNine = false;
            hasUnlockedPageTen = false;
        }
        if(other.tag == "KillingBox" && safeFromCain == false)
        {
            cainDummy.SetActive(true);
            
            //Animator CameraAnimator = GetComponentInChildren<Animator>();
            //CameraAnimator.enabled = true;
            //GetComponentInChildren<Animator>().Play("CameraFalling", 0, 0.0f);
            realCain.SetActive(false);
            endCane.SetActive(false);
            KillAnimator.Play("Dead", 0, 0.0f);
            StartCoroutine(GameOver());
            cutscene.clip = cainScreamSFX;
            cutscene.Play();
            beingHeldByCain = true;
        }
        if (other.tag == "EndKillingBox")
        {
            cainDummy.SetActive(true);
            CameraAnimator.enabled = true;
            GetComponentInChildren<Animator>().Play("CameraFalling", 0, 0.0f);
            endCane.SetActive(false);
            VictemRun.SetActive(true);
            RunAnimator.Play("Run", 0, 0.0f);
            StartCoroutine(EndChapter());
            //worldSounds.clip = cainScreamSFX;
            //worldSounds.Play();
            beingHeldByCain = true;
            StartCoroutine(CRTBreak());
        }
        if(other.tag == "VoidTrigger")
        {
            SceneManager.LoadScene("1.5");
        }

        if (other.tag == "DragTrigger")
        {
            realCain.SetActive(true);
            Destroy(other);
            watchinCutsecene = true;
            CaneDrag.Play("drag", 0, 0.0f);
            VictemDrag.Play("struggle", 0, 0.0f);
            StartCoroutine(cutScene());
            cutscene.Play();
        }
        
    }
    public BoxCollider parentsRoom;
    public BoxCollider cainsRoom;

    private IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(1f);
        pickUpText2.SetActive(false);
        stealText.SetActive(false);
        talkText.SetActive(false);
    }
    
    private IEnumerator CRTBreak()
    {
        yield return new WaitForSeconds(0.5f);
        UIBatteriesNumbers.SetActive(false);
        UIKeysnumbers.SetActive(false);
        UIKeysTitle.SetActive(false);
        UIBatteriesTitle.SetActive(false);
        UIcrosshair.SetActive(false);
        UIminutes.SetActive(false);
        UIhour.SetActive(false);
        UIdate.SetActive(false);
        UIPM.SetActive(false);
        UIManagerThing.SetActive(false);
        UIScreenCrack.SetActive(true);
        flashlightThing1.SetActive(false);
        flashlightThing2.SetActive(false);
        flashlightThing3.SetActive(false);
        cutscene.clip = thumpSFX;
        cutscene.Play();
    }
}


