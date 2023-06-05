using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;

public class Idle : State
{
    PlayerStateManager player;
    public override void UpdateState(PlayerStateManager player)
    {

        if (player.moveInput.x != 0)
        {
            player.ChangeState(player.runningState);
            return;
        }
        if (!player.isGrounded)
        {
            player.ChangeState(player.inAirState);
            return;
        }

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
        if (context.ReadValueAsButton())
        {
            Debug.Log("Interact button pressed");
            return;
        }
    }
}
