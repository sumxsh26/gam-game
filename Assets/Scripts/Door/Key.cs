//using UnityEngine;

//public class Key : MonoBehaviour
//{
//    public int keyCount = 0;
//    public int maxKeys = 3;
//    public GameObject door;
//    private bool doorDestroyed = false;

//    public KeyDisplay keyDisplay;  // Direct reference to KeyDisplay

//    private void Update()
//    {
//        if (keyCount >= maxKeys && !doorDestroyed)
//        {
//            doorDestroyed = true;
//            Destroy(door);
//        }
//    }

//    public void CollectKey()
//    {
//        if (keyCount < maxKeys)
//        {
//            keyCount++;
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Key"))
//        {
//            CollectKey();
//            Destroy(other.gameObject);
//        }
//    }
//}

//using UnityEngine;

//public class Key : MonoBehaviour
//{
//    public GameObject doorPrefab; // Assign the Door prefab in Inspector
//    public Transform doorSpawnPoint; // Assign the left-end spawn point in Inspector

//    private bool doorSpawned = false;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && !doorSpawned)
//        {
//            // Destroy the key upon collection
//            Destroy(gameObject);

//            // Spawn the door at the left-end spawn point
//            GameObject spawnedDoor = Instantiate(doorPrefab, doorSpawnPoint.position, Quaternion.identity);
//            doorSpawned = true;

//            // Get SlidingDoor script and manually assign child references
//            SlidingDoor slidingDoor = spawnedDoor.GetComponent<SlidingDoor>();
//            if (slidingDoor != null)
//            {
//                // Ensure left and right doors are found
//                Transform left = spawnedDoor.transform.Find("DoorLeft");
//                Transform right = spawnedDoor.transform.Find("DoorRight");

//                if (left != null && right != null)
//                {
//                    slidingDoor.AssignDoors(left, right);
//                    slidingDoor.OpenDoor();
//                }
//                else
//                {
//                    Debug.LogError("ERROR: Could not find DoorLeft or DoorRight in spawned door!");
//                }
//            }
//            else
//            {
//                Debug.LogError("ERROR: SlidingDoor script is missing on the spawned door!");
//            }

//            Debug.Log("Key collected! Door spawned and opened.");
//        }
//    }
//}



//using UnityEngine;

//public class Key : MonoBehaviour
//{
//    public GameObject doorPrefab; // Assign the Door prefab in Inspector
//    public Transform doorSpawnPoint; // Assign the left-end spawn point in Inspector
//    public GameObject completeZone; // Assign the scene-specific CompleteZone in Inspector

//    private bool doorSpawned = false;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && !doorSpawned)
//        {
//            // Destroy the key upon collection
//            Destroy(gameObject);

//            // Debugging: Check if CompleteZone is assigned
//            if (completeZone == null)
//            {
//                Debug.LogError("ERROR: CompleteZone is NOT assigned in Key script!");
//                return; // Stop execution to prevent passing a null reference
//            }

//            Debug.Log($"CompleteZone assigned successfully in Key script: {completeZone.name}");

//            // Spawn the door at the left-end spawn point
//            GameObject spawnedDoor = Instantiate(doorPrefab, doorSpawnPoint.position, Quaternion.identity);
//            doorSpawned = true;

//            // Get SlidingDoor script and manually assign child references
//            SlidingDoor slidingDoor = spawnedDoor.GetComponent<SlidingDoor>();
//            if (slidingDoor != null)
//            {
//                // Ensure left and right doors are found
//                Transform left = spawnedDoor.transform.Find("DoorLeft");
//                Transform right = spawnedDoor.transform.Find("DoorRight");

//                if (left != null && right != null)
//                {
//                    slidingDoor.AssignDoors(left, right);

//                    // Debugging: Make sure AssignCompleteZone is called correctly
//                    Debug.Log($"Passing CompleteZone '{completeZone.name}' to SlidingDoor.");

//                    slidingDoor.AssignCompleteZone(completeZone); // Ensure this is NOT null
//                    slidingDoor.OpenDoor();
//                }
//                else
//                {
//                    Debug.LogError("ERROR: Could not find DoorLeft or DoorRight in spawned door!");
//                }
//            }
//            else
//            {
//                Debug.LogError("ERROR: SlidingDoor script is missing on the spawned door!");
//            }

//            Debug.Log("Key collected! Door spawned and opened.");
//        }
//    }
//}


// spawning door

//using UnityEngine;

//public class Key : MonoBehaviour
//{
//    public GameObject doorPrefab; // Assign the Door prefab in Inspector
//    public Transform doorSpawnPoint; // Assign the spawn point in Inspector
//    public GameObject completeZone; // Assign the scene-specific CompleteZone in Inspector

//    private bool doorSpawned = false;

//    private void Start()
//    {
//        // Ensure required objects are assigned
//        if (doorPrefab == null)
//        {
//            Debug.LogError("ERROR: DoorPrefab is not assigned in Key script!");
//        }
//        if (doorSpawnPoint == null)
//        {
//            Debug.LogError("ERROR: DoorSpawnPoint is not assigned in Key script!");
//        }
//        if (completeZone == null)
//        {
//            Debug.LogError("ERROR: CompleteZone is not assigned in Key script!");
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            SpawnDoor();
//            Destroy(gameObject); // Destroy key after spawning the door
//        }
//    }

//    private void SpawnDoor()
//    {
//        if (doorSpawned)
//        {
//            Debug.LogWarning("Door has already been spawned, skipping.");
//            return;
//        }

//        if (doorPrefab == null || doorSpawnPoint == null)
//        {
//            Debug.LogError("ERROR: Cannot spawn door because DoorPrefab or DoorSpawnPoint is missing!");
//            return;
//        }

