//using UnityEngine;
//using UnityEngine.UI;

//public class KeyDisplay : MonoBehaviour
//{
//    public Sprite emptyKey;
//    public Sprite fullKey;
//    public Image keyIcon; // Single key icon

//    private Key key; // Reference to Key object

//    void Start()
//    {
//        key = Object.FindFirstObjectByType<Key>(); // Find the Key object in the scene
//    }

//    void Update()
//    {
//        // Try to find the key in the scene
//        Key key = Object.FindFirstObjectByType<Key>();

//        // If the key exists, show an empty key
//        if (key != null)
//        {
//            keyIcon.sprite = emptyKey;
//            keyIcon.enabled = true;
//        }
//        else
//        {
//            keyIcon.sprite = fullKey; // Key has been collected
//            keyIcon.enabled = true;
//        }
//    }
//}


using UnityEngine;
using UnityEngine.UI;

public class KeyDisplay : MonoBehaviour
{
    public Sprite emptyKey;
    public Sprite fullKey;
    public Image[] keyIcons; // Array for three key icons

    private int keysCollected = 0;
    private int totalKeys = 3; // Number of keys required

    void Update()
    {
        // Count collected keys in the scene
        keysCollected = totalKeys - FindObjectsByType<Key>(FindObjectsSortMode.None).Length;

        for (int i = 0; i < keyIcons.Length; i++)
        {
            if (i < totalKeys)
            {
                keyIcons[i].enabled = true;

                if (i < keysCollected)
                {
                    keyIcons[i].sprite = fullKey;  // Show full key if collected
                }
                else
                {
                    keyIcons[i].sprite = emptyKey; // Show empty key if not yet collected
                }
            }
            else
            {
                keyIcons[i].enabled = false; // Hide extra key slots
            }
        }
    }
}
