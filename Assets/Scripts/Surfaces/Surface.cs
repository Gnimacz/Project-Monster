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
    public UnityEvent OnSurfaceEnter;
    public UnityEvent OnSurfaceExit;


    public void EnterSurface() {
        OnSurfaceEnter?.Invoke();
    }

    public void ExitSurface() { 
        OnSurfaceExit?.Invoke();
    }
}
