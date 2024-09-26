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
        batteryLevel = batteryLevel + 1;
        batteryLevelText.text = batteryLevel.ToString();
    }

    public void decreaseBatteryLevel()
    {
        batteryLevel = batteryLevel - 1;
        batteryLevelText.text = batteryLevel.ToString();
    }
}
