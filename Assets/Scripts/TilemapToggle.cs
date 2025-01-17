using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToggle : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;  // For platform visuals
    private TilemapCollider2D tilemapCollider;  // For platform collision

    public float displayTime = 3f;  // Time the platform stays visible
    public float cooldownTime = 30f;  // 30-second cooldown
    private float lastToggleTime = -30f;  // Keeps track of the last toggle time

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>();

        // Hide visuals but keep collision active at the start
        tilemapRenderer.enabled = false;
        tilemapCollider.enabled = true;  // Keep platform colliders active
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time - lastToggleTime >= cooldownTime)
            {
                TogglePlatform();  // Toggle if cooldown has passed
                lastToggleTime = Time.time;  // Record current time as last toggle time
            }
            else
            {
                Debug.Log("Cooldown in progress! Please wait.");
            }
        }
    }

    void TogglePlatform()
    {
        // Toggle platform visuals
        tilemapRenderer.enabled = !tilemapRenderer.enabled;  // Show/hide visuals
        Invoke("HideVisuals", displayTime);  // Hide visuals after display time
    }

    void HideVisuals()
    {
        tilemapRenderer.enabled = false;  // Hide platform visuals
    }
}
