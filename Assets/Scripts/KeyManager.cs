using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public int keyCount = 0;
    public int maxKeys = 3;
    public GameObject door;
    public GameObject secondDoor;
    private bool doorDestroyed = false;

    public KeyDisplay keyDisplay;  // Direct reference to KeyDisplay

    private void Update()
    {
        if (keyCount >= maxKeys && !doorDestroyed)
        {
            doorDestroyed = true;
            Destroy(door);
            Destroy(secondDoor);
        }
    }

    public void CollectKey()
    {
        if (keyCount < maxKeys)
        {
            keyCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            CollectKey();
            Destroy(other.gameObject);
        }
    }
}




