using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Speaker))]
public class SpeakerEditor : Editor
{
    private Speaker speaker;
    public Speaker.DialogHitboxType speakerCollisionType;
    private float speakRange = 5f;
    private Vector3 boxSize = new Vector3(1, 1, 1);
    private Vector3 boxPosition = new Vector3(0, 0, 0);

    private void OnEnable()
    {
        speaker = (Speaker)target;
        speakRange = speaker.speakRange;
        speakerCollisionType = speaker.dialogHitboxType;
        boxSize = speaker.transform.InverseTransformDirection(speaker.boxSize);
        boxPosition = speaker.boxPosition;
        speakerCollisionType = speaker.dialogHitboxType;

        // speaker.gameObject.transform.hideFlags = HideFlags.HideInInspector;

        Tools.hidden = true;
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnDisable()
    {
        Tools.hidden = false;
        Undo.undoRedoPerformed -= OnUndoRedo;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        speakerCollisionType = (Speaker.DialogHitboxType)EditorGUILayout.EnumPopup(speakerCollisionType);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(speaker, "changed dialog hitbox type");
            speaker.dialogHitboxType = speakerCollisionType;
            EditorUtility.SetDirty(speaker);
        }
        if (speakerCollisionType == Speaker.DialogHitboxType.Box)
        {
            EditorGUILayout.LabelField("Box Settings", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            boxSize = EditorGUILayout.Vector3Field("Box Size", boxSize);
            boxPosition = EditorGUILayout.Vector3Field("Box Position", boxPosition);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(speaker, "changed box size or position");
                speaker.boxSize = boxSize;
                speaker.boxPosition = boxPosition;
                EditorUtility.SetDirty(speaker);
            }
        }
        else if (speakerCollisionType == Speaker.DialogHitboxType.Sphere)
        {
            EditorGUILayout.LabelField("Sphere Settings", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            speakRange = EditorGUILayout.FloatField("Speak Range", speakRange);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(speaker, "changed speak range");
                speaker.speakRange = speakRange;
                EditorUtility.SetDirty(speaker);
            }
        }
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        if (speaker is null) return;
        switch (speakerCollisionType)
        {
            case Speaker.DialogHitboxType.Box:

                //create handles to move the cube around and update the variables accordingly
                if (Tools.current == Tool.Move)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector3 newBoxPosition = Handles.PositionHandle(speaker.transform.position + boxPosition, Quaternion.identity);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(speaker, "changed box position");
                        boxPosition = newBoxPosition - speaker.transform.position;
                        // boxPosition = speaker.transform.InverseTransformVector(newBoxPosition - speaker.transform.position);
                        Debug.Log(boxPosition);
                        speaker.boxPosition = boxPosition;
                        EditorUtility.SetDirty(speaker);
                    }
                }
                if (Tools.current == Tool.Scale)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector3 newBoxSize = Handles.ScaleHandle(boxSize, speaker.transform.position + boxPosition, Quaternion.identity);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(speaker, "changed box size");
                        boxSize = newBoxSize;
                        //transform the box size to world space
                        boxSize = newBoxSize;
                        speaker.boxSize = boxSize;
                        EditorUtility.SetDirty(speaker);
                    }
                }
                //draw the box
                Handles.color = Color.green;
                Handles.DrawWireCube(speaker.transform.position + boxPosition, boxSize);
                break;
            case Speaker.DialogHitboxType.Sphere:
                Handles.color = Color.green;
                Handles.DrawWireDisc(speaker.transform.position, Camera.current ? Camera.current.transform.forward : Vector3.up, speakRange);
                Handles.DrawGizmos(Camera.current);
                break;

            default: break;
        }
    }

    private void OnDrawGizmos()
    {
        if (speaker is null) return;
        switch (speakerCollisionType)
        {
            case Speaker.DialogHitboxType.Box:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(speaker.transform.position + boxPosition, boxSize);
                break;
            case Speaker.DialogHitboxType.Sphere:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(speaker.transform.position, speakRange);
                break;

            default: break;
        }
    }

    private void OnUndoRedo()
    {
        Debug.Log("Undo/Redo");
        speakRange = speaker.speakRange;
        speakerCollisionType = speaker.dialogHitboxType;
        boxSize = speaker.transform.InverseTransformDirection(speaker.boxSize);
        boxPosition = speaker.boxPosition;
        speakerCollisionType = speaker.dialogHitboxType;

        // speaker.boxSize = boxSize;
        // speaker.boxPosition = boxPosition;
        // speaker.speakRange = speakRange;
        // speaker.dialogHitboxType = speakerCollisionType;
        // speaker.speakRange = speakRange;
    }

}