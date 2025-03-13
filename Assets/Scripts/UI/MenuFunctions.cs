using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public string cutsceneSceneName = "Cutscene";
    public string playSceneName = "Test";
    public string creditsSceneName = "Credits";
    public string settingsSceneName = "Settings";

    private void Awake()
    {
        int screenW = 1920;
        int screenH = 1080;
        bool isFullscreen = false;

        Screen.SetResolution(screenW, screenH, isFullscreen);
    }


    public void GoToCutscene()
    {
        SceneManager.LoadSceneAsync(cutsceneSceneName);
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(playSceneName);
    }

    public void GoToCredits()
    {
        SceneManager.LoadSceneAsync(creditsSceneName);
    }

    public void GoToSettings()
    {
        SceneManager.LoadSceneAsync(settingsSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}