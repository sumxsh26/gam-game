using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformManager : MonoBehaviour
{
    public static FallingPlatformManager Instance { get; private set; }

    private List<FallingPlatformTracker> allPlatforms = new List<FallingPlatformTracker>();

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

    public void RegisterPlatform(FallingPlatformTracker platform)
    {
        if (!allPlatforms.Contains(platform))
        {
            allPlatforms.Add(platform);
        }
    }

    public void ResetAllPlatforms()
    {
        foreach (FallingPlatformTracker platform in allPlatforms)
        {
            if (platform != null)
            {
                platform.ResetPlatform();
            }
        }
    }
}
