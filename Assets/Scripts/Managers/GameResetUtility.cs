using UnityEngine;

public static class GameResetUtility
{
    public static void CleanupPersistentManagers()
    {
        if (FallingPlatformManager.Instance != null)
        {
            FallingPlatformManager.Instance.DestroyPlatformsOnLevelTransition();
        }

        if (FallingSpikeManager.Instance != null)
        {
            FallingSpikeManager.Instance.DestroySpikesOnLevelTransition();
        }

        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.DestroyCheckpointsOnLevelTransition();
        }

        Debug.Log("[GameResetUtility] Persistent managers cleaned up.");
    }
}
