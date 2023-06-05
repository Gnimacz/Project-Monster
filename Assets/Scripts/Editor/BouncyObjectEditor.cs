using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(BouncyObject))]
public class BouncyObjectEditor : Editor
{
    private BouncyObject bouncyObject;

    private void OnEnable()
    {
        bouncyObject = (BouncyObject)target;
    }

    private void OnSceneGUI()
    {
        bouncyObject.launchDirection = bouncyObject.transform.up;
        LaunchDirectionIndicator();
    }

    void LaunchDirectionIndicator()
    {
        Debug.DrawLine(
            bouncyObject.transform.position,
            bouncyObject.transform.position + bouncyObject.launchDirection * (bouncyObject.launchForce / 2), 
            Color.red);
    }

}
