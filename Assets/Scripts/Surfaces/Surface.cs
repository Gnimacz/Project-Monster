using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Surface : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform Collision;
    public Vector3 normal;
    public bool invertNormal;
    // we need a default value for this one, it might be beneficial to have a place where the designers can change the default valur but rn it doesn't seem worth it
    public float edgeCapRadius = 1f;
    public UnityEvent OnSurfaceEnter;
    public UnityEvent OnSurfaceExit;


    public void EnterSurface() {
        OnSurfaceEnter?.Invoke();
    }

    public void ExitSurface() { 
        OnSurfaceExit?.Invoke();
    }
}
