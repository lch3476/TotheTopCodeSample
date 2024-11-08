using System.Collections;
using System.Collections.Generic;
using Cinemachine.PostFX;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [SerializeField] [Range(0.0f, 1.0f)] float volume = 1.0f;

    public void PlaySoundClipAtMainCamera(AudioClip _clip)
    {
        if (_clip != null)
        {
            AudioSource.PlayClipAtPoint(
                _clip,
                Camera.main.transform.position,
                volume);
        }
    }
}
