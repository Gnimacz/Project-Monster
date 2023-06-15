using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.InputSystem;
using MonsterInput;

public class Death : State
{
    PlayerStateManager player;
    float DeathTimer = 0f;

    public override void UpdateState(PlayerStateManager player)
    {
        player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
        ControlValues.Instance.targetMeshRotation = Quaternion.Euler(0, 0, 0);
        DeathTimer += Time.deltaTime;
        if (DeathTimer >= ControlValues.Instance.deathCooldown)
        {
            player.rb.velocity = Vector3.zero;
            player.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager player) { }

    public override void EnterState(PlayerStateManager player)
    {
        DeathTimer = 0f;
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
