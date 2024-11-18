using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPanel : MonoBehaviour
{
    public GameObject door;
    public AudioSource worldsounds;
    public AudioClip metalDoor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            door.SetActive(false);
            worldsounds.clip = metalDoor;
            worldsounds.Play();

        }
    }
}
