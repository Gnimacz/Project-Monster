using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using MonsterInput;


public class CutSceneSwitch : MonoBehaviour
{
    public string TemplateScene;
    public TimelineAsset timeline;
    private bool isHoldingA;
    private bool isTimelineFinished;
    [SerializeField] private float timer;
    [SerializeField] private float timeDelay = 3f;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            timer = 0f;
        }
        // Check if the "A" key is being held down
        if (Input.anyKey)
        {
            //add Time.deltaTime to the timer value
            timer += Time.deltaTime;

            //check if the timer value is above a certain amount
            if(timer >= timeDelay)
            {
                SwitchToNextScene(TemplateScene);
            }
        }

    }

    public void SwitchToNextScene(string SceneToSwitchTo)
    {
        // Load the next scene by name
        //SceneManager.LoadScene(TemplateScene);
        LevelLoadData.levelName = SceneToSwitchTo;
        SceneManager.LoadScene("LoadingScreen");
    }

}
