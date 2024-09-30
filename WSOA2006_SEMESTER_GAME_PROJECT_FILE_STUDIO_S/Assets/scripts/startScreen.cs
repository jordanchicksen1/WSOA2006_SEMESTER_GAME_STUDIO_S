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

    //bools so that people don't press the buttons multiple times
    public bool notPressedBegin = true;
    public bool notPressedPlay = true;
    public bool notPressedChpOne = true;
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

    public void Back()
    {
        camera1.transform.position = new Vector3(-451.7f, -710f, -579.9f); ;
        pageTurner.clip = page1;
        pageTurner.Play();
    }
}
