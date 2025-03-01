//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class MenuFunctions : MonoBehaviour
//{
//    public void GoToInstructions()
//    {
//        SceneManager.LoadSceneAsync("Instructions");
//    }

//    public void PlayGame()
//    {
//        SceneManager.LoadSceneAsync("Movement");
//    }

//    public void GoToCredits()
//    {
//        SceneManager.LoadSceneAsync("Credits");
//    }

//    public void GoToSettings()
//    {
//        SceneManager.LoadSceneAsync("Settings");
//    }

//    public void QuitGame()
//    {
//        Application.Quit();
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void GoToCutscene()
    {
        SceneManager.LoadSceneAsync("Cutscene");
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Test");
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