//        // Spawn the door at the correct position
//        GameObject spawnedDoor = Instantiate(doorPrefab, doorSpawnPoint.position, Quaternion.identity);
//        doorSpawned = true;

//        // Find SlidingDoor component
//        SlidingDoor slidingDoor = spawnedDoor.GetComponent<SlidingDoor>();
//        if (slidingDoor != null)
//        {
//            Transform left = spawnedDoor.transform.Find("DoorLeft");
//            Transform right = spawnedDoor.transform.Find("DoorRight");

//            if (left != null && right != null)
//            {
//                // Ensure doors are assigned BEFORE opening them!
//                slidingDoor.AssignDoors(left, right);

//                if (completeZone != null)
//                {
//                    slidingDoor.AssignCompleteZone(completeZone);
//                }
//                else
//                {
//                    Debug.LogError("ERROR: CompleteZone is NULL in Key script! Assign it in the Inspector.");
//                }

//                // Open the door only AFTER AssignDoors() is complete
//                slidingDoor.OpenDoor();

//                Debug.Log("Key collected! Door spawned and opened.");
//            }
//            else
//            {
//                Debug.LogError("ERROR: Could not find DoorLeft or DoorRight in spawned door!");
//            }
//        }
//        else
//        {
//            Debug.LogError("ERROR: SlidingDoor script is missing on the spawned door!");
//        }
//    }

//}


//using UnityEngine;

//public class Key : MonoBehaviour
//{
//    public GameObject exitDoor; // Assign "Exit" GameObject in Inspector
//    public GameObject completeZone; // Assign CompleteZone in Inspector

//    private static int totalKeys = 3; // Total keys required
//    private static int collectedKeys = 0; // Keys collected so far
//    private SlidingDoor slidingDoor;

//    private void Start()
//    {
//        // Ensure Exit GameObject is assigned
//        if (exitDoor == null)
//        {
//            Debug.LogError("ERROR: Exit door (Exit GameObject) is not assigned in Key script! Assign it in the Inspector.");
//            return;
//        }

//        // Get SlidingDoor component from Exit
//        slidingDoor = exitDoor.GetComponent<SlidingDoor>();
//        if (slidingDoor == null)
//        {
//            Debug.LogError("ERROR: SlidingDoor script is missing on the Exit GameObject!");
//            return;
//        }

//        // Assign DoorLeft and DoorRight dynamically
//        Transform left = exitDoor.transform.Find("DoorLeft");
//        Transform right = exitDoor.transform.Find("DoorRight");

//        if (left != null && right != null)
//        {
//            slidingDoor.AssignDoors(left, right);
//        }
//        else
//        {
//            Debug.LogError("ERROR: Could not find DoorLeft or DoorRight in Exit GameObject!");
//        }

//        // Assign CompleteZone if available
//        if (completeZone != null)
//        {
//            slidingDoor.AssignCompleteZone(completeZone);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            collectedKeys++;

//            Debug.Log($"Key collected! ({collectedKeys}/{totalKeys})");

//            // Destroy key after pickup
//            Destroy(gameObject);

//            // If all keys are collected, open the door
//            if (collectedKeys >= totalKeys)
//            {
//                OpenDoor();
//            }
//        }
//    }

//    private void OpenDoor()
//    {
//        if (slidingDoor != null)
//        {
//            slidingDoor.OpenDoor();
//            Debug.Log("All keys collected! Door opened.");
//        }
//    }
//}

using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject exitDoor; // Assign "Exit" GameObject in Inspector
    public GameObject completeZone; // Assign CompleteZone in Inspector

    private static int totalKeys = 3; // Total keys required
    private static int collectedKeys = 0; // Keys collected so far
    private SlidingDoor slidingDoor;

    private void Start()
    {
        // Ensure Exit GameObject is assigned
        if (exitDoor == null)
        {
            Debug.LogError("ERROR: Exit door (Exit GameObject) is not assigned in Key script! Assign it in the Inspector.");
            return;
        }

        // Get SlidingDoor component from Exit
        slidingDoor = exitDoor.GetComponent<SlidingDoor>();
        if (slidingDoor == null)
        {
            Debug.LogError("ERROR: SlidingDoor script is missing on the Exit GameObject!");
            return;
        }

        // Assign DoorLeft and DoorRight dynamically
        Transform left = exitDoor.transform.Find("DoorLeft");
        Transform right = exitDoor.transform.Find("DoorRight");

        if (left != null && right != null)
        {
            slidingDoor.AssignDoors(left, right);
        }
        else
        {
            Debug.LogError("ERROR: Could not find DoorLeft or DoorRight in Exit GameObject!");
        }

        // Assign CompleteZone if available
        if (completeZone != null)
        {
            slidingDoor.AssignCompleteZone(completeZone);
            completeZone.SetActive(false); // Disable CompleteZone at start
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            collectedKeys++;

            Debug.Log($"Key collected! ({collectedKeys}/{totalKeys})");

            // Destroy key after pickup
            Destroy(gameObject);

            // If all keys are collected, open the door & enable CompleteZone
            if (collectedKeys >= totalKeys)
            {
                OpenDoor();
                EnableCompleteZone();
            }
        }
    }

    private void OpenDoor()
    {
        if (slidingDoor != null)
        {
            slidingDoor.OpenDoor();
            Debug.Log("All keys collected! Door opened.");
        }
    }

    private void EnableCompleteZone()
    {
        if (completeZone != null)
        {
            completeZone.SetActive(true);
            Debug.Log("Complete Zone enabled!");
        }
        else
        {
            Debug.LogError("ERROR: CompleteZone is NULL in Key script! Assign it in the Inspector.");
        }
    }
}

