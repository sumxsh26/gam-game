using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    public float fallThreshold = -18f; // Y-coordinate where the player falls out of bounds
    public bool isFalling = false; // To check if the player is falling
    private GameControllerScript gameController;
    public PlayerController playerController; // Drag PlayerController object into this in the Inspector


    [System.Obsolete]
    void Start()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>(); // Or assign it in another way
        }
    }

    private void Update()
    {
        // Print the player's current Y position to the console for debugging
        //Debug.Log("Player Y Position: " + transform.position.y);

        // If the player's Y position falls below the threshold, it's out of bounds
        if (transform.position.y < fallThreshold)
        {
            TriggerGameOver();
        }
    }

    // Function to trigger game over
    private void TriggerGameOver()
    {
        if (!isFalling) // To prevent triggering multiple times
        {
            isFalling = true;

            // Trigger your game over logic here
            Debug.Log("Player died from a high place");

            gameController.PlayerController.TriggerPlayerDeath();

            
            
        }
    }
}
