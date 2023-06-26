using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOneShot : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip;
    public List<AudioClip> clips = new List<AudioClip>();
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if(clip is null || audioSource is null) return;
        audioSource.PlayOneShot(clip);
    }

    public void PlayRandom()
    {
        if(clip is null || audioSource is null) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Count)]);
    }
}
