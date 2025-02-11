using System.Collections;
using UnityEngine;

public class FallingSpikesController : MonoBehaviour
{
    public float minSpawnInterval = 0.5f; // Minimum time between falls
    public float maxSpawnInterval = 1.5f; // Maximum time between falls
    public GameObject fallingObject;
    public Transform[] spawnPoints; // Assign spawn points in Inspector

    private bool isPlayerInZone = false;

    void Start()
    {
        StartCoroutine(SpawnSpikes());
    }

    IEnumerator SpawnSpikes()
    {
        while (true)
        {
            if (isPlayerInZone)
            {
                int spikeCount = Random.Range(2, 5); // Controls how many spikes fall at once

                for (int i = 0; i < spikeCount; i++)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    Instantiate(fallingObject, spawnPoint.position, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}
