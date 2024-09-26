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
    public void Play()
    {
        camera1.transform.position = new Vector3(762f, 1f, -579.9f);
        pageTurner.clip = page1;
        pageTurner.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void next1()
    {
        camera1.transform.position = new Vector3(2008.528f, 1f, -579.9f);
        pageTurner.clip = page1;
        pageTurner.Play();
    }

    public void next2()
    {
        StartCoroutine(sceneChange());
        pageTurner.clip = tvSFX;
        pageTurner.Play();
    }

    public IEnumerator sceneChange()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("dayna_fuckaround");
    }
}
