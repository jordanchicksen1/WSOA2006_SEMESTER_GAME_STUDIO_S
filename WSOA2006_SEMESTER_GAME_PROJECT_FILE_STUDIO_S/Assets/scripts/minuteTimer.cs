using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class minuteTimer : MonoBehaviour
{
    
    public TextMeshProUGUI minuteCounterText;
    public float timeSpent;
    public int minutes;
    public int seconds;
    

    private void Update()
    {
        timeSpent += Time.deltaTime;
          
        minutes = Mathf.FloorToInt(timeSpent / 60);
        seconds = Mathf.FloorToInt(timeSpent % 60);
        minuteCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
