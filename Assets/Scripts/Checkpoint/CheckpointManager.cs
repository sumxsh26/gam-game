//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CheckpointManager : MonoBehaviour
//{
//    public static CheckpointManager Instance { get; private set; }

//    private Vector3 lastCheckpointPos;
//    private Quaternion lastCheckpointRot;
//    private bool checkpointSet = false;

//    private GameObject player;
//    private PlayerMovement playerMovement;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//        if (player != null)
//        {
//            playerMovement = player.GetComponent<PlayerMovement>();
//            Debug.Log("[CheckpointManager] Found player on scene load");
//        }
//        else
//        {
//            Debug.LogWarning("[CheckpointManager] No player found on scene load");
//        }
//    }

//    public void SetCheckpoint(Vector3 position, Quaternion rotation)
//    {
//        // If not yet set, or position is different, update
//        if (!checkpointSet || position != lastCheckpointPos)
//        {
//            lastCheckpointPos = position;
//            lastCheckpointRot = rotation;
//            checkpointSet = true;

//            if (playerMovement != null)
//            {
//                playerMovement.SaveMouseState();
//            }

//            Debug.Log("[CheckpointManager] New checkpoint set at " + position);
//        }
//        else
//        {
//            Debug.Log("[CheckpointManager] Checkpoint already set here. Ignoring.");
//        }
//    }

//    //public void RespawnAtCheckpoint()
//    //{
//    //    Debug.Log("[CheckpointManager] RespawnAtCheckpoint called");

//    //    // If no checkpoint has been set, restart the entire scene
//    //    if (!checkpointSet)
//    //    {
//    //        Debug.LogWarning("[CheckpointManager] No checkpoint set. Restarting scene...");
//    //        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    //        return;
//    //    }

//    //    // Ensure player reference is valid
//    //    if (playerMovement == null)
//    //    {
//    //        player = GameObject.FindGameObjectWithTag("Player");
//    //        playerMovement = player != null ? player.GetComponent<PlayerMovement>() : null;
//    //    }

//    //    if (playerMovement == null)
//    //    {
//    //        Debug.LogError("[CheckpointManager] No player found, even though checkpoint is set!");
//    //        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // fallback
//    //        return;
//    //    }

//    //    // Log what we're about to do
//    //    Debug.Log("[CheckpointManager] Respawning to position: " + lastCheckpointPos);

//    //    // Reset Rigidbody BEFORE moving the player
//    //    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
//    //    rb.linearVelocity = Vector2.zero;
//    //    rb.angularVelocity = 0f;
//    //    rb.constraints = RigidbodyConstraints2D.FreezeRotation;

//    //    // Move player to checkpoint position
//    //    player.transform.position = lastCheckpointPos;
//    //    player.transform.rotation = lastCheckpointRot;

//    //    // Log after setting position
//    //    Debug.Log("[CheckpointManager] Player position after respawn: " + player.transform.position);

//    //    // Reset internal movement state
//    //    playerMovement.ResetHealthAndState();
//    //    playerMovement.RestoreSavedMouseImmediately();

//    //    // Rebind GameController to the newly reset player
//    //    GameController.Instance?.RebindPlayer();
//    //    Debug.Log("[CheckpointManager] Called GameController.RebindPlayer()");
//    //}


//    public void RespawnAtCheckpoint()
//    {
//        Debug.Log("[CheckpointManager] RespawnAtCheckpoint called");

//        // If no checkpoint has been set, restart the entire scene
//        if (!checkpointSet)
//        {
//            Debug.LogWarning("[CheckpointManager] No checkpoint set. Restarting scene...");
//            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//            return;
//        }

//        // Ensure player reference is valid
//        if (playerMovement == null)
//        {
//            player = GameObject.FindGameObjectWithTag("Player");
//            playerMovement = player != null ? player.GetComponent<PlayerMovement>() : null;
//        }

//        if (playerMovement == null)
//        {
//            Debug.LogError("[CheckpointManager] No player found, even though checkpoint is set!");
//            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // fallback
//            return;
//        }

//        // Log what we're about to do
//        Debug.Log("[CheckpointManager] Respawning to position: " + lastCheckpointPos);

//        // Reset Rigidbody BEFORE moving the player
//        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
//        rb.linearVelocity = Vector2.zero;
//        rb.angularVelocity = 0f;
//        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

//        // Move player to checkpoint position
//        player.transform.position = lastCheckpointPos;
//        player.transform.rotation = lastCheckpointRot;

