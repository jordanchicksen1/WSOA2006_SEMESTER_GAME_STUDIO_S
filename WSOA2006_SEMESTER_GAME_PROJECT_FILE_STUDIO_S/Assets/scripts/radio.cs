using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radio : MonoBehaviour
{
    public AudioSource radioBox;
    public AudioClip scream1;
    public AudioClip scream2;
    public AudioClip scream3;
    void Start()
    {
        StartCoroutine(firstTimer());
        StartCoroutine(secondTimer());
        StartCoroutine(thirdTimer());
        StartCoroutine(firstScream());
        Debug.Log("coroutines started");
    }

    public IEnumerator firstTimer()
    {
        yield return new WaitForSeconds(132);
        radioBox.clip = scream2;  
        radioBox.Play();
        Debug.Log("first track");
    }

    public IEnumerator secondTimer()
    {
        yield return new WaitForSeconds(192);
        radioBox.clip = scream3;
        radioBox.Play();
        Debug.Log("second track");
    }

    public IEnumerator thirdTimer()
    {
        yield return new WaitForSeconds(252);
        radioBox.clip = scream1;
        radioBox.Play();
    }


    public IEnumerator firstScream()
    {
        yield return new WaitForSeconds(12);
        radioBox.clip = scream1;
        radioBox.Play();
    }

}

