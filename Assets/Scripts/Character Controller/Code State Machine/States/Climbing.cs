using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;

public class Climbing : State
{
    PlayerStateManager player;
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
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        
    }
    
    public override void EnterState(PlayerStateManager player)
    {
        this.player = player;
        player.rb.velocity = Vector3.zero;

        Vector3 closetsPoint = Utils.ClosestPointOnLineSegment(
            ControlValues.Instance.currentClimbStart,
            ControlValues.Instance.currentClimbEnd,
            player.rb.position);

        player.rb.position = closetsPoint; // snap the player to the clmbable surface

        player.rb.useGravity = false;

        Vector3 surfaceForwardVector = ControlValues.Instance.currentClimbEnd - ControlValues.Instance.currentClimbStart;
        ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(surfaceForwardVector.normalized, ControlValues.Instance.currentSurfaceNormal);
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.rb.useGravity = true;
        
        player.rb.AddForce(new Vector3(Mathf.Ceil(player.moveInput.x), 0, 0) * player.climbExitJumpForce, ForceMode.Impulse);
        
        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;
    }

    private void OnMove(object sender, InputAction.CallbackContext context) { }

    private void OnJump(object sender, InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jumping from climbing state");
            player.ChangeState(player.jumpingState);
            return;
        }
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }

}
