using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip ambientSound;
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float minVolume = 0.2f;
    [SerializeField] private float fadeSpeed = 0.5f;

    private float targetVolume = 0f;
    private bool isPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ambientSound;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = minVolume;
        }
    }

    private void Update()
    {
        if (Mathf.Abs(audioSource.volume - targetVolume) > 0.01f)
        {
            isPlaying = true;
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
        }
        else
        {
            if (isPlaying)
            {
                isPlaying = false;
                audioSource.volume = targetVolume;
            }
        }
    }
}
