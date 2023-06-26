using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BouncyObject : MonoBehaviour
{
    public float launchForce = 10f;
    [NonSerialized] public Vector3 launchDirection = Vector3.up;

    public UnityEvent OnSurfaceEnter;
    public UnityEvent OnSurfaceExit;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            OnSurfaceEnter?.Invoke();
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            OnSurfaceExit?.Invoke();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
