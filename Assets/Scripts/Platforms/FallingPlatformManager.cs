using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformManager : MonoBehaviour
{
    public static FallingPlatformManager Instance { get; private set; }

    [Header("All Falling Platforms in Scene")]
    public List<FallingPlatformTracker> allPlatforms = new List<FallingPlatformTracker>();

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
        }
    }

    public void ResetAllPlatforms()
    {
        Debug.Log("[FallingPlatformManager] Resetting " + allPlatforms.Count + " platforms");

        foreach (FallingPlatformTracker platform in allPlatforms)
        {
            if (platform != null)
            {
                platform.ResetPlatform();
            }
        }
    }

    public void DestroyPlatformsOnLevelTransition()
    {
        allPlatforms.Clear();

        if (Instance == this)
        {
            Instance = null; // CLEAR the static reference
        }

        Destroy(gameObject);
    }


}
