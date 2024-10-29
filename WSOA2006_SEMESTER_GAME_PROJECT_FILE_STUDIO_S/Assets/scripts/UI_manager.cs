using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_manager : MonoBehaviour
{
    public GameObject gunUiText;
    public GameObject flashlightUiText;
    
    //key text
    public GameObject gotKey;

    //battery text
    public GameObject gotBattery;
    
    //pick up text
    public GameObject pickupText;
    public GameObject collectText;
    public GameObject openText;
    
    //controls text
    public GameObject moveLookTMP;
    public GameObject jumpCrouchTMP;

    //ui images
    public GameObject flashlightUI;
    public GameObject stungunUI;
    
    //safe text
    public GameObject safeText;
    
    //pause screen stuff
    public bool isPaused = false;
    public bool isOnMainScreen = false;
    public bool isOnControlsScreen = false;
    public GameObject pauseScreen;
    public GameObject mainScreen;
    public GameObject controlsScreen;
    public GameObject flySound1;
    public GameObject flySound2;
    public GameObject flySound3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
     public void Pause()
    {
        if(isPaused == false)
        {
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
            isPaused = false;
            isOnMainScreen = false;
            isOnControlsScreen = false;
            pauseScreen.SetActive(false);
            controlsScreen.SetActive(false);
            mainScreen.SetActive(true);
            Time.timeScale = 1f;
            Debug.Log("should unpause");
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
        }
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void DisplayUsingFlashlight()
    {
        flashlightUI.SetActive(true);
        stungunUI.SetActive(false);
    }

    public void DisplayUsingStunGun()
    {
        flashlightUI.SetActive(false);
        stungunUI.SetActive(true);
    }
}
