using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;

public class Climbing : State
{
    PlayerStateManager player;
    private double timeAtStateEnter;
    public override void UpdateState(PlayerStateManager player)
    {
        player.rb.velocity = Vector3.zero;

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     player.ChangeState(player.jumpingState);
        //     return;
        // }

        float input = ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.LeftRight
            ? player.moveInput.x : player.moveInput.y;

        Vector3 climbDirection = ControlValues.Instance.currentClimbEnd - ControlValues.Instance.currentClimbStart;
        climbDirection.Normalize();

        Vector3 closer = Vector3.Distance(player.rb.position, ControlValues.Instance.currentClimbStart) <
                         Vector3.Distance(player.rb.position, ControlValues.Instance.currentClimbEnd)
            ? ControlValues.Instance.currentClimbStart
            : ControlValues.Instance.currentClimbEnd;

        //these two ifs are here to prevent the player from climbing outside of the climb area
        if (closer == ControlValues.Instance.currentClimbStart &&
            Vector3.Distance(player.rb.position, closer) < 0.5f &&
            input < 0)
            return;

        if (closer == ControlValues.Instance.currentClimbEnd &&
            Vector3.Distance(player.rb.position, closer) < 0.5f &&
            input > 0)
            return;

        player.rb.velocity = climbDirection * input * player.climbSpeed;

        if (player.rb.velocity.x > 0 && ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.LeftRight)
            //rotate the player to face the right
            ControlValues.Instance.targetMeshRotation = Quaternion.Euler(0, -90, 0);
        else if (player.rb.velocity.x < 0 && ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.LeftRight)
            //rotate the player to face the left
            ControlValues.Instance.targetMeshRotation = Quaternion.Euler(0, 90, 0);
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {

    }

    public override void EnterState(PlayerStateManager player)
    {
        this.player = player;
        player.rb.velocity = Vector3.zero;
        timeAtStateEnter = Time.timeSinceLevelLoad;

        Vector3 closetsPoint = Utils.ClosestPointOnLineSegment(
            ControlValues.Instance.currentClimbStart,
            ControlValues.Instance.currentClimbEnd,
            player.rb.position);

        player.rb.position = closetsPoint; // snap the player to the clmbable surface

        player.rb.useGravity = false;

        Vector3 surfaceForwardVector = ControlValues.Instance.currentClimbEnd - ControlValues.Instance.currentClimbStart;
        if (ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.LeftRight)
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(surfaceForwardVector.normalized, ControlValues.Instance.currentSurfaceNormal);
        if (ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.UpDown)
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(-ControlValues.Instance.currentSurfaceNormal, surfaceForwardVector.normalized);
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;

        if (ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.LeftRight)
        {
            player.animator.SetBool("ClimbHorizontal", true);
        }
        if (ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.UpDown)
        {
            player.animator.SetBool("ClimbVertical", true);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.rb.useGravity = true;

        player.animator.SetBool("ClimbHorizontal", false);

        player.animator.SetBool("ClimbVertical", false);


        player.rb.AddForce(new Vector3(Mathf.Ceil(player.moveInput.x), 0, 0) * player.climbExitJumpForce, ForceMode.Impulse);

        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;

        ControlValues.Instance.lastClimbingTime = Time.timeSinceLevelLoad;
    }

    private void OnMove(object sender, InputAction.CallbackContext context) { }

    private void OnJump(object sender, InputAction.CallbackContext context)
    {
        if (context.started && Time.timeSinceLevelLoad - timeAtStateEnter > player.climbEnterExitCooldown)
        {
            player.ChangeState(player.jumpingState);
            return;
        }
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }

}
