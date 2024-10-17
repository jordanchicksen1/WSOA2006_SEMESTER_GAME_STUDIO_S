using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startScreen : MonoBehaviour
{
    public GameObject camera1;
    public AudioSource pageTurner;
    public AudioClip page1;
    public AudioClip tvSFX;
    public AudioClip beginSFX;

    //title screen positio info x:-452.03 y:1 z:-579.9

    //chapterSpotLights
    public GameObject spotLight1;
    public GameObject spotLight2;
    public GameObject spotLight3;

    //rain particle effect
    public GameObject rain;

    //bools so that people don't press the buttons multiple times
    public bool notPressedBegin = true;
    public bool notPressedPlay = true;
    public bool notPressedChpOne = true;
    public bool notPressedChpTwo = true;
    public void Play()
    {
        
        pageTurner.clip = page1;
        pageTurner.Play();
        camera1.transform.position = new Vector3(762f, 1f, -579.9f);
    }

    public void Begin()
    {
        if (notPressedBegin == true)
        {
            StartCoroutine(beginTimer());
            pageTurner.clip = beginSFX;
            pageTurner.Play();
            notPressedBegin = false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Replay()
    {
        SceneManager.LoadScene("dayna_fuckaround");
    }

    public void next1()
    {
        
        pageTurner.clip = page1;
        pageTurner.Play();
        camera1.transform.position = new Vector3(2008.528f, 1f, -579.9f);
    }

    public void next2()
    { if (notPressedPlay == true)
        {
            StartCoroutine(sceneChange());
            pageTurner.clip = tvSFX;
            pageTurner.Play();
            notPressedPlay = false;
        }
    }

    public IEnumerator sceneChange()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("dayna_fuckaround");
    }

    public IEnumerator beginTimer()
    {
        yield return new WaitForSeconds(3f);
        camera1.transform.position = new Vector3(-451.7f, -710f, -579.9f);
        rain.SetActive(false);
    }

    public void chapterSelect()
    {
        camera1.transform.position = new Vector3(2010f, -712f, -579.9f);
        pageTurner.clip = page1;
        pageTurner.Play();
    }

    public IEnumerator ChapterOneSelect()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("dayna_fuckaround");
    }

    public IEnumerator ChapterTwoSelect()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ChapterTwo");
    }

    public void chapterOne()
    { if (notPressedChpOne == true)
        {
            StartCoroutine(ChapterOneSelect());
            pageTurner.clip = tvSFX;
            pageTurner.Play();
            spotLight1.SetActive(true);
            notPressedChpOne = false;
        }
    }

    public void chapterTwo()
    {
        if (notPressedChpTwo == true)
        {
            pageTurner.clip = tvSFX;
            pageTurner.Play();
            StartCoroutine(ChapterTwoSelect());
            spotLight2.SetActive(true);
            notPressedChpTwo = false;

        }
    }
    public void Back()
    {
        camera1.transform.position = new Vector3(-451.7f, -710f, -579.9f); 
        pageTurner.clip = page1;
        pageTurner.Play();
    }

    public void BackFromChpTwo()
    {
        camera1.transform.position = new Vector3(2010f, -712f, -579.9f);
        pageTurner.clip = page1;
        pageTurner.Play();
    }
}
