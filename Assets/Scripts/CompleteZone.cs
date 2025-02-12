using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteZone : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Set in the Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Loading scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}