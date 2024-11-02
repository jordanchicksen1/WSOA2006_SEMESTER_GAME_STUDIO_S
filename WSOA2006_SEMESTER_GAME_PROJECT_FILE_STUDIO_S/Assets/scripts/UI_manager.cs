using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    //got em text
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
    
    //evidence related stuff
    public bool gotEvidence1 = false;
    public bool gotEvidence2 = false;
    public GameObject collectedEvidence;

    //locked door stuff
    public GameObject lockedDoor;
    public GameObject blockedDoor;
    
    //controls text
    public GameObject moveLookTMP;
    public GameObject jumpCrouchTMP;

    //ui images
    public GameObject flashlightONui;
    public GameObject flashlightOFFui;
    public GameObject flashlightFillui;
    
    public GameObject stungunUI;
    
    
    //safe text
    public GameObject safeText;
    public GameObject noteOneCombinationText;
    public GameObject noteTwoCombinationText;
    public GameObject noteThreeCombinationText;
    public GameObject wrongCombination;
    public GameObject rightCombination;
    
    //pause screen stuff
    public bool isPaused = false;
    public bool isOnMainScreen = false;
    public bool isOnControlsScreen = false;
    
    public GameObject pauseScreen;
    public GameObject mainScreen;
    public GameObject controlsScreen;
    
    //switch text
    public GameObject switchText;
    
    //notebook
    public GameObject notebookText;
    public GameObject notebookUpdateText;

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
        if(!isPaused)
        {
            isPaused = true;
            isOnMainScreen = true;
            pauseScreen.SetActive(true);
            mainScreen.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("should pause");
            Cursor.visible = true;
        }
        
        else if(isPaused) 
        {
            isPaused = false;
            isOnMainScreen = false;
            isOnControlsScreen = false;
            pauseScreen.SetActive(false);
            controlsScreen.SetActive(false);
            mainScreen.SetActive(true);
            Time.timeScale = 1f;
            Debug.Log("should unpause");
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

    public void DisplayFlashlightOFF()
    {
        flashlightOFFui.SetActive(true);
        flashlightFillui.SetActive(true);
        flashlightONui.SetActive(false);
        stungunUI.SetActive(false);
    }

    public void DisplayJustGotFlashlight()
    {
        flashlightOFFui.SetActive(true);
        //flashlightFillui.SetActive(true);
        flashlightONui.SetActive(false);
        stungunUI.SetActive(false);
    }
    public void DisplayFlashlightON()
    {
        flashlightONui.SetActive(true);
        flashlightFillui.SetActive(true);
        flashlightOFFui.SetActive(false);
        stungunUI.SetActive(false);
    }

    public void GotFirstBattery()
    {
        flashlightFillui.SetActive(true);
    }

    public void decreaseFlashFill()
    {
        flashlightFillui.GetComponent<Image>().fillAmount = flashlightFillui.GetComponent<Image>().fillAmount - 0.01f;
    }

    public void fillFlashlight()
    {
        flashlightFillui.GetComponent<Image>().fillAmount = 1;
    }

    public void DisplayUsingStunGun()
    {
        flashlightOFFui.SetActive(false);
        flashlightONui.SetActive(false);
        stungunUI.SetActive(true);
    }

    public void NotebookUpdated()
    {
        notebookUpdateText.SetActive(true);
    }

    public void CutThePickUpPrompts()
    {
        pickupText.SetActive(false);
        collectText.SetActive(false);
        openText.SetActive(false);
        noteOneCombinationText.SetActive(false);
        noteTwoCombinationText.SetActive(false);
        noteThreeCombinationText.SetActive(false);
        safeText.SetActive(false);
    }
    
    public void ShowPickupText()
    {
        pickupText.SetActive(true);
    }

    public void ShowOpenText()
    {
        openText.SetActive(true);
    }

    public void ShowCollectText()
    {
        collectText.SetActive(true);
    }

    public void ShowSafeText()
    {
        safeText.SetActive(true);
    }
    
    public IEnumerator ReceivedKey()
    {
        gotKey.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        gotKey.SetActive(false);
    }
    public IEnumerator WrongCombination()
    {
        wrongCombination.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        wrongCombination.SetActive(false);
    }
    public IEnumerator RightCombination()
    {
        rightCombination.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        rightCombination.SetActive(false);
    }
    public IEnumerator ReceivedBattery()
    {
        gotBattery.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        gotBattery.SetActive(false);
    }
    public IEnumerator CollectedEvidence()
    {
        collectedEvidence.SetActive(true);
        yield return new WaitForSeconds(2);
        collectedEvidence.SetActive(false);
    }
    
    public IEnumerator LockedDoor()
    {
        lockedDoor.SetActive(true);
        yield return new WaitForSeconds(1f);
        lockedDoor.SetActive(false);
    }
    public IEnumerator BlockedDoor()
    {
        blockedDoor.SetActive(true);
        yield return new WaitForSeconds(1f);
        blockedDoor.SetActive(false);
    }
    public IEnumerator FlashlightText()
    {
        flashlightUiText.SetActive(true);
        yield return new WaitForSeconds(5f);
        flashlightUiText.SetActive(false);
    }
    public IEnumerator NotebookText()
    {
        notebookText.SetActive(true);
        yield return new WaitForSeconds(5f);
        notebookText.SetActive(false);
    }
    public IEnumerator StunGunText()
    {
        gunUiText.SetActive(true);
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
}
