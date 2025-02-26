using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void GoToInstructions()
    {
        SceneManager.LoadSceneAsync("Instructions");
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("PlayTest 1");
    }

    public void GoToCredits()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    public void GoToSettings()
    {
        SceneManager.LoadSceneAsync("Settings");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
