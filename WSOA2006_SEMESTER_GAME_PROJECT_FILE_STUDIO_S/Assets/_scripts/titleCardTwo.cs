using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleCardTwo : MonoBehaviour
{
    public GameObject chpTwoTitle;
    public GameObject theEncounterTitle;
    public AudioSource AudioSource;
    public AudioClip bell;
    public void Start()
    {
        StartCoroutine(chpTwoText());
        StartCoroutine(theEncounterText());
        StartCoroutine(startChpTwo());
    }

    private IEnumerator chpTwoText()
    {
        yield return new WaitForSeconds(1f);
        chpTwoTitle.SetActive(true);
        AudioSource.clip = bell;
        AudioSource.Play();
    }

    private IEnumerator theEncounterText()
    {
        yield return new WaitForSeconds(3f);
        theEncounterTitle.SetActive(true);
        AudioSource.clip = bell;
        AudioSource.Play();
    }

    private IEnumerator startChpTwo()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("ChapterTwo");
    }
}
