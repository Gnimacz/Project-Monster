using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SimpleLevelSwitch : MonoBehaviour
{

    [HideInInspector] public string levelToLoad;
    public UnityEvent onLevelSwitch;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LevelLoadData.levelName = levelToLoad;
            onLevelSwitch?.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    public void SceneSwitchAnimation(GameObject target)
    {
        LeanTween.rotate(target, new Vector3(0, 0, 3600), 0.8f).setEase(LeanTweenType.easeInCubic);
        LeanTween.scale(target, Vector3.one, 0.8f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => SceneManager.LoadScene("LoadingScreen"));
    }
}
