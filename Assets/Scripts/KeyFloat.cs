using UnityEngine;

public class KeyFloat : MonoBehaviour
{
    public float floatSpeed = 2f;       // Speed of bobbing
    public float floatAmount = 0.25f;   // Distance to move up and down

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}
