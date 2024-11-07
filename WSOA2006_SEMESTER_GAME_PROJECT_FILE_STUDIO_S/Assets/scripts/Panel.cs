using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public GameObject door;
    public batteryManager batteryManager;
   public bool doorDestroyed = false;
    public AudioSource worldsounds;
    public AudioClip metalDoor;  

     
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") //&& batteryManager.batteryLevel > 0.99 && doorDestroyed == false)
        {
            door.SetActive(false);
            //batteryManager.decreaseBatteryLevel();
            doorDestroyed = true;
            worldsounds.clip = metalDoor;
            worldsounds.Play();
            Debug.Log("it hit");
        }
    }
}
