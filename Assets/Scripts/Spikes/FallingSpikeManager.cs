using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeManager : MonoBehaviour
{
    public static FallingSpikeManager Instance { get; private set; }

    [Header("All Falling Spikes in Scene")]
    public List<FallingSpikeTracker> allSpikes = new List<FallingSpikeTracker>();

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

    public void ResetAllSpikes()
    {
        Debug.Log("[FallingSpikeManager] Resetting " + allSpikes.Count + " spikes");

        foreach (FallingSpikeTracker spike in allSpikes)
        {
            if (spike != null)
            {
                spike.ResetSpike();
            }
        }
    }
}
