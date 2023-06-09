using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using MonsterInput;
using UnityEngine.VFX;

public class PlayerStateManager : MonoBehaviour
{
    #region States
    public Idle idleState = new Idle();
    public Running runningState = new Running();
    public Jumping jumpingState = new Jumping();
    public InAir inAirState = new InAir();
    public Climbing climbingState = new Climbing();
    public Sliding slideState = new Sliding();
    public Dialogue dialogueState = new Dialogue();
    public Death deathState = new Death();
    #endregion

    #region Current and previous states
    [SerializeField] private State currentState;
    private State previousState;
    #endregion

    #region Properties

    public Rigidbody rb;
    public Transform feet;
    public Transform mesh;
    public Animator animator;

    [Space(10)]
    [Header("General movement")]
    public float horizontalDrag = 5f;
    public float runAcceleration = 5f;
    public float runMaxSpeed = 5f;
    public float meshRotationSpeed = 0.1f;
    [SerializeField] private LayerMask groundLayerMask;
    [Space(10)]
    [Header("Jumping variables")]
    public float airAcceleration = 5f;
    public float airMaxSpeed = 5f;
    public float jumpForce = 5f;
    public float coyoteGraceTime = 0.1f;
    public float maxJumpDuration = 2f;
    public float jumpFloatForce = 1f;
    [Space(10)]
    [Header("Climbing and sliding")]
    public float climbSpeed = 5f;
    public float slideSpeed = 10f;
    public float climbExitJumpForce = 3f;
    public float slideExitLaunchForce = 3f;
    public float climbEnterExitCooldown = 0.5f;
    public float defaultSurfacEdgeCapRadius = 0.5f; //the distance from the edge of surfaces that decides if the player is going to exit the surface

    [NonSerialized] public bool isGrounded = false; //{ get; private set; }
    public Vector2 moveInput { get; private set; }
    #endregion 

    #region Sounds
    [Space(10)]
    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioSource secondaryAudioSource;
    public AudioClip runSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip slideSound;
    public AudioClip climbSound;
    public AudioClip bounceSound;
    public AudioClip deathSound;
    #endregion

    #region VFX
    [Space(10)]
    [Header("VFX")]
    public VisualEffect runVFX;
    public VisualEffect jumpVFX;
    public VisualEffect slideVFX;
    #endregion

    private void Start()
    {
        //add current position as checkpoint at the start for testing
        ControlValues.Instance.lastCheckpoint = transform.position;
        ControlValues.Instance.checkpointBacklog.Add(transform.position);

        //stop the vfx
        runVFX.Stop();
        jumpVFX.Stop();
        slideVFX.Stop();
    }

