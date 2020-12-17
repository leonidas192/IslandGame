using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSoundPlayer : MonoBehaviour
{
    public AudioSource footStepAudioSource;
    public AudioClip footStepClip;
    float lastTime = 0;
    float duration;

    private void Start()
    {
        duration = footStepClip.length;
    }

    public void PlayFootStepSound()
    {
        if(lastTime == 0)
        {
            footStepAudioSource.PlayOneShot(footStepClip);
        }
        if(Time.time - lastTime >= duration)
        {
            lastTime = Time.time;
            footStepAudioSource.PlayOneShot(footStepClip);
        }
    }
}
