using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAnim : MonoBehaviour
{
    public Animator anim;
    private void OnEnable() {
        if (anim != null) return;
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            anim.SetTrigger("Bounce");
        }
    }
}
