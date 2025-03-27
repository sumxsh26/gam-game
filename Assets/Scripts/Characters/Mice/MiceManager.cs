using System.Collections.Generic;
using UnityEngine;

public class MiceManager : MonoBehaviour
{
    public static MiceManager Instance { get; private set; }

    private List<MiceSpawnData> initialMiceData = new List<MiceSpawnData>();

    [System.Serializable]
    public class MiceSpawnData
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool isBlueMouse;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        RecordInitialMouseStates();
    }

    private void RecordInitialMouseStates()
    {
        Mice[] miceInScene = FindObjectsByType<Mice>(FindObjectsSortMode.None);

        foreach (Mice mouse in miceInScene)
        {
            MiceSpawnData data = new MiceSpawnData
            {
                position = mouse.transform.position,
                rotation = mouse.transform.rotation,
                isBlueMouse = mouse.isBlueMouse
            };

            initialMiceData.Add(data);
        }
    }

    public void ResetAllMice(GameObject blueMousePrefab, GameObject redMousePrefab)
    {
        // Destroy existing mice
        Mice[] currentMice = FindObjectsByType<Mice>(FindObjectsSortMode.None);
        foreach (Mice mouse in currentMice)
        {
            Destroy(mouse.gameObject);
        }

        // Respawn from saved data
        foreach (var data in initialMiceData)
        {
            GameObject prefab = data.isBlueMouse ? blueMousePrefab : redMousePrefab;
            Instantiate(prefab, data.position, data.rotation);
        }
    }
}
