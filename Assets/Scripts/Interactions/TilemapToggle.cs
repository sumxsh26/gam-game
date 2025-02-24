////using UnityEngine;
////using UnityEngine.Tilemaps;
////using UnityEngine.UI;  // Add this to work with UI

////public class TilemapToggle : MonoBehaviour
////{
////    private TilemapRenderer tilemapRenderer;
////    private TilemapCollider2D tilemapCollider;

////    public float displayTime = 3f;
////    public float cooldownTime = 30f;
////    private float lastToggleTime = -30f;

////    public Slider cooldownBar;  // Reference to the UI Slider

////    void Start()
////    {
////        tilemapRenderer = GetComponent<TilemapRenderer>();
////        tilemapCollider = GetComponent<TilemapCollider2D>();

////        tilemapRenderer.enabled = false;
////        tilemapCollider.enabled = true;

////        if (cooldownBar != null)
////        {
////            cooldownBar.maxValue = cooldownTime;  // Set max value to cooldown time
////            cooldownBar.value = cooldownTime;     // Start as fully charged
////        }
////    }

////    void Update()
////    {
////        if (Input.GetKeyDown(KeyCode.Q))
////        {
////            if (Time.time - lastToggleTime >= cooldownTime)
////            {
////                TogglePlatform();
////                lastToggleTime = Time.time;

////                if (cooldownBar != null)
////                {
////                    cooldownBar.value = 0;  // Reset the cooldown bar when ability is used
////                }
////            }
////            else
////            {
////                Debug.Log("Cooldown in progress! Please wait.");
////            }
////        }

////        // Update the cooldown bar over time
////        if (cooldownBar != null && cooldownBar.value < cooldownTime)
////        {
////            cooldownBar.value += Time.deltaTime;  // Refill bar gradually
////        }
////    }

////    void TogglePlatform()
////    {
////        tilemapRenderer.enabled = !tilemapRenderer.enabled;
////        Invoke("HideVisuals", displayTime);
////    }

////    void HideVisuals()
////    {
////        tilemapRenderer.enabled = false;
////    }
////}


////// toggle version where both visibility and collider are gone
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.UI;

//public class TilemapToggle : MonoBehaviour
//{
//    private TilemapRenderer tilemapRenderer;
//    private BoxCollider2D boxCollider;

//    public float displayTime = 3f;
//    public float cooldownTime = 30f;
//    private float lastToggleTime = -30f;

//    public Slider cooldownBar;

//    void Start()
//    {
//        tilemapRenderer = GetComponent<TilemapRenderer>();
//        boxCollider = GetComponent<BoxCollider2D>();

//        tilemapRenderer.enabled = false;
//        boxCollider.enabled = false;  // Initially disable collider

//        if (cooldownBar != null)
//        {
//            cooldownBar.maxValue = cooldownTime;
//            cooldownBar.value = cooldownTime;
//        }
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            if (Time.time - lastToggleTime >= cooldownTime)
//            {
//                TogglePlatform();
//                lastToggleTime = Time.time;

//                if (cooldownBar != null)
//                {
//                    cooldownBar.value = 0;
//                }
//            }
//            else
//            {
//                Debug.Log("Cooldown in progress! Please wait.");
//            }
//        }

//        if (cooldownBar != null && cooldownBar.value < cooldownTime)
//        {
//            cooldownBar.value += Time.deltaTime;
//        }
//    }

//    void TogglePlatform()
//    {
//        bool isCurrentlyVisible = tilemapRenderer.enabled;

//        if (!isCurrentlyVisible)
//        {
//            EnablePlatform();
//        }
//        else
//        {
//            DisablePlatform();
//        }
//    }

//    void EnablePlatform()
//    {
//        tilemapRenderer.enabled = true;
//        boxCollider.enabled = true;

//        // Ensure players are pushed out safely
//        PushUpPlayers();

//        Invoke("DisablePlatform", displayTime);
//    }

//    void DisablePlatform()
//    {
//        tilemapRenderer.enabled = false;
//        boxCollider.enabled = false;
//    }

//    void PushUpPlayers()
//    {
//        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0);
//        foreach (Collider2D collider in colliders)
//        {
//            if (collider.CompareTag("Player"))
//            {
//                Rigidbody2D playerRb = collider.GetComponent<Rigidbody2D>();
//                if (playerRb != null)
//                {
//                    Vector2 pushUp = Vector2.up * 5f; // Push upwards
//                    playerRb.position += pushUp * 0.2f; // Small instant reposition to avoid overlap
//                    playerRb.linearVelocity = pushUp; // Apply force upwards
//                }
//            }
//        }
//    }
//}


// updated - linked to toggling 
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilemapToggle : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private BoxCollider2D boxCollider;

    public float displayTime = 3f;
    public float cooldownTime = 30f;
    private float lastToggleTime = -30f;

    public Slider cooldownBar;
    private int availableToggles = 0; // Mice used for toggling

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        tilemapRenderer.enabled = false;
        boxCollider.enabled = false;

        if (cooldownBar != null)
        {
            cooldownBar.maxValue = cooldownTime;
            cooldownBar.value = cooldownTime;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time - lastToggleTime >= cooldownTime && availableToggles > 0)
            {
                TogglePlatform();
                lastToggleTime = Time.time;
                availableToggles--; // Use one toggle

                if (cooldownBar != null)
                {
                    cooldownBar.value = 0;
                }
            }
            else
            {
                Debug.Log("Cannot activate! Either cooldown is in progress or no mice collected.");
            }
        }

        if (cooldownBar != null && cooldownBar.value < cooldownTime)
        {
            cooldownBar.value += Time.deltaTime;
        }
    }

    void TogglePlatform()
    {
        bool isCurrentlyVisible = tilemapRenderer.enabled;

        if (!isCurrentlyVisible)
        {
            EnablePlatform();
        }
        else
        {
            DisablePlatform();
        }
    }

    void EnablePlatform()
    {
        tilemapRenderer.enabled = true;
        boxCollider.enabled = true;
        Invoke("DisablePlatform", displayTime);
    }

    void DisablePlatform()
    {
        tilemapRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    public void AddMouse()
    {
        availableToggles++;
    }
}


