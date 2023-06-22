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
        player.animator.SetBool("Sliding", true);
        player.rb.velocity = Vector3.zero;
        Vector3 closetsPoint = Utils.ClosestPointOnLineSegment(
            ControlValues.Instance.currentSlideStart,
            ControlValues.Instance.currentSlideEnd,
            player.rb.position);

        player.rb.position = closetsPoint; // snap the player to the clmbable surface

        player.rb.useGravity = false;
        
        ControlValues.Instance.targetMeshRotation = Quaternion.LookRotation(ControlValues.Instance.currentSlideDirection, ControlValues.Instance.currentSurfaceNormal);

        this.player = player;
        InputEvents.Move += OnMove;
        InputEvents.InteractButton += OnInteract;
        InputEvents.JumpButton += OnJump;

        //loop the sliding sound
        player.audioSource.clip = player.slideSound;
        player.audioSource.loop = true;
        player.audioSource.Play();
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.animator.SetBool("Sliding", false);
        player.rb.useGravity = true;

        Vector3 horizontalDirection = new Vector3(Mathf.Round(ControlValues.Instance.currentSlideDirection.x), 0, 0);
        player.rb.AddForce(horizontalDirection * player.slideExitLaunchForce, ForceMode.Impulse);

        InputEvents.Move -= OnMove;
        InputEvents.InteractButton -= OnInteract;
        InputEvents.JumpButton -= OnJump;

        //create a new audio source for the sliding sound

        player.secondaryAudioSource.clip = player.slideSound;
        player.secondaryAudioSource.time = player.audioSource.time;
        player.secondaryAudioSource.volume = player.audioSource.volume;
        //fade out the sliding sound
        player.audioSource.Stop();
        player.secondaryAudioSource.Play();
        player.StartCoroutine(FadeOutSlideSound(player.secondaryAudioSource));

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
