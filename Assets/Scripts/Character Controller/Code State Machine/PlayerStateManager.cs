using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using MonsterInput;

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
    #endregion

    #region Current and previous states
    [SerializeField] private State currentState;
    private State previousState;
    #endregion

    #region Properties

    public Rigidbody rb;
    public Transform feet;

    public float horizontalDrag = 5f;
    public float runAcceleration = 5f;
    public float runMaxSpeed = 5f;
    public float airAcceleration = 5f;
    public float airMaxSpeed = 5f;
    [NonSerialized] public float originalAirMaxSpeed = 5f; //TODO(Look into a possible better way to handle this hack)
    public float jumpForce = 5f;
    public float climbSpeed = 5f;
    public float slideSpeed = 10f;
    public float climbExitJumpForce = 3f;
    public float slideExitLaunchForce = 3f;
    public float coyoteGraceTime = 0.1f;
    

    public RaycastHit groundRayCastResults;
    [SerializeField] private float groundRayLength = 1.5f;
    public bool isGrounded = false; //{ get; private set; }
    [SerializeField] private LayerMask groundLayerMask;

    public Vector2 moveInput { get; private set; }
    #endregion 

    private void Start()
    {
        //add current position as checkpoint at the start for testing
        ControlValues.Instance.lastCheckpoint = transform.position;
        ControlValues.Instance.checkpointBacklog.Add(transform.position);
        originalAirMaxSpeed = airMaxSpeed;
    }

    private void OnEnable()
    {
        currentState = idleState;
        currentState.EnterState(this);
        previousState = currentState;

        InputEvents.Move += OnMove;
    }
    private void OnDisable()
    {
        InputEvents.Move -= OnMove;
    }
    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        //hardcoded limitations because unity is stupid
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);

        //cast a ray downard from the bottom of the character collider to see if we are on the ground
        isGrounded = Physics.OverlapSphere(feet.position, 0.4f, ~LayerMask.GetMask("Player")).Length > 0;
        if (isGrounded)
        {
            ControlValues.Instance.lastGroundedTime = Time.timeSinceLevelLoad;
        }
    }

    public void ChangeState(State newState)
    {
        if (newState == currentState) return;
        currentState.ExitState(this);
        newState.EnterState(this);
        currentState = newState;
    }

    public void ApplyDrag()
    {
        rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, horizontalDrag * Time.deltaTime), rb.velocity.y, 0);
    }

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.position = ControlValues.Instance.lastCheckpoint;
        ChangeState(idleState);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), "Current State: " + currentState.ToString());
        GUI.Label(new Rect(10, 20, 300, 30), "Current Velocity: " + rb.velocity.ToString());
        GUI.Label(new Rect(10, 30, 300, 30), (Time.timeSinceLevelLoad - ControlValues.Instance.lastGroundedTime).ToString());
        GUI.Label(new Rect(10, 50, 256, 30), "Current MoveInput: " + moveInput.ToString());
    }

    private void OnTriggerStay(Collider other)
    {
        //this has to be here too so you can enter the climbing state without leaving the area
        if (other.tag == "ClimbSurface" && rb.velocity.y <= 0 && currentState != climbingState)
        {
            ClimbSurface climbSurface = other.transform.parent.GetComponent<ClimbSurface>();
            
            if (climbSurface.climbOrientation == ControlValues.ClimbOrientation.LeftRight) return;
            
            ControlValues.Instance.currentClimbStart = climbSurface.startPoint.position;
            ControlValues.Instance.currentClimbEnd = climbSurface.endPoint.position;
            ControlValues.Instance.currentClimbOrientation = climbSurface.climbOrientation;
            
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

                ChangeState(climbingState);
                break;

            case "SlideSurface":
                SlideSurface slideSurface = other.transform.parent.GetComponent<SlideSurface>();
                ControlValues.Instance.currentSlideStart = slideSurface.startPoint.position;
                ControlValues.Instance.currentSlideEnd = slideSurface.endPoint.position;
                ControlValues.Instance.currentSlideDirection = (slideSurface.endPoint.position - slideSurface.startPoint.position).normalized;

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

    #region Input Actions
    public void OnMove(object sender, InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    #endregion
}
