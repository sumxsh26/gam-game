using UnityEngine;

public class PickupMouse : MonoBehaviour
{
    private Mice parentMouse;

    private void Awake()
    {
        parentMouse = GetComponentInParent<Mice>();
        if (parentMouse == null)
        {
            Debug.LogError("PickupMouse script is missing a reference to its parent Mice script.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered pickup zone.");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Only pick up if the button was actually pressed
            if (InputManager.PickupWasPressed)
            {
                Debug.Log("Player pressed pickup button while in pickup zone.");
                TryPickup(other);
            }
        }
    }

    private void TryPickup(Collider2D playerCollider)
    {
        PlayerMovement playerMovement = playerCollider.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            Debug.Log("Mouse successfully picked up!");
            playerMovement.PickupMouse(parentMouse);

            // Reset pickup input after successful pickup to prevent double pickups
            InputManager.PickupWasPressed = false;
        }
    }


}
