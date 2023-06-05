using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class LevelMarker : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public UnityEvent onLevelStart;
    public UnityEvent onLevelEnd;
    public UnityEvent onMoveOutOfSpawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            onLevelEnd?.Invoke();
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            onMoveOutOfSpawnPoint?.Invoke();
        }
    }
    private void Start() {
        onLevelStart?.Invoke();
    }
}
