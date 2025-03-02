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
//    private Cage cage; // Reference to track stored mice

//    void Start()
//    {
//        tilemapRenderer = GetComponent<TilemapRenderer>();
//        boxCollider = GetComponent<BoxCollider2D>();

//        tilemapRenderer.enabled = false;
//        boxCollider.enabled = false;

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
//            if (cage != null && cage.CanToggle() && Time.time - lastToggleTime >= cooldownTime)
//            {
//                TogglePlatform();
//                lastToggleTime = Time.time;
//                cage.UseMouseForToggle(); // Consume a stored mouse

//                if (cooldownBar != null)
//                {
//                    cooldownBar.value = 0;
//                }
//            }
//            else
//            {
//                Debug.Log("Cannot activate! Either cooldown is in progress or no mice available.");
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

//        PushUpPlayers(); // Push players out if inside

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

//    // Allow the Cage script to assign itself
//    public void SetCageReference(Cage cageReference)
//    {
//        cage = cageReference;
//    }
//}


//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.UI;
//using System.Linq; // For checking cage conditions

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

//        // Ensure platform starts hidden
//        SetPlatformState(false);

//        if (cooldownBar != null)
//        {
//            cooldownBar.maxValue = cooldownTime;
//            cooldownBar.value = cooldownTime;
//        }
//    }

//    public void TogglePlatform()
//    {
//        if (Time.time - lastToggleTime < cooldownTime)
//        {
//            Debug.Log("Toggle is on cooldown.");
//            return;
//        }

//        lastToggleTime = Time.time;

//        // Toggle platform state
//        bool isCurrentlyVisible = tilemapRenderer.enabled;
//        SetPlatformState(!isCurrentlyVisible);

//        // Start cooldown
//        if (cooldownBar != null)
//        {
//            cooldownBar.value = 0;
//        }

//        // Schedule platform to disappear after displayTime
//        if (!isCurrentlyVisible)
//        {
//            Invoke(nameof(DisablePlatform), displayTime);
//        }
//    }

//    void SetPlatformState(bool isActive)
//    {
//        tilemapRenderer.enabled = isActive;
//        boxCollider.enabled = isActive;

//        if (isActive)
//        {
//            PushUpPlayers();
//        }
//    }

//    void DisablePlatform()
//    {
//        SetPlatformState(false);
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
//                    Vector2 pushUp = Vector2.up * 5f;
//                    playerRb.position += pushUp * 0.2f;
//                    playerRb.linearVelocity = pushUp;
//                }
//            }
//        }
//    }
//}


using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class TilemapToggle : MonoBehaviour
{
    private Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;
    private bool isFlashing = false;

    public float displayTime = 3f; // How long the platform is visible
    public float flashDuration = 0.3f; // How long each flash lasts
    public int flashCount = 5; // Number of flashes before disappearing

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();

        if (tilemap == null || tilemapRenderer == null)
        {
            Debug.LogError("Tilemap or TilemapRenderer not found on " + gameObject.name);
        }

        // Ensure the platform starts invisible
        tilemapRenderer.enabled = false;
    }

    public void TogglePlatform()
    {
        if (!isFlashing)
        {
            StartCoroutine(ShowPlatform());
        }
    }

    private IEnumerator ShowPlatform()
    {
        // Show platform
        tilemapRenderer.enabled = true;

        // Wait for display time
        yield return new WaitForSeconds(displayTime - (flashCount * flashDuration));

        // Start flashing effect before disappearing
        StartCoroutine(FlashBeforeDisappearing());
    }

    private IEnumerator FlashBeforeDisappearing()
    {
        isFlashing = true;

        for (int i = 0; i < flashCount; i++)
        {
            tilemapRenderer.enabled = false;
            yield return new WaitForSeconds(flashDuration / 2);
            tilemapRenderer.enabled = true;
            yield return new WaitForSeconds(flashDuration / 2);
        }

        // Fully hide the platform after flashing
        tilemapRenderer.enabled = false;
        isFlashing = false;
    }
}



