using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;

public class Jumping : State
{
    PlayerStateManager player;
    private double stateEnterTime;
    
    public override void UpdateState(PlayerStateManager player)
    {
        player.rb.AddForce(new Vector3(0, player.jumpFloatForce * Time.deltaTime * 100, 0), ForceMode.Acceleration);
        
        if(player.isGrounded && Time.timeSinceLevelLoad - stateEnterTime > 0.1f) // just to give time for the player to leave the ground
        {
            Debug.Log("entered idle because gounded");
            player.ChangeState(player.idleState);
            return;
        }

        if (Time.timeSinceLevelLoad - stateEnterTime > player.maxJumpDuration || player.rb.velocity.y < 0)
        {
            player.ChangeState(player.inAirState);
            return;
        }
        
        if (player.rb.velocity.x > 0)
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        else
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        
        InAirMovement(this.player);
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        
    }
    
    void InAirMovement(PlayerStateManager player)
    {
        float acceleration = player.moveInput.x * player.airAcceleration * Time.deltaTime;
        if ((acceleration > 0 && player.rb.velocity.x < player.airMaxSpeed) || 
            (acceleration < 0 && player.rb.velocity.x > -player.airMaxSpeed))
            player.rb.velocity += new Vector3(acceleration, 0, 0);
    }
    
    public override void EnterState(PlayerStateManager player)
    {
    
        player.rb.velocity = new Vector3(player.rb.velocity.x, 0, 0);
        player.rb.AddForce(new Vector3(0, player.jumpForce, 0), ForceMode.Impulse);

        stateEnterTime = Time.timeSinceLevelLoad;
        
        this.player = player;
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;

        player.animator.SetBool("Jump", true);

        //play the jump sound with a random pitch
        player.audioSource.pitch = Random.Range(0.8f, 1f);
        player.audioSource.clip = player.jumpSound;
        player.audioSource.loop = false;
        player.audioSource.volume = 0.5f;
        player.audioSource.Play();

        //play the jump vfx
        player.jumpVFX.Play();
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.audioSource.pitch = 1;
        player.animator.SetBool("Jump", false);
        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;
    }

    private void OnMove(object sender, InputAction.CallbackContext context)
    {
    }

    private void OnJump(object sender, InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            player.ChangeState(player.inAirState);
        }
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }
}
