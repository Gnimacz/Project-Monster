using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlValues : MonoBehaviour
{
    public static ControlValues Instance { get; private set; }

    public enum ClimbOrientation
    {
        UpDown,
        LeftRight
    }
    
    public Vector3 currentClimbStart;
    public Vector3 currentClimbEnd;
    public ClimbOrientation currentClimbOrientation = ClimbOrientation.LeftRight;
    public Vector3 currentSlideStart;
    public Vector3 currentSlideEnd;
    public Vector3 currentSlideDirection;
    public Vector3 lastCheckpoint;
    public Vector3 currentSurfaceNormal;
    public List<Vector3> checkpointBacklog = new List<Vector3>();
    public float lastGroundedTime;
    public float lastClimbingTime;
    public Quaternion targetMeshRotation;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy(gameObject);
            Instance = this;
        }
    }
}
