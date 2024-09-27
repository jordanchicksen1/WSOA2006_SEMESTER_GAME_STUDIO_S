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
}
