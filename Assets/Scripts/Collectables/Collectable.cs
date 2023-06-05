using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    public bool shouldDestroyOnCollect = true;
    public AudioClip collectSound;
    public UnityEvent onCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (/*other.CompareTag("Player")*/ other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collectSound != null) AudioSource.PlayClipAtPoint(collectSound, transform.position);
            onCollect?.Invoke();
            gameObject.SetActive(!shouldDestroyOnCollect);
            Debug.Log("Collected");
        }
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         if(collectSound is not null) AudioSource.PlayClipAtPoint(collectSound, transform.position);
    //         onCollect?.Invoke();
    //         gameObject.SetActive(false);
    //         Debug.Log("Collected");
    //     }
    // }

}
