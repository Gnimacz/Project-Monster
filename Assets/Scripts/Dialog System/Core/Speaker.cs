using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.InputSystem;
using TMPro;
using MonsterInput;
using UnityEngine.UI;


public class Speaker : MonoBehaviour
{
    public enum DialogHitboxType
    {
        Sphere,
        Box
    }

    private PlayerStateManager player;
    [HideInInspector] public DialogHitboxType dialogHitboxType;
    [HideInInspector] public float speakRange = 5f;
    [SerializeField] private bool activateInRange = true;
    public List<Dialog> dialog;
    private Sentence currentSentence;
    public static string currentText = "";
    public int currentDialogIndex = 0;
    private string[] dialogTexts;
    private int currentSentenceIndex = 0;
    private bool isSpeaking = false;
    private bool isDialogActive = false;
    [SerializeField] private bool isWithinRange = false;
    [SerializeField] private bool isDialogComplete = false;
    [SerializeField] public SphereCollider speakCollider;
    [SerializeField] public BoxCollider boxCollider;
    [HideInInspector] public Vector3 boxSize = new Vector3(1, 1, 1);
    [HideInInspector] public Vector3 boxPosition = new Vector3(0, 0, 0);
    [HideInInspector] public Transform boxTransform;

    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private GameObject dialogCanvas;

    void Start()
    {
        // speakCollider = gameObject.AddComponent<SphereCollider>();
        // speakCollider.radius = speakRange;
        // speakCollider.transform.position = transform.position;
        // speakCollider.isTrigger = true;
        // SetColliders();
    }
    void OnEnable()
    {
        dialogTexts = new string[dialog.Count];
        dialogCanvas.SetActive(false);

        InputEvents.InteractButton += OnInteract;
        InputEvents.UIInteractButton += OnUIInteract;
    }
    private void OnDisable()
    {
        InputEvents.InteractButton -= OnInteract;
        InputEvents.UIInteractButton -= OnUIInteract;
    }
    public void StartDialog()
    {
        if (isSpeaking || currentSentenceIndex > dialog[currentDialogIndex].sentences.Length - 1) return;
        dialogCanvas.SetActive(true);
        StartCoroutine(TypeText(dialog[currentDialogIndex]
        .sentences[currentSentenceIndex]
        .localizedSentence.GetLocalizedString()));
        isDialogActive = true;
        isSpeaking = true;
        isDialogComplete = false;
        if (InputEvents.playerInput.currentActionMap.name != "UI")
        {
            InputEvents.playerInput.SwitchCurrentActionMap("UI");
        }

    }

    IEnumerator TypeText(string text)
    {
        currentText = "";
        characterName.text = dialog[currentDialogIndex].sentences[currentSentenceIndex].name;
        foreach (char letter in text.ToCharArray())
        {
            currentText += letter;
            dialogText.text = currentText;
            yield return new WaitForSeconds(dialog[currentDialogIndex].sentences[currentSentenceIndex].delay);
        }
        dialog[currentDialogIndex].sentences[currentSentenceIndex].OnSentenceComplete?.Invoke();
        currentSentenceIndex++;
        isSpeaking = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        isWithinRange = true;
        if (!isDialogComplete && activateInRange && other.gameObject.layer == LayerMask.NameToLayer("Player") && !isDialogActive)
        {
            StartDialog();
            player = other.GetComponent<PlayerStateManager>();
            player.ChangeState(player.dialogueState);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isWithinRange = false;
        Debug.Log("Player exited dialog range");
    }

    // private void Update()
    // {
    //     if (isDialogActive)
    //     {
    //         if (Input.anyKeyDown)
    //         {
    //             if (isSpeaking && currentSentenceIndex <= dialog[currentDialogIndex].sentences.Length - 1)
    //             {
    //                 StopAllCoroutines();
    //                 dialogText.text = dialog[currentDialogIndex].sentences[currentSentenceIndex].localizedSentence.GetLocalizedString();
    //                 isSpeaking = false;
    //                 dialog[currentDialogIndex].sentences[currentSentenceIndex].OnSentenceComplete?.Invoke();
    //                 currentSentenceIndex++;
    //             }
    //             else if (currentSentenceIndex > dialog[currentDialogIndex].sentences.Length - 1)
    //             {
    //                 Debug.Log("End of dialog");
    //                 dialogCanvas.SetActive(false);
    //                 dialog[currentDialogIndex].OnDialogComplete?.Invoke();
    //                 currentSentenceIndex = 0;
    //                 isDialogActive = false;
    //                 isSpeaking = false;
    //                 player.ChangeState(player.idleState);
    //             }
    //             else
    //             {
    //                 StartDialog();
    //             }
    //         }
    //     }
    // }


    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
        if (isDialogComplete && isWithinRange && !isDialogActive && context.started)
        {
            StartDialog();
        }
    }

