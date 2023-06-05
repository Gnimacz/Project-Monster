using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(SlideSurface))]
public class SlideSurfaceEditor : Editor
{
    private SlideSurface slideSurface;

    private void OnEnable()
    {
        slideSurface = (SlideSurface)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Apply"))
        {
            Apply();
        }
    }

    public void OnSceneGUI()
    {
        if (slideSurface is null)
            return;
        
        StartEndHandles();
        NormalIndicator();
    }
    
    void NormalIndicator()
    {
        Vector3 center = (slideSurface.startPoint.position + slideSurface.endPoint.position) / 2;
        Debug.DrawLine(center, center + slideSurface.normal * 3, Color.red);
    }
    
    void StartEndHandles()
    {
        Transform handleTransform = slideSurface.transform;

        EditorGUI.BeginChangeCheck();
        Vector3 newStartPos = Handles.PositionHandle(slideSurface.startPoint.position, Quaternion.identity);
        Vector3 newEndPos = Handles.PositionHandle(slideSurface.endPoint.position, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(slideSurface, "changed start or end position");
            EditorUtility.SetDirty(slideSurface);
            slideSurface.startPoint.position = newStartPos;
            slideSurface.endPoint.position = newEndPos;
            Apply();
        }
    }

    void Apply()
    {
        Transform collision = slideSurface.Collision;
        Vector3 startPoint = slideSurface.startPoint.position;
        Vector3 endPoint = slideSurface.endPoint.position;

        collision.position = (startPoint + endPoint) / 2;
        collision.LookAt(endPoint);
        collision.localScale = new Vector3(1, 1, Vector3.Distance(startPoint, endPoint));
        
        Vector3 vector = (endPoint - startPoint).normalized;
        slideSurface.normal = new Vector3(-vector.y, vector.x, 0);
        if (slideSurface.invertNormal) slideSurface.normal = -slideSurface.normal;
    }
    
}
