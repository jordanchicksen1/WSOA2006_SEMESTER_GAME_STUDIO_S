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
    public void Play()
    {
        
        pageTurner.clip = page1;
        pageTurner.Play();
        camera1.transform.position = new Vector3(762f, 1f, -579.9f);
    }

    public void Begin()
    {
        StartCoroutine(beginTimer());   
        pageTurner.clip = beginSFX;
        pageTurner.Play();
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
    {
        StartCoroutine(sceneChange());
        pageTurner.clip = tvSFX;
        pageTurner.Play();
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
    {
        StartCoroutine(ChapterOneSelect());
        pageTurner.clip = tvSFX;
        pageTurner.Play();
        spotLight1.SetActive(true);
    }
}
