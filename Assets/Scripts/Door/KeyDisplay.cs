using UnityEngine;
using UnityEngine.UI;

public class KeyDisplay : MonoBehaviour
{
    public Sprite emptyKey;
    public Sprite fullKey;
    public Image keyIcon; // Single key icon

    private Key key; // Reference to Key object

    void Start()
    {
        key = Object.FindFirstObjectByType<Key>(); // Find the Key object in the scene
    }

    void Update()
    {
        // Try to find the key in the scene
        Key key = Object.FindFirstObjectByType<Key>();

        // If the key exists, show an empty key
        if (key != null)
        {
            keyIcon.sprite = emptyKey;
            keyIcon.enabled = true;
        }
        else
        {
            keyIcon.sprite = fullKey; // Key has been collected
            keyIcon.enabled = true;
        }
    }
}
