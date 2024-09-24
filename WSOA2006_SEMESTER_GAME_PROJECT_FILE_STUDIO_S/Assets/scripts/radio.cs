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
    }

    public IEnumerator firstTimer()
    {
        yield return new WaitForSeconds(75);
        radioBox.clip = scream2;  
        radioBox.Play();
    }

    public IEnumerator secondTimer()
    {
        yield return new WaitForSeconds(135);
        radioBox.clip = scream3;
        radioBox.Play();
    }

    public IEnumerator thirdTimer()
    {
        yield return new WaitForSeconds(195);
        radioBox.clip = scream1;
        radioBox.Play();
    }
}
