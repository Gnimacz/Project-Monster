using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;
using UnityEngine.VFX;

public class InAir : State
{
    PlayerStateManager player;
    public override void UpdateState(PlayerStateManager player)
    {
        if (player.isGrounded)
        {
            player.ChangeState(player.idleState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) &&
            Time.timeSinceLevelLoad - ControlValues.Instance.lastGroundedTime <= player.coyoteGraceTime) //coyote time jump in air
        {
            player.ChangeState(player.jumpingState);
            return;
        }
        
        if (player.rb.velocity.x > 0)
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        else
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        
        InAirMovement(player);
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        
    }
    
    public override void EnterState(PlayerStateManager player)
    {
        player.animator.SetBool("Jump", true);
        this.player = player;
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.animator.SetBool("Jump", false);
        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;
        if (player.rb.velocity.y <= -20f) player.audioSource.PlayOneShot(player.landSound);

        player.jumpVFX.Stop();
    }

    void InAirMovement(PlayerStateManager player)
    {
        float acceleration = player.moveInput.x * player.airAcceleration * Time.deltaTime;
        if ((acceleration > 0 && player.rb.velocity.x < player.airMaxSpeed) || 
            (acceleration < 0 && player.rb.velocity.x > -player.airMaxSpeed))
            player.rb.velocity += new Vector3(acceleration, 0, 0);
    }

    private void OnMove(object sender, InputAction.CallbackContext context)
    {
    }

    private void OnJump(object sender, InputAction.CallbackContext context)
    {
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }

}
