using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Singleton
    public static LevelManager Instance { get; private set; }

    public string nextLevelName;
    public string currentLevelName;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Changelevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public static void Changelevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public static void Changelevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
