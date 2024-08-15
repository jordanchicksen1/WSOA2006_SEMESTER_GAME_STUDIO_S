using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject purpleDoor1;
    public FirstPersonControls FirstPersonControls;
    void OnTriggerEnter (Collider other)
    {
        if(other.tag == "PurpleDoor1" && FirstPersonControls.hasPurpleItem == true)

        {
            Destroy( purpleDoor1 );
        }
    }
}
