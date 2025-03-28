////using UnityEngine;

////public class CPDetection : MonoBehaviour
////{
////    //private bool hasBeenActivated = false;

////    //private void OnTriggerEnter2D(Collider2D other)
////    //{
////    //    if (!hasBeenActivated && other.CompareTag("Player"))
////    //    {
////    //        hasBeenActivated = true;
////    //        CheckpointManager.Instance.SetCheckpoint(transform.position, transform.rotation);
////    //    }
////    //}

////    private void OnTriggerEnter2D(Collider2D other)
////    {
////        // Check if the collider belongs to the player (using the player tag)
////        if (other.CompareTag("Player"))
////        {
////            Debug.Log($"[CPDetection] Triggered by: {other.gameObject.name}");

////            // Set checkpoint data (could be used for respawning later)
////            CheckpointManager.Instance.SetCheckpoint(transform.position, transform.rotation);

////            // Trigger light-up or any other logic
////            CPTorch torch = GetComponent<CPTorch>();
////            if (torch != null)
////            {
////                torch.TriggerLightup();
////            }
////        }
////        else
////        {
////            Debug.Log($"[CPDetection] Triggered by non-player object: {other.gameObject.name}");
////        }
////    }


////}



////using UnityEngine;

////public class CPDetection : MonoBehaviour
////{
////    private bool checkpointActivated = false; // Flag to track if checkpoint is already activated

////    private void OnTriggerEnter2D(Collider2D other)
////    {
////        // Ensure we are triggering only for the player
////        if (other.CompareTag("Player"))
////        {
////            if (!checkpointActivated) // Check if the checkpoint has already been activated
////            {
////                checkpointActivated = true; // Mark the checkpoint as activated
////                Debug.Log("[CPDetection] Triggered by player, activating checkpoint.");

////                // Add your logic to activate the checkpoint (e.g., save position, trigger animation, etc.)
////                CheckpointManager.Instance.SetCheckpoint(transform.position, transform.rotation);

////                // Optionally, trigger animation or light-up (example from your earlier scripts)
////                CPTorch torch = GetComponent<CPTorch>();
////                if (torch != null)
////                {
////                    torch.TriggerLightup();
////                }
////            }
////            else
////            {
////                Debug.Log("[CPDetection] Checkpoint already activated, ignoring trigger.");
////            }
////        }
////    }

////    // Optional: If you want to reset the checkpoint on scene restart, you can use the following method
////    public void ResetCheckpoint()
////    {
////        checkpointActivated = false;
////        Debug.Log("[CPDetection] Checkpoint has been reset.");
////    }
////}


//// audio handling
//using UnityEngine;

//public class CPDetection : MonoBehaviour
//{
//    private bool checkpointActivated = false; // Flag to track if checkpoint is already activated

//    private AudioManager audioManager;

//    private void Awake()
//    {
//        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
//        if (audioManager == null)
//        {
//            Debug.LogWarning("[CPDetection] AudioManager not found!");
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        // Ensure we are triggering only for the player
//        if (other.CompareTag("Player"))
//        {
//            if (!checkpointActivated) // Check if the checkpoint has already been activated
//            {
//                checkpointActivated = true;
//                Debug.Log("[CPDetection] Triggered by player, activating checkpoint.");

//                // Play checkpoint sound
//                if (audioManager != null && audioManager.checkPoint != null)
//                {
//                    audioManager.PlaySFX(audioManager.checkPoint);
//                }

//                // Save checkpoint
//                CheckpointManager.Instance.SetCheckpoint(transform.position, transform.rotation);

//                // Light up the torch animation
//                CPTorch torch = GetComponent<CPTorch>();
//                if (torch != null)
//                {
//                    torch.TriggerLightup();
//                }
//            }
//            else
//            {
//                Debug.Log("[CPDetection] Checkpoint already activated, ignoring trigger.");
//            }
//        }
//    }

//    public void ResetCheckpoint()
//    {
//        checkpointActivated = false;
//        Debug.Log("[CPDetection] Checkpoint has been reset.");
//    }
//}

