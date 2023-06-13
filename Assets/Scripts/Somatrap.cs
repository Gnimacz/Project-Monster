using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Somatrap : MonoBehaviour
{
    [SerializeField] private Animator myAnimationcontroller;

    private void OnTriggerEnter(Collider other) {
        myAnimationcontroller.SetBool("Trigger", true);
    }

    private void OnTriggerExit(Collider other) {
     myAnimationcontroller.SetBool("Trigger" ,false); 
    }
       

        
  

}
