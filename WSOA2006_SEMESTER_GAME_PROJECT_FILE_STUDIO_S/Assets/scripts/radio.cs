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
        Debug.Log("coroutines started");
    }

    public IEnumerator firstTimer()
    {
        yield return new WaitForSeconds(120);
        radioBox.clip = scream2;  
        radioBox.Play();
        Debug.Log("first track");
    }

    public IEnumerator secondTimer()
    {
        yield return new WaitForSeconds(180);
        radioBox.clip = scream3;
        radioBox.Play();
        Debug.Log("second track");
    }

    public IEnumerator thirdTimer()
    {
        yield return new WaitForSeconds(240);
        radioBox.clip = scream1;
        radioBox.Play();
    }

    
}

