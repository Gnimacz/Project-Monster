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
        Tools.hidden = true;
        speakerCollisionType = speaker.dialogHitboxType;
        // boxPosition = speaker.boxPosition;
        // boxSize = speaker.boxSize;
        // speakRange = speaker.speakRange;
        // Undo.undoRedoPerformed += OnUndoRedo;
        if (speaker.boxCollider == null)
        {
            speaker.boxCollider = speaker.gameObject.AddComponent<BoxCollider>();
        }
        if (speaker.speakCollider == null)
        {
            speaker.speakCollider = speaker.gameObject.AddComponent<SphereCollider>();

        }
        if (speakerCollisionType == Speaker.DialogHitboxType.Box)
        {
            speaker.speakCollider.enabled = false;
            speaker.speakCollider.hideFlags = HideFlags.HideInInspector;
            speaker.boxCollider.hideFlags = HideFlags.None;
        }
        else if (speakerCollisionType == Speaker.DialogHitboxType.Sphere)
        {
            speaker.speakCollider.enabled = true;
            speaker.speakCollider.hideFlags = HideFlags.None;
            speaker.boxCollider.hideFlags = HideFlags.HideInInspector;
        }
    }

    private void OnDisable()
    {
        Tools.hidden = false;
        // Undo.undoRedoPerformed -= OnUndoRedo;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        speakerCollisionType = (Speaker.DialogHitboxType)EditorGUILayout.EnumPopup(speakerCollisionType);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(speaker, "changed dialog hitbox type");
            speaker.dialogHitboxType = speakerCollisionType;
            if (speakerCollisionType == Speaker.DialogHitboxType.Box)
            {

                if (speaker.boxCollider == null)
                {
                    speaker.boxCollider = speaker.gameObject.AddComponent<BoxCollider>();
                }
                if (speaker.speakCollider != null) speaker.speakCollider.enabled = false;
                if (!speaker.boxCollider.enabled) speaker.boxCollider.enabled = true;
                if (!speaker.boxCollider.isTrigger) speaker.boxCollider.isTrigger = true;
                speaker.boxCollider.hideFlags = HideFlags.None;
                speaker.speakCollider.hideFlags = HideFlags.HideInInspector;
            }
            else if (speakerCollisionType == Speaker.DialogHitboxType.Sphere)
            {
                speaker.speakRange = speakRange;
                if (speaker.speakCollider == null)
                {
                    speaker.speakCollider = speaker.gameObject.AddComponent<SphereCollider>();
                    speaker.speakCollider.enabled = true;
                    speaker.speakCollider.isTrigger = true;
                }
                if (speaker.boxCollider != null) speaker.boxCollider.enabled = false;
                speaker.speakCollider.enabled = true;
                speaker.speakCollider.hideFlags = HideFlags.None;
                speaker.boxCollider.hideFlags = HideFlags.HideInInspector;
            }
            EditorUtility.SetDirty(speaker);
        }
        #region old code
        // if (speakerCollisionType == Speaker.DialogHitboxType.Box)
        // {
        //     EditorGUILayout.LabelField("Box Settings", EditorStyles.boldLabel);
        //     EditorGUI.BeginChangeCheck();
        //     boxSize = EditorGUILayout.Vector3Field("Box Size", boxSize);
        //     boxPosition = EditorGUILayout.Vector3Field("Box Position", boxPosition);

        //     if (EditorGUI.EndChangeCheck())
        //     {
        //         Undo.RecordObject(speaker, "changed box size or position");
        //         SetSizeVariables();
        //         EditorUtility.SetDirty(speaker);
        //     }
        // }
        // else if (speakerCollisionType == Speaker.DialogHitboxType.Sphere)
        // {
        //     EditorGUILayout.LabelField("Sphere Settings", EditorStyles.boldLabel);
        //     EditorGUI.BeginChangeCheck();
        //     speakRange = EditorGUILayout.FloatField("Speak Range", speakRange);
        //     if (EditorGUI.EndChangeCheck())
        //     {
        //         Undo.RecordObject(speaker, "changed speak range");
        //         SetSizeVariables();
        //         EditorUtility.SetDirty(speaker);
        //     }
        // }
        #endregion
        base.OnInspectorGUI();
    }


    void OnSceneGUI()
    {
        if (speakerCollisionType == Speaker.DialogHitboxType.Box)
        {
            DrawBoxEditor();
        }
        else if (speakerCollisionType == Speaker.DialogHitboxType.Sphere)
        {
            DrawSphereEditor();
        }
    }

    void DrawBoxEditor()
    {
        EditorGUI.BeginChangeCheck();
        if (Tools.current == Tool.Move)
        {
            Vector3 newBoxPosition = Handles.PositionHandle(speaker.transform.position + speaker.transform.TransformVector(speaker.boxCollider.center), Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Speak Box Collider Position");
                EditorUtility.SetDirty(speaker);
                speaker.boxCollider.center = speaker.transform.InverseTransformVector(newBoxPosition - speaker.transform.position);
            }
        }
        else if (Tools.current == Tool.Scale)
        {
            Vector3 newBoxScale = Handles.ScaleHandle(speaker.transform.localScale + speaker.transform.TransformVector(speaker.boxCollider.size), speaker.transform.position + speaker.transform.TransformVector(speaker.boxCollider.center), Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Speak Box Collider Size");
                EditorUtility.SetDirty(speaker);
                speaker.boxCollider.size = speaker.transform.InverseTransformVector(newBoxScale - speaker.transform.localScale);
            }
        }

        //draw the box
        Handles.color = Color.green;
        Handles.DrawWireCube(speaker.transform.position + speaker.transform.TransformVector(speaker.boxCollider.center), speaker.transform.TransformVector(speaker.boxCollider.size));
    }

    void DrawSphereEditor()
    {
        //draw the editor for the spherecollider
        EditorGUI.BeginChangeCheck();
        if (Tools.current == Tool.Move)
        {
            Vector3 newSpherePosition = Handles.PositionHandle(speaker.transform.position + speaker.transform.TransformVector(speaker.speakCollider.center), Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Speak Sphere Collider Position");
                EditorUtility.SetDirty(speaker);
                speaker.speakCollider.center = speaker.transform.InverseTransformVector(newSpherePosition - speaker.transform.position);
            }
        }
        else if (Tools.current == Tool.Scale)
        {
            Handles.color = Color.green;
            float newSphereRadius = Handles.ScaleSlider(speaker.transform.TransformVector(new Vector3(speaker.speakCollider.radius, 0, 0)).x, speaker.transform.position, Camera.current.transform.up, Quaternion.identity, HandleUtility.GetHandleSize(speaker.transform.position), 1);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Speak Sphere Collider Radius");
                EditorUtility.SetDirty(speaker);
                speaker.speakCollider.radius = speaker.transform.InverseTransformVector(new Vector3(newSphereRadius, 0, 0)).x;
                // speaker.speakRange = newSphereRadius;
            }
        }
    }
}