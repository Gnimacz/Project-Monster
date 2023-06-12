using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[CustomEditor(typeof(SimpleLevelSwitch))]
public class LevelSwitchSelector : Editor
{
    SimpleLevelSwitch levelSwitchScript;
    string selectedScene;
    private void OnEnable()
    {
        levelSwitchScript = (SimpleLevelSwitch)target;
    }

    public override void OnInspectorGUI()
    {
        // DrawDefaultInspector();
        GUILayout.Space(10);
        GUILayout.Label("Select Level to Load");
        selectedScene = levelSwitchScript.levelToLoad;

        //loop over all scenes in build settings and add them to a list
        List<string> sceneNames = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            sceneNames.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
        //convert list to array
        string[] sceneNamesArray = sceneNames.ToArray();
        //get the index of the current scene
        int currentSceneIndex = sceneNames.FindIndex(x => x == selectedScene);
        //display a popup with the current scene selected
        currentSceneIndex = EditorGUILayout.Popup("", currentSceneIndex, sceneNamesArray);
        //if the selected scene is different from the current scene, load the selected scene
        if (currentSceneIndex != sceneNames.FindIndex(x => x == selectedScene))
        {
            levelSwitchScript.levelToLoad = sceneNamesArray[currentSceneIndex];
        }
    }
}
