using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player completed Level One. Loading next scene...");
            SceneManager.LoadScene("Alpha"); // go to Alpha scene after passing level one door
        }
    }
}
