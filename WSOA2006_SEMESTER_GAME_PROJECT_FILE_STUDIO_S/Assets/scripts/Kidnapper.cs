using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kidnapper : MonoBehaviour
{
    public GameObject kidnapper;
    public batteryManager batteryManager;
   

    void OnCollisionEnter(Collision other)
    {//if the player shoots the panel near the door, the door will be destroyed
        if (other.gameObject.CompareTag("Bullet") && batteryManager.batteryLevel > 0.99)
        {
            Destroy(kidnapper);
            batteryManager.decreaseBatteryLevel();
            Debug.Log("Apprehended Kidnapper!");
            SceneManager.LoadScene("End Screen");
            

        }
    }

   
}
