using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("GameplayScene");
    }

    public void GoToCredits()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
