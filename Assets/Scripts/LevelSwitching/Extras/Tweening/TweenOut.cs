using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOut : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public void StartFadeOut()
    {
        LeanTween.scale(target, Vector3.zero, 1f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => Destroy(gameObject));
        // LeanTween.alpha(target, 0f, 1f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => Destroy(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
