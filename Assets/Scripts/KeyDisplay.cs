using UnityEngine;
using UnityEngine.UI;

public class KeyDisplay : MonoBehaviour
{
    public Sprite emptyKey;
    public Sprite fullKey;
    public Image[] keyIcons;

    public KeyManager keyManager;  // Direct reference to KeyManager

    void Update()
    {
        int keyCount = keyManager.keyCount;
        int maxKeys = keyManager.maxKeys;

        for (int i = 0; i < keyIcons.Length; i++)
        {
            if (i < maxKeys)
            {
                keyIcons[i].enabled = true;

                if (i < keyCount)
                {
                    keyIcons[i].sprite = fullKey;  // Show collected key
                }
                else
                {
                    keyIcons[i].sprite = emptyKey; // Show empty key slot
                }
            }
            else
            {
                keyIcons[i].enabled = false; // Hide icons beyond max keys
            }
        }
    }
}