//        // Log after setting position
//        Debug.Log("[CheckpointManager] Player position after respawn: " + player.transform.position);

//        // Reset internal movement state
//        playerMovement.ResetHealthAndState();
//        MiceManager.Instance?.ResetAllMice(playerMovement.blueMousePrefab, playerMovement.redMousePrefab);
//        playerMovement.RestoreSavedMouseImmediately();

//        // Rebind GameController to the newly reset player
//        GameController.Instance?.RebindPlayer();
//        Debug.Log("[CheckpointManager] Called GameController.RebindPlayer()");
//    }



//    //public void ClearCheckpointData()
//    //{
//    //    Debug.Log("[CheckpointManager] Clearing checkpoint data");

//    //    checkpointSet = false;
//    //    lastCheckpointPos = Vector3.zero;
//    //    lastCheckpointRot = Quaternion.identity;

//    //    if (playerMovement != null)
//    //    {
//    //        playerMovement.ClearMouseCheckpointData();
//    //    }
//    //}

//    public void ClearCheckpointData()
//    {
//        Debug.Log("[CheckpointManager] Clearing checkpoint data");

//        checkpointSet = false;
//        lastCheckpointPos = Vector3.zero;
//        lastCheckpointRot = Quaternion.identity;

//        if (playerMovement != null)
//        {
//            playerMovement.ClearMouseCheckpointData();
//        }

//        // Reset all torch animations in the scene
//        CPTorch[] torches = FindObjectsByType<CPTorch>(FindObjectsSortMode.None);
//        foreach (CPTorch torch in torches)
//        {
//            torch.ResetTorch();
//        }
//    }


//    private void OnDestroy()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    //private void OnTriggerEnter2D(Collider2D other)
//    //{
//    //    if (other.CompareTag("Player") && !checkpointSet)
//    //    {
//    //        checkpointSet = true;

//    //        player = other.gameObject;
//    //        playerMovement = player.GetComponent<PlayerMovement>();

//    //        lastCheckpointPos = transform.position;
//    //        lastCheckpointRot = transform.rotation;

//    //        playerMovement.SaveMouseState();


//    //        Debug.Log("[Checkpoint] Triggered at position: " + transform.position);
//    //    }

//    //}

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && !checkpointSet)
//        {
//            checkpointSet = true;

//            player = other.gameObject;
//            playerMovement = player.GetComponent<PlayerMovement>();

//            lastCheckpointPos = transform.position;
//            lastCheckpointRot = transform.rotation;

//            playerMovement.SaveMouseState();

//            // Light up the torch animation
//            CPTorch torch = GetComponent<CPTorch>();
//            if (torch != null)
//            {
//                torch.TriggerLightup();
//            }

//            // Inform the global checkpoint manager
//            CheckpointManager.Instance.SetCheckpoint(transform.position, transform.rotation);

//            Debug.Log("[Checkpoint] Triggered at position: " + transform.position);
//        }
//    }

//}

// multiple checkpoints
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private GameObject player;
    private PlayerMovement playerMovement;

    private Checkpoint currentCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player?.GetComponent<PlayerMovement>();
    }

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        if (currentCheckpoint == null || checkpoint.CheckpointID > currentCheckpoint.CheckpointID)
        {
            currentCheckpoint = checkpoint;
            Debug.Log("[CheckpointManager] New checkpoint set: " + checkpoint.CheckpointID);

            playerMovement?.SaveMouseState();
        }
    }

    public void RespawnAtCheckpoint()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogWarning("[CheckpointManager] No checkpoint set. Restarting scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (playerMovement == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player?.GetComponent<PlayerMovement>();
        }

        if (playerMovement == null)
        {
            Debug.LogError("[CheckpointManager] Player missing. Reloading scene.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // Move and reset player
        player.transform.position = currentCheckpoint.transform.position;
        player.transform.rotation = currentCheckpoint.transform.rotation;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        playerMovement.ResetHealthAndState();

        // First reset all mice and platforms in the scene
        MiceManager.Instance?.ResetAllMice(playerMovement.blueMousePrefab, playerMovement.redMousePrefab);
        FallingPlatformManager.Instance?.ResetAllPlatforms();

        // THEN restore the saved mouse
        playerMovement.RestoreSavedMouseImmediately();

        GameController.Instance?.RebindPlayer();

    }


    public void ClearCheckpointData()
    {
        currentCheckpoint = null;
        playerMovement?.ClearMouseCheckpointData();

        foreach (var cp in FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))
        {
            cp.ResetCheckpoint();
        }
    }
}
