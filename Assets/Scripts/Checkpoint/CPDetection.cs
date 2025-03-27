using UnityEngine;

public class CPDetection : MonoBehaviour
{
    private bool hasBeenActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenActivated && other.CompareTag("Player"))
        {
            hasBeenActivated = true;
            CheckpointManager.Instance.SetCheckpoint(transform.position, transform.rotation);
        }
    }
}
