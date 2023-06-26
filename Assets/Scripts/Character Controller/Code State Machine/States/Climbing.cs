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
            Vector3.Distance(player.rb.position, closer) < ControlValues.Instance.currentSurfaceEdgeCapRadius &&
            input < 0)
            return;

        if (closer == ControlValues.Instance.currentClimbEnd &&
            Vector3.Distance(player.rb.position, closer) < ControlValues.Instance.currentSurfaceEdgeCapRadius &&
            input > 0)
            return;

        player.rb.velocity = climbDirection * input * player.climbSpeed;

        if (player.rb.velocity.x > 0 && ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.LeftRight)
            //rotate the player to face the right
            ControlValues.Instance.targetMeshRotation = Quaternion.Euler(0, -90, 0);
        else if (player.rb.velocity.x < 0 && ControlValues.Instance.currentClimbOrientation == ControlValues.ClimbOrientation.LeftRight)
            //rotate the player to face the left
            ControlValues.Instance.targetMeshRotation = Quaternion.Euler(0, 90, 0);

        // if(player.rb.velocity.magnitude > 0.1f)
        //     player.audioSource.UnPause();
        // else
        //     player.audioSource.Pause();

        //if the player is not moving, fade out the climbing sound
        if (player.rb.velocity.magnitude < 0.1f)
        {
            player.audioSource.volume -= Time.deltaTime * 2;
            if (player.audioSource.volume <= 0.1f)
            {
                player.audioSource.volume = 0.1f;
                player.audioSource.Pause();
            }
        }
        else
        {
            player.audioSource.volume = 1;
            player.audioSource.UnPause();
        }
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

        //loop the climbing sound
        player.audioSource.clip = player.climbSound;
        player.audioSource.loop = true;
        player.audioSource.volume = UnityEngine.Random.Range(0.4f, 0.5f);
        player.audioSource.Play();
    }

    IEnumerator FadeOutSlideSound(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 0.5f;
            yield return null;
        }
        audioSource.Stop();
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
        
        player.secondaryAudioSource.clip = player.slideSound;
        player.secondaryAudioSource.time = player.audioSource.time;
        player.secondaryAudioSource.volume = player.audioSource.volume;
        //fade out the sliding sound
        player.audioSource.Stop();
        player.secondaryAudioSource.Play();
        player.StartCoroutine(FadeOutSlideSound(player.secondaryAudioSource));
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
