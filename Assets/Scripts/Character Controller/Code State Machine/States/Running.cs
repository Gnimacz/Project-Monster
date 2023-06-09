﻿using System;
using Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;
using UnityEngine.VFX;

public class Running : State
{
    PlayerStateManager player;
    Transform oldParentTransform;
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
        player.animator.SetBool("Run", true);

        this.player = player;
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;
        //loop the running sound
        player.audioSource.pitch = 1.2f;
        player.audioSource.clip = player.runSound;
        player.audioSource.loop = true;
        player.audioSource.volume = 0.4f;
        player.audioSource.Play();

        //play the running vfx
        if (oldParentTransform == null) oldParentTransform = player.runVFX.transform.parent;
        player.runVFX.gameObject.transform.parent = oldParentTransform;
        player.runVFX.gameObject.transform.localPosition = new Vector3(0, -0.5f, 0);
        player.runVFX.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        // player.runVFX.gameObject.transform.rotation = player.mesh.rotation * Quaternion.Euler(0, 90, 0);
        player.runVFX.Play();
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.animator.SetBool("Run", false);
        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;
        player.audioSource.Stop();

        oldParentTransform = player.runVFX.transform.parent;
        Debug.Log(oldParentTransform.name);
        player.runVFX.Stop();
    }

    private void OnMove(object sender, InputAction.CallbackContext context) { }

    private void OnJump(object sender, InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            player.ChangeState(player.jumpingState);
            player.runVFX.transform.parent = null;
            return;
        }
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }
}
