using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sounds : MonoBehaviour
{
    enum SoundCases { gameProcess, manSound, horsSound }

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGameSound()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == "go")
            {
                audioSource.clip = clip;
                audioSource.Play();
                audioSource.Play(4000);
            }
        }
    }

    public void KoyotSound ()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == "koyout")
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void ManCrashSound()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == "man-noise")
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void LeavesNoise()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == "leaves-noise")
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void WindNoise()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == "wind-noise")
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}
