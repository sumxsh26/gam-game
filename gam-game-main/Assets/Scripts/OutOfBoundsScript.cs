using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private GameControllerScript gameController;

    private void Start()
    {
        gameController = FindFirstObjectByType<GameControllerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player fell out of bounds!");  // Check if it’s detecting correctly

            // Trigger game over sequence
            gameController.PlayerController.TriggerPlayerDeath();

            //destroyed player after fall
            Destroy(collision.gameObject);

            // Disable player movement
            collision.GetComponent<PlayerController>().enabled = false;

        }
    }
}