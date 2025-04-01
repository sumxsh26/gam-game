using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private GameObject player;
    private PlayerMovement playerMovement;

    private Checkpoint currentCheckpoint;

    [Header("Resettable Doors")]
    public List<DoorBehaviour> allDoors = new();

    [Header("Scene Timer")]
    public TimerManager sceneTimer;
    private float savedTimerAtCheckpoint = -1f;



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

            if (sceneTimer != null)
            {
                savedTimerAtCheckpoint = sceneTimer.GetCurrentTime();
                Debug.Log("[CheckpointManager] Timer saved at: " + savedTimerAtCheckpoint);
            }
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
        FallingSpikeManager.Instance?.ResetAllSpikes();

        foreach (var door in allDoors)
        {
            if (door != null)
                door.ResetDoor();
        }

        playerMovement.RestoreSavedMouseImmediately();

        GameController.Instance?.RebindPlayer();

        sceneTimer?.ResetTimer(GetSavedTimerTime());

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

    // when player goes to the next level
    public void DestroyCheckpointsOnLevelTransition()
    {
        currentCheckpoint = null;

        // Reset mouse state
        playerMovement?.ClearMouseCheckpointData();

        // Optional: Clean up any state for doors or other persistent systems
        allDoors.Clear();

        // Unsubscribe from scene events to avoid duplicates if this gets recreated
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Destroy this singleton instance so a new one is created in the next scene
        Destroy(gameObject);
    }

    // for timer

    public float GetSavedTimerTime()
    {
        return savedTimerAtCheckpoint > 0 ? savedTimerAtCheckpoint : sceneTimer.startingTime;
    }



}
