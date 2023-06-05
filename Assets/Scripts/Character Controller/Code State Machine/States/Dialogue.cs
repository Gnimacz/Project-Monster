using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.InputSystem;
using MonsterInput;

public class Dialogue : State
{
    PlayerStateManager player;
    public override void UpdateState(PlayerStateManager player)
    {
        player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
    }

    public override void FixedUpdateState(PlayerStateManager player) { }

    public override void EnterState(PlayerStateManager player)
    {
        this.player = player;
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;
        player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
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
    }

    private void OnInteract(object sender, InputAction.CallbackContext context)
    {
    }
}
