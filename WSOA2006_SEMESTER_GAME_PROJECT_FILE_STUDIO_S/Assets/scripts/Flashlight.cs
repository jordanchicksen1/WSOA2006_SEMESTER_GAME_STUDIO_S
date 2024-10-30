using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float InternalBatteryLevel = 100;
    private Coroutine batteryLevelCo;
    public bool ON = false;
    public UI_manager ui_manager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ON && batteryLevelCo == null)
        {
            batteryLevelCo = StartCoroutine(depreciateInternalBatteryLevel());
            ui_manager.decreaseFlashFill();
            
        }
        else if(!ON && batteryLevelCo != null)
        {
            StopCoroutine(batteryLevelCo);
            batteryLevelCo = null;
        }
    }
    
    private IEnumerator depreciateInternalBatteryLevel()
    {
        while(InternalBatteryLevel > 0)
        {
            InternalBatteryLevel--;
            
            yield return new WaitForSeconds(1f);
            
            Debug.Log(InternalBatteryLevel);
        }
        Debug.Log("Stop");
        batteryLevelCo = null;
    }
}
