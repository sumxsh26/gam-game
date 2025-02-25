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
//using UnityEngine;

//public class Cage : MonoBehaviour
//{
//    public bool locked;
//    private int storedMice = 0; // Mice inside the cage

//    private void Start()
//    {
//        locked = true;
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Mice"))
//        {
//            Mice mouse = other.GetComponent<Mice>();

//            // **Prevent wild mice from entering the cage**
//            if (mouse != null && mouse.isFollowingPlayer)
//            {
//                if (!mouse.gameObject.activeSelf) return; // Prevent duplicate counting

//                storedMice++;

//                Debug.Log("Mouse stored! Total stored mice: " + storedMice);

//                // Activate all toggle platforms
//                ActivateAllTilemapToggles();

//                // Disable the mouse before destroying to prevent multiple triggers
//                mouse.gameObject.SetActive(false);
//                Destroy(mouse.gameObject);
//            }
//        }
//    }

//    public void ActivateAllTilemapToggles()
//    {
//        TilemapToggle[] allTilemapToggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);

//        if (allTilemapToggles.Length > 0)
//        {
//            foreach (TilemapToggle toggle in allTilemapToggles)
//            {
//                toggle.SetCageReference(this); // Pass reference to allow toggling
//            }
//        }
//        else
//        {
//            Debug.LogError("No TilemapToggle objects found in the scene!");
//        }
//    }

//    // Check if there are stored mice to allow toggling
//    public bool CanToggle()
//    {
//        return storedMice > 0;
//    }

//    // Consume one mouse when toggling
//    public void UseMouseForToggle()
//    {
//        if (storedMice > 0)
//        {
//            storedMice--;
//            Debug.Log("Mouse used! Remaining mice: " + storedMice);
//        }
//        else
//        {
//            Debug.LogWarning("No mice left for toggling!");
//        }
//    }
//}

using UnityEngine;
using System.Collections.Generic;

public class Cage : MonoBehaviour
{
    public bool locked;
    private int storedMice = 0; // Mice inside the cage
    private static List<TilemapToggle> tilemapToggles = new List<TilemapToggle>(); // Global list of toggles

    private void Start()
    {
        locked = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mice"))
        {
            Mice mouse = other.GetComponent<Mice>();

            if (mouse != null && mouse.isFollowingPlayer)
            {
                if (!mouse.gameObject.activeSelf) return; // Prevent duplicate counting

                storedMice++;
                Debug.Log("Mouse stored! Total stored mice: " + storedMice);

                // Disable and destroy the mouse (but don't activate toggle yet)
                mouse.gameObject.SetActive(false);
                Destroy(mouse.gameObject);
            }
        }
    }

    public static void RegisterToggle(TilemapToggle toggle)
    {
        if (!tilemapToggles.Contains(toggle))
        {
            tilemapToggles.Add(toggle);
            Debug.Log("TilemapToggle registered: " + toggle.gameObject.name);
        }
    }

    public static void ToggleAllPlatforms()
    {
        tilemapToggles.RemoveAll(toggle => toggle == null); // Clean up destroyed objects

        Debug.Log("Total tilemap toggles registered: " + tilemapToggles.Count);

        if (tilemapToggles.Count > 0)
        {
            foreach (TilemapToggle toggle in tilemapToggles)
            {
                if (toggle != null)
                {
                    Debug.Log("Toggling platform: " + toggle.gameObject.name);
                    toggle.TogglePlatform();
                }
            }
        }
        else
        {
            Debug.LogError(" No tilemap toggles found. Make sure they are in the scene and correctly registered.");
        }
    }


    public bool CanToggle()
    {
        return storedMice > 0;
    }
}
















