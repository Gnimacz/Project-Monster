using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelWithLoadingScreen : MonoBehaviour
{
    float minLoadTime = 3f;
    float loadTime = 0f;
    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel(){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LevelLoadData.levelName);
        asyncLoad.allowSceneActivation = false;
        loadTime = 0f;
        while(!asyncLoad.isDone){
            loadTime += Time.deltaTime;
            Debug.LogWarning("Loading progress: " + asyncLoad.progress);
            if(asyncLoad.progress >= 0.9f && loadTime >= minLoadTime){
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
