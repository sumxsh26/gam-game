using UnityEngine;

[System.Serializable]
public class ZoneTextPair
{
    public Transform zone;       // Assign your Zone GameObject here
    public Transform textBox;    // Assign your instruction text GameObject here
}

public class TextZoneAligner : MonoBehaviour
{
    [SerializeField] private ZoneTextPair[] pairs;

    private void Start()
    {
        foreach (var pair in pairs)
        {
            if (pair.zone == null || pair.textBox == null) continue;

            Vector3 currentPos = pair.textBox.position;
            Vector3 newPos = new Vector3(pair.zone.position.x, currentPos.y, currentPos.z);
            pair.textBox.position = newPos;
        }
    }
}
