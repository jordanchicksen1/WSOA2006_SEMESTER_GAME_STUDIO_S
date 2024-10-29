using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    //sound effects general
    public AudioSource worldSounds;
    public AudioClip keySFX;
    public AudioClip batterySFX;
    public AudioClip flashlightSFX;
    public AudioClip doorSFX;
    public AudioClip stungunSFX;
    public AudioClip evidenceSFX;
    public AudioClip lockedDoorSFX;
    public AudioClip pageSFX;
    public AudioClip plankSFX;
    public AudioClip blockedDoorSFX;
    public AudioClip incorrectSFX;
    public AudioClip correctSFX;
    
    public GameObject sound;
    private void Awake()
    {
        DontDestroyOnLoad(sound);
    }
    public void playKeySFX()
    {
        worldSounds.clip = keySFX;
        worldSounds.Play();
    }
    
    public void playBatterySFX()
    {
        worldSounds.clip = batterySFX;
        worldSounds.Play();
    }
    
    public void playFlashlightSFX()
    {
        worldSounds.clip = flashlightSFX;
        worldSounds.Play();
    }
    
    public void playDoorSFX()
    {
        worldSounds.clip = doorSFX;
        worldSounds.Play();
    }
    
    public void playStunGunSFX()
    {
        worldSounds.clip = stungunSFX;
        worldSounds.Play();
    }
    
    public void playEvidenceSFX()
    {
        worldSounds.clip = evidenceSFX;
        worldSounds.Play();
    }
    
    public void playLockedDoorSFX()
    {
        worldSounds.clip = lockedDoorSFX;
        worldSounds.Play();
    }
    
    public void playPageSFX()
    {
        worldSounds.clip = pageSFX;
        worldSounds.Play();
    }
    
    public void playPlankSFX()
    {
        worldSounds.clip = plankSFX;
        worldSounds.Play();
    }
    
    public void playBlockedDoorSFX()
    {
        worldSounds.clip = blockedDoorSFX;
        worldSounds.Play();
    }
    
    public void playIncorrectSFX()
    {
        worldSounds.clip = incorrectSFX;
        worldSounds.Play();
    }
    
    public void playCorrectSFX()
    {
        worldSounds.clip = correctSFX;
        worldSounds.Play();
    }
}
