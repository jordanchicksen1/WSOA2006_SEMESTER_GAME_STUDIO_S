using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleCards : MonoBehaviour
{

    public GameObject chpOneTitle;
    public GameObject theHouseTitle;
    public AudioSource AudioSource;
    public AudioClip bell;
    public void Start()
    {
        StartCoroutine(chpOneText());
        StartCoroutine(theHouseText());
        StartCoroutine(startChpOne());
    }

    private IEnumerator chpOneText()
    {
        yield return new WaitForSeconds(1f);
        chpOneTitle.SetActive(true);
        AudioSource.clip = bell;
        AudioSource.Play();
    }

    private IEnumerator theHouseText()
    {
        yield return new WaitForSeconds(3f);
        theHouseTitle.SetActive(true);
        AudioSource.clip = bell;
        AudioSource.Play();
    }

    private IEnumerator startChpOne()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("dayna_fuckaround");
    }
}
