using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public GameObject sound;
    private void Awake()
    {
        
        DontDestroyOnLoad(sound);
    }
}