    private void OnUIInteract(object sender, InputAction.CallbackContext context)
    {
        if (isDialogActive && context.started)
        {
            if (InputEvents.playerInput.currentActionMap.name == "UI" && isSpeaking && currentSentenceIndex <= dialog[currentDialogIndex].sentences.Length - 1)
            {
                Debug.LogWarning("Skipping dialog");
                StopAllCoroutines();
                dialogText.text = dialog[currentDialogIndex].sentences[currentSentenceIndex]
                .localizedSentence.GetLocalizedString();
                isSpeaking = false;
                dialog[currentDialogIndex].sentences[currentSentenceIndex].OnSentenceComplete?.Invoke();
                currentSentenceIndex++;
            }
            else if (context.phase == InputActionPhase.Started && currentSentenceIndex > dialog[currentDialogIndex].sentences.Length - 1)
            {
                Debug.Log("End of dialog");
                dialogCanvas.SetActive(false);
                dialog[currentDialogIndex].OnDialogComplete?.Invoke();
                currentSentenceIndex = 0;
                isDialogActive = false;
                isSpeaking = false;
                isDialogComplete = true;
                InputEvents.playerInput.SwitchCurrentActionMap("Player");
                player.ChangeState(player.idleState);
            }
            else
            {
                Debug.Log("Start dialog");
                StartDialog();
            }
        }
    }

    public void SetDialogIndex(int index)
    {
        isDialogComplete = false;
        currentDialogIndex = index;
    }
    public void SetDialog(string identifier)
    {
        if (dialog.Count == 0) return;
        if (dialog.Contains(dialog.Find(x => x.identifier == identifier)))
        {
            SetDialogIndex(dialog.IndexOf(dialog.Find(x => x.identifier == identifier)));
        }
    }


    public void SetColliders()
    {
        if (dialogHitboxType == DialogHitboxType.Sphere)
        {
            if (speakCollider = null) { speakCollider = gameObject.AddComponent<SphereCollider>(); }
            speakCollider.enabled = true;
            speakCollider.radius = speakRange * 0.325f;
            speakCollider.transform.position = transform.position;
            speakCollider.isTrigger = true;
        }
        else if (dialogHitboxType == DialogHitboxType.Box)
        {
            if (boxCollider != null){ boxCollider = gameObject.AddComponent<BoxCollider>(); }
            boxCollider.enabled = true;
            boxCollider.isTrigger = true;
            // boxCollider.size = boxCollider.transform.InverseTransformDirection(boxSize);//boxSize;
            // boxCollider.transform.localScale = boxSize;
            boxCollider.size = boxSize;
            boxCollider.center = boxPosition;
        }
    }

    public void RemoveColliders()
    {
        if (dialogHitboxType == DialogHitboxType.Sphere)
        {
            DestroyImmediate(speakCollider);
        }
        else if (dialogHitboxType == DialogHitboxType.Box)
        {
            DestroyImmediate(boxCollider);
        }
    }

    public void SetColliderProperties()
    {
        if (dialogHitboxType == DialogHitboxType.Sphere)
        {
            speakCollider.radius = speakRange;
            speakCollider.transform.InverseTransformPoint(transform.position);
            speakCollider.transform.position = transform.position;
            speakCollider.isTrigger = true;
        }
        else if (dialogHitboxType == DialogHitboxType.Box)
        {
            boxCollider.size = boxSize;
            boxCollider.center = boxPosition;
        }
    }

    [ContextMenu("Lock Transform")]
    public void LockTransform()
    {
        gameObject.transform.hideFlags = HideFlags.NotEditable;
    }
    [ContextMenu("Unlock Transform")]
    public void UnlockTransform()
    {
        gameObject.transform.hideFlags = HideFlags.None;
    }
}
