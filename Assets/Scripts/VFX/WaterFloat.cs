using UnityEngine;

public class WaterFloat : MonoBehaviour
{
    [Header("Bobbing Motion")]
    public float bobSpeed = 1f;
    public float bobHeight = 0.1f;

    [Header("Rocking Motion")]
    public float rockSpeed = 1f;
    public float rockAngle = 5f;

    [Header("Base Tilt")]
    public float baseTilt = 0f; // Set this in Inspector to lean the barrel like in your second pic

    private Vector3 startPos;
    private float randomOffset;

    void Start()
    {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Bobbing up and down
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed + randomOffset) * bobHeight;

        // Rocking rotation + base tilt
        float rockingRotation = Mathf.Sin(Time.time * rockSpeed + randomOffset) * rockAngle;

        transform.position = new Vector3(startPos.x, newY, startPos.z);
        transform.rotation = Quaternion.Euler(0f, 0f, baseTilt + rockingRotation);
    }
}
