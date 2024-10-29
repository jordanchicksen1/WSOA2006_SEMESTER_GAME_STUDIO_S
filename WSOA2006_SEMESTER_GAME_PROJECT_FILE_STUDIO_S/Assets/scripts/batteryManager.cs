using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class batteryManager : MonoBehaviour
{
    public int batteryLevel;
    public TextMeshProUGUI batteryLevelText;

    public void addBatteryLevel()
    {
        batteryLevel ++;
        batteryLevelText.text = batteryLevel.ToString();
    }

    public void decreaseBatteryLevel()
    {
        batteryLevel --;
        batteryLevelText.text = batteryLevel.ToString();
    }
}
