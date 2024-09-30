using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class minuteTimer : MonoBehaviour
{
    public int minuteCounter;
    public TextMeshProUGUI minuteCounterText;
    void Start()
    {
        StartCoroutine(firstMinute());
    }

    public IEnumerator firstMinute()
    {
        yield return new WaitForSeconds(60);
        minuteCounter = minuteCounter + 1;
        minuteCounterText.text = minuteCounter.ToString();
        StartCoroutine(secondMinute());
    }

    public IEnumerator secondMinute()
    {
        yield return new WaitForSeconds(60);
        minuteCounter = minuteCounter + 1;
        minuteCounterText.text = minuteCounter.ToString();
    }
}
