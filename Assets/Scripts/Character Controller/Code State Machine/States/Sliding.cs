using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MonsterInput;

public class Sliding : State
{
    private PlayerStateManager player;
    public override void UpdateState(PlayerStateManager player)
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     player.ChangeState(player.jumpingState);
        //     return;
        // }

        if (Vector3.Distance(player.rb.position, ControlValues.Instance.currentSlideEnd) < 0.5f)
        {
            player.ChangeState(player.idleState);
            return;
        }
        
        player.rb.velocity = ControlValues.Instance.currentSlideDirection * player.slideSpeed;

    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        
    }
    
    public override void EnterState(PlayerStateManager player)
    {
        player.rb.velocity = Vector3.zero;

        Vector3 closetsPoint = Utils.ClosestPointOnLineSegment(
            ControlValues.Instance.currentSlideStart,
            ControlValues.Instance.currentSlideEnd,
            player.rb.position);

        player.rb.position = closetsPoint; // snap the player to the clmbable surface

        player.rb.useGravity = false;

        this.player = player;
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.rb.useGravity = true;

        Vector3 horizontalDirection = new Vector3(Mathf.Round(ControlValues.Instance.currentSlideDirection.x), 0, 0);
        player.rb.AddForce(horizontalDirection * player.slideExitLaunchForce, ForceMode.Impulse);
        player.airMaxSpeed = player.rb.velocity.magnitude;

        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;
    }
    
    private void OnMove(object sender, InputAction.CallbackContext context) { }

    private void OnJump(object sender, InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.ChangeState(player.jumpingState);
            return;
        }
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }
    
}
