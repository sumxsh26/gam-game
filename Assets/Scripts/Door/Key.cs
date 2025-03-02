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

using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject doorPrefab; // Assign the Door prefab in Inspector
    public Transform doorSpawnPoint; // Assign the left-end spawn point in Inspector

    private bool doorSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !doorSpawned)
        {
            // Destroy the key upon collection
            Destroy(gameObject);

            // Spawn the door at the left-end spawn point
            GameObject spawnedDoor = Instantiate(doorPrefab, doorSpawnPoint.position, Quaternion.identity);
            doorSpawned = true;

            // Get SlidingDoor script and manually assign child references
            SlidingDoor slidingDoor = spawnedDoor.GetComponent<SlidingDoor>();
            if (slidingDoor != null)
            {
                // Ensure left and right doors are found
                Transform left = spawnedDoor.transform.Find("DoorLeft");
                Transform right = spawnedDoor.transform.Find("DoorRight");

                if (left != null && right != null)
                {
                    slidingDoor.AssignDoors(left, right);
                    slidingDoor.OpenDoor();
                }
                else
                {
                    Debug.LogError("ERROR: Could not find DoorLeft or DoorRight in spawned door!");
                }
            }
            else
            {
                Debug.LogError("ERROR: SlidingDoor script is missing on the spawned door!");
            }

            Debug.Log("Key collected! Door spawned and opened.");
        }
    }
}




