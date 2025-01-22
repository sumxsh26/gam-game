using UnityEngine;
using UnityEngine.UI;


public class KeyManager : MonoBehaviour
{
    public int keyCount;
    public Text keyText;
    public GameObject door;
    private bool doorDestroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        keyText.text = " : " + keyCount.ToString();

        if (keyCount == 1 && !doorDestroyed)
        {
            doorDestroyed = true;
            Destroy(door);
        }
    }
}
