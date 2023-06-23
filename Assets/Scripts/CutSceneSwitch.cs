using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;


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
        if (Input.GetKeyDown(KeyCode.A))
        {
            timer = 0f;
        }
        // Check if the "A" key is being held down
        if (Input.GetKey(KeyCode.A))
        {
            //add Time.deltaTime to the timer value
            timer += Time.deltaTime;

            //check if the timer value is above a certain amount
            if(timer >= timeDelay)
            {
                SwitchToNextScene();
            }
        }


    }

    public void SwitchToNextScene()
    {
        // Load the next scene by name
        //SceneManager.LoadScene(TemplateScene);
        SceneManager.LoadScene(2);
    }

}
