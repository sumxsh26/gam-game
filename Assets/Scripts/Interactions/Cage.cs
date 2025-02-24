//using UnityEngine;

//public class Cage : MonoBehaviour
//{
//    public bool locked;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        locked = true;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.gameObject.CompareTag("Mice"))
//        {
//            locked = false;
//            Destroy(other.gameObject);
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.gameObject.CompareTag("Mice"))
//        {
//            locked = true;
//        }
//    }
//}

// updated - linked to toggling
using UnityEngine;

public class Cage : MonoBehaviour
{
    public bool locked;
    private int storedMice = 0; // Mice inside the cage

    private void Start()
    {
        locked = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mice"))
        {
            // Get the MiceManager component
            Mice mouse = other.GetComponent<Mice>();

            // **Prevent wild mice from entering the cage**
            if (mouse != null && !mouse.isFollowingPlayer)
            {
                return; // Ignore wild mice that are not carried by the player
            }

            storedMice++;

            // Activate all toggle platforms
            ActivateAllTilemapToggles();

            Destroy(other.gameObject); // Remove mouse
        }
    }

    public void ActivateAllTilemapToggles()
    {
        // Find all TilemapToggle objects in the scene and activate them
        TilemapToggle[] allTilemapToggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);

        if (allTilemapToggles.Length > 0)
        {
            foreach (TilemapToggle toggle in allTilemapToggles)
            {
                toggle.AddMouse(); // Activate every platform
            }
        }
        else
        {
            Debug.LogError("No TilemapToggle objects found in the scene!");
        }
    }
}





