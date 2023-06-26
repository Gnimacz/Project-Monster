using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceCollider : MonoBehaviour
{
    public Surface surface;

    private void OnEnable()
    {
        transform.parent.gameObject.GetComponent<Surface>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")){
            surface.EnterSurface();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")){
            surface.ExitSurface();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")){
            surface.EnterSurface();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")){
            surface.ExitSurface();
        }
    }
}
