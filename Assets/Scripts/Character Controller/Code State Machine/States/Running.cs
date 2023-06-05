﻿using System;
using Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;

public class Running : State
{
    PlayerStateManager player;
    public override void UpdateState(PlayerStateManager player)
    {
        //Refactor this
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            player.ChangeState(player.idleState);
            return;
        }

        // float acceleration = Input.GetAxis("Horizontal") * player.runAcceleration * Time.deltaTime;
        float acceleration = player.moveInput.x * player.runAcceleration * Time.deltaTime;
        if ((acceleration > 0 && player.rb.velocity.x < player.runMaxSpeed) ||
            (acceleration < 0 && player.rb.velocity.x > -player.runMaxSpeed))
            player.rb.velocity += new Vector3(acceleration, 0, 0);


        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     player.ChangeState(player.jumpingState);
        //     return;
        // }
        if (!player.isGrounded)
        {
            player.ChangeState(player.inAirState);
            return;
        }
        
        if (player.rb.velocity.x > 0)
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        else
            ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        

        player.ApplyDrag();
    }

    public override void FixedUpdateState(PlayerStateManager player) { }

    public override void EnterState(PlayerStateManager player)
    {
        this.player = player;
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;
    }

    public override void ExitState(PlayerStateManager player)
    {
        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;
    }

    private void OnMove(object sender, InputAction.CallbackContext context) { }

    private void OnJump(object sender, InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            player.ChangeState(player.jumpingState);
            return;
        }
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }
}
