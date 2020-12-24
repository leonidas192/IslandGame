using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSoundPlayer : MonoBehaviour
{
    public AudioSource footStepAudioSource;
    public AudioClip footStepClip, waterStepClip;
    private AudioClip currentStepSound;
    float lastTime = 0;
    float duration;

    private void Start()
    {
        duration = footStepClip.length;
        currentStepSound = footStepClip;
    }

    public void PlayFootStepSound()
    {
        if(lastTime == 0)
        {
            footStepAudioSource.PlayOneShot(currentStepSound);
        }
        if(Time.time - lastTime >= duration)
        {
            lastTime = Time.time;
            footStepAudioSource.PlayOneShot(currentStepSound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "water")
        {
            currentStepSound = waterStepClip;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            currentStepSound = footStepClip;
        }
    }
}
