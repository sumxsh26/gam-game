using System.Collections;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    public GameObject spikePrefab;
    public Transform[] spawnPoints;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 1.5f;
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
                int spikeCount = Random.Range(1, 2); // 2-3 spikes at a time

                for (int i = 0; i < spikeCount; i++)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject spike = Instantiate(spikePrefab, spawnPoint.position, Quaternion.identity);

                    Rigidbody2D rb = spike.GetComponent<Rigidbody2D>();
                    if (rb == null)
                    {
                        rb = spike.AddComponent<Rigidbody2D>();
                    }

                    rb.gravityScale = 2; // Ensure spikes fall
                    rb.linearVelocity = Vector2.down * 0.1f; // Small push to ensure falling
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
