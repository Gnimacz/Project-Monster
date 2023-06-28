using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

public class ButtonBehaviors : MonoBehaviour
{
    public float timeToWait = 0.5f;
    public string levelToLoad;
    private GameObject currentlySelected;
    public void LoadLevelByName(string levelToLoad)
    {
        LevelLoadData.levelName = levelToLoad;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void ShowPopUp(GameObject popUp)
    {
        //twean the pop up from top to bottom
        popUp.SetActive(true);
        LeanTween.moveX(popUp, 0.5f * Screen.width, timeToWait).setEaseOutBack().setOnComplete(() =>
        {
            MenuPopup popUpComponent = popUp.GetComponent<MenuPopup>();
            if (popUpComponent == null)
            {
                return;
            }
            popUp.GetComponent<MenuPopup>().previousSelectedButton = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(popUp.GetComponent<MenuPopup>()?.selectedButton);
        });
    }
    public void ClosePopUp(GameObject popUp)
    {
        LeanTween.moveX(popUp, 1.5f * Screen.width, timeToWait).setEaseOutBack().setOnComplete(() =>
        {
            popUp.SetActive(false);
            EventSystem.current.SetSelectedGameObject(popUp.GetComponent<MenuPopup>()?.previousSelectedButton);
        });
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }


    bool localeChangeActive = false;
    public void SwitchLanguage(int languageIndex)
    {
        if (localeChangeActive) return;

        StartCoroutine(SwitchLocale(languageIndex));
    }
    IEnumerator SwitchLocale(int languageIndex)
    {
        localeChangeActive = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
        localeChangeActive = false;
    }
}
