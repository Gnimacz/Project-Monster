using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Events;

// [CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog System/Dialog")]
[System.Serializable]
public class Dialog
{
    public string identifier;
    public AudioClip[] voiceClips;
    public Sentence[] sentences;

    public UnityEvent OnDialogComplete;
}

[System.Serializable]
public class Sentence 
{
    [Header("Speaker")]
    public string name = "";

    // [TextArea(3, 10)]
    // public string sentence;
    public LocalizedString localizedSentence;
    public float delay = 0.03f;
    public UnityEvent OnSentenceComplete;
}