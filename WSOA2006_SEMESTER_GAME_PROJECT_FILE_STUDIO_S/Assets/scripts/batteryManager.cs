using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class batteryManager : MonoBehaviour
{
    public int batteryLevel;
    public Text batteryLevelText;

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
