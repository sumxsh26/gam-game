using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointID = 0;
    private bool isActivated = false;
    private AudioManager audioManager;
    private CPTorch torch;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        torch = GetComponent<CPTorch>();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!isActivated && other.CompareTag("Player"))
    //    {
    //        isActivated = true;

    //        Debug.Log("[Checkpoint] Activated checkpoint ID: " + CheckpointID);

    //        CheckpointManager.Instance?.RegisterCheckpoint(this);

    //        if (audioManager != null && audioManager.checkPoint != null)
    //        {
    //            audioManager.PlaySFX(audioManager.checkPoint);
    //        }

    //        torch?.TriggerLightup();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;

            Debug.Log("[Checkpoint] Activated checkpoint ID: " + CheckpointID);

            CheckpointManager.Instance?.RegisterCheckpoint(this);

            // Fetch AudioManager again in case it was destroyed and reloaded
            if (audioManager == null)
            {
                audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
            }

            if (audioManager != null && audioManager.checkPoint != null)
            {
                audioManager.PlaySFX(audioManager.checkPoint);
            }

            torch?.TriggerLightup();
        }
    }


    public void ResetCheckpoint()
    {
        isActivated = false;
        torch?.ResetTorch();
    }
}
