using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSFXManager : MonoBehaviour
{
    AudioSource audioSource;

    void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySingleClip(in AudioClip _clip)
    {
        
        if (_clip && _clip.length > 0 && !audioSource.isPlaying)
        {
            audioSource.clip = _clip;
            audioSource.Play();
        }
    }

    public void PlayClipsRandom(in AudioClip[] _clips)
    {
        if (_clips.Length > 0 && !audioSource.isPlaying)
        {
            AudioClip nextClip = _clips[Random.Range(0, _clips.Length)];

            if (nextClip && nextClip.length > 0)
            {
                audioSource.clip = nextClip;
                audioSource.Play();
            }
        }
    }

    public void StopPlaying()
    {
        audioSource.Stop();
    }
}
