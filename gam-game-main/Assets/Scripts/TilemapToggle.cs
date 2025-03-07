using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;  // Add this to work with UI

public class TilemapToggle : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;

    public float displayTime = 3f;
    public float cooldownTime = 30f;
    private float lastToggleTime = -30f;

    public Slider cooldownBar;  // Reference to the UI Slider

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>();

        tilemapRenderer.enabled = false;
        tilemapCollider.enabled = true;

        if (cooldownBar != null)
        {
            cooldownBar.maxValue = cooldownTime;  // Set max value to cooldown time
            cooldownBar.value = cooldownTime;     // Start as fully charged
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time - lastToggleTime >= cooldownTime)
            {
                TogglePlatform();
                lastToggleTime = Time.time;

                if (cooldownBar != null)
                {
                    cooldownBar.value = 0;  // Reset the cooldown bar when ability is used
                }
            }
            else
            {
                Debug.Log("Cooldown in progress! Please wait.");
            }
        }

        // Update the cooldown bar over time
        if (cooldownBar != null && cooldownBar.value < cooldownTime)
        {
            cooldownBar.value += Time.deltaTime;  // Refill bar gradually
        }
    }

    void TogglePlatform()
    {
        tilemapRenderer.enabled = !tilemapRenderer.enabled;
        Invoke("HideVisuals", displayTime);
    }

    void HideVisuals()
    {
        tilemapRenderer.enabled = false;
    }
}
