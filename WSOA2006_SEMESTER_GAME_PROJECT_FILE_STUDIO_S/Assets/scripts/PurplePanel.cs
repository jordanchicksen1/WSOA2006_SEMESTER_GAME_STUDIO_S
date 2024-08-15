using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurplePanel : MonoBehaviour
{
    public GameObject door;
    public batteryManager batteryManager;
    public bool doorDestroyed = false;
    public FirstPersonControls firstPersonControls;

    void OnCollisionEnter(Collision other)
    {//if the player shoots the panel near the door, the door will be destroyed
        if (other.gameObject.CompareTag("Bullet") && batteryManager.batteryLevel > 0.99 && doorDestroyed == false && firstPersonControls.hasPurpleUpgrade ==  true)
        {
            Destroy(door);
            batteryManager.decreaseBatteryLevel();
            doorDestroyed = true;
            
        }
    }
}