    private void OnEnable()
    {
        currentState = idleState;
        currentState.EnterState(this);
        previousState = currentState;

        InputEvents.Move += OnMove;
        animator = GetComponentInChildren<Animator>();
    }
    private void OnDisable()
    {
        InputEvents.Move -= OnMove;
        currentState.ExitState(this);
    }
    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        //hardcoded limitations because unity is stupid
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        UpdateMeshRotation();
        animator.SetFloat("MovementInput.x", rb.velocity.normalized.x);
        animator.SetFloat("MovementInput.y", rb.velocity.normalized.y);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);

        //cast a ray downard from the bottom of the character collider to see if we are on the ground
        isGrounded = Physics.OverlapSphere(feet.position, 0.4f, /*~LayerMask.GetMask("Player"))*/groundLayerMask).Length > 0;
        if (isGrounded)
        {
            ControlValues.Instance.lastGroundedTime = Time.timeSinceLevelLoad;
        }
    }

    public void ChangeState(State newState)
    {
        if (newState == currentState) return;
        currentState.ExitState(this);
        //audioSource.volume = 1;
        //audioSource.pitch = 1;
        newState.EnterState(this);
        currentState = newState;
    }

    public void UpdateMeshRotation()
    {
        if(ControlValues.Instance.targetMeshRotation == null) return;
        mesh.localRotation = Quaternion.Lerp(
            mesh.localRotation, 
            ControlValues.Instance.targetMeshRotation, 
            meshRotationSpeed * Time.deltaTime);
    }
    
    public void ApplyDrag()
    {
        rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, horizontalDrag * Time.deltaTime), rb.velocity.y, 0);
    }

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.position = ControlValues.Instance.lastCheckpoint;
        ChangeState(deathState);
    }

    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(10, 10, 300, 30), "Current State: " + currentState.ToString());
    //     GUI.Label(new Rect(10, 20, 300, 30), "Current Velocity: " + rb.velocity.ToString());
    //     GUI.Label(new Rect(10, 30, 300, 30), (Time.timeSinceLevelLoad - ControlValues.Instance.lastGroundedTime).ToString());
    //     GUI.Label(new Rect(10, 50, 256, 30), "Current MoveInput: " + moveInput.ToString());
    // }

    private void OnTriggerStay(Collider other)
    {
        //this has to be here too so you can enter the climbing state without leaving the area
        if (other.tag == "ClimbSurface" 
            && rb.velocity.y <= 0 
            && currentState != climbingState 
            && Time.timeSinceLevelLoad - ControlValues.Instance.lastClimbingTime > climbEnterExitCooldown)
        {
            ClimbSurface climbSurface = other.transform.parent.GetComponent<ClimbSurface>();
            
            if (climbSurface.climbOrientation == ControlValues.ClimbOrientation.LeftRight) return;
            
            ControlValues.Instance.currentClimbStart = climbSurface.startPoint.position;
            ControlValues.Instance.currentClimbEnd = climbSurface.endPoint.position;
            ControlValues.Instance.currentClimbOrientation = climbSurface.climbOrientation;
            ControlValues.Instance.currentSurfaceEdgeCapRadius = climbSurface.edgeCapRadius;
            
            ChangeState(climbingState);
        }
    }

    private void OnTriggerEnter(Collider other) // to implement it quickly I'm doing this here but there's probably a cleaner way
    {
        switch (other.tag)
        {
            case "ClimbSurface":
                ClimbSurface climbSurface = other.transform.parent.GetComponent<ClimbSurface>();
                ControlValues.Instance.currentClimbStart = climbSurface.startPoint.position;
                ControlValues.Instance.currentClimbEnd = climbSurface.endPoint.position;
                ControlValues.Instance.currentClimbOrientation = climbSurface.climbOrientation;
                ControlValues.Instance.currentSurfaceNormal = climbSurface.normal;
                ControlValues.Instance.currentSurfaceEdgeCapRadius = climbSurface.edgeCapRadius;

                ChangeState(climbingState);
                break;

            case "SlideSurface":
                SlideSurface slideSurface = other.transform.parent.GetComponent<SlideSurface>();
                ControlValues.Instance.currentSlideStart = slideSurface.startPoint.position;
                ControlValues.Instance.currentSlideEnd = slideSurface.endPoint.position;
                ControlValues.Instance.currentSlideDirection = (slideSurface.endPoint.position - slideSurface.startPoint.position).normalized;
                ControlValues.Instance.currentSurfaceNormal = slideSurface.normal;
                ControlValues.Instance.currentSurfaceEdgeCapRadius = slideSurface.edgeCapRadius;

                ChangeState(slideState);
                break;

            case "DeathSurface":
                Respawn();
                break;

            case "Checkpoint":
                if (!ControlValues.Instance.checkpointBacklog.Contains(other.transform.position))
                {
                    ControlValues.Instance.lastCheckpoint = other.transform.position;
                    ControlValues.Instance.checkpointBacklog.Add(other.transform.position);
                }
                break;

            default:
                break;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BouncySurface")
        {
            BouncyObject bouncyObject = other.gameObject.GetComponent<BouncyObject>();
            Vector3 launchDirectionRotated = new Vector3(-bouncyObject.launchDirection.y, bouncyObject.launchDirection.x, 0);
            rb.velocity = launchDirectionRotated * Vector3.Dot(rb.velocity, launchDirectionRotated);
            //I did a vecto projection here to make it so that the player doesn't loose velocity in relation to
            //the bouncy object but still gets launched the same amout no matter what
            //I think it works the way I intended but you never know with math
            rb.velocity += bouncyObject.launchDirection * bouncyObject.launchForce;
            secondaryAudioSource.pitch = 1;
            secondaryAudioSource.volume = 1;
            secondaryAudioSource.PlayOneShot(bounceSound);
        }
    }

    #region Input Actions
    public void OnMove(object sender, InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    #endregion
}
