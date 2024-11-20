using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class batteryManager : MonoBehaviour
{
    public int batteryLevel;
    public TextMeshProUGUI batteryLevelText;

    public float InternalBatteryLevel = 100;
    public UI_manager ui_manager;
    public void addBatteryLevel()
    {
        batteryLevel ++;
        batteryLevelText.text = batteryLevel.ToString();
    }

    public void fillInternalBatteryLevel()
    {
        ui_manager.fillFlashlight();
        InternalBatteryLevel = 100;
    }

    public void decreaseBatteryLevel()
    {
        batteryLevel = Mathf.Clamp(batteryLevel - 1, 0, int.MaxValue);
        batteryLevelText.text = batteryLevel.ToString();
    }
    
    public IEnumerator depreciateInternalBatteryLevel()
    {
        while(InternalBatteryLevel > 0)
        {
            InternalBatteryLevel--;
            ui_manager.decreaseFlashFill();
            yield return new WaitForSeconds(1f);
            
        }
    }
}
