using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private GameController gameController;

    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player fell out of bounds!");  // Check if it’s detecting correctly

            // Trigger game over sequence
            gameController.PlayerMovement.TriggerPlayerDeath();

            //destroyed player after fall
            Destroy(collision.gameObject);

            // Disable player movement
            collision.GetComponent<PlayerMovement>().enabled = false;

        }
    }
}