using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformToggle : MonoBehaviour
{
    public bool isBluePlatform;

    private Tilemap tilemap;
    private bool isActive = false; // Platforms start INACTIVE
    private float inactiveAlpha = 50f / 255f; // 150 Alpha when inactive
    private float activeAlpha = 1f; // 255 Alpha when active

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        // Set the initial alpha to 150 (semi-transparent)
        SetPlatformAlpha(inactiveAlpha);
        SetPlatformState(isActive); // Ensure initial state is INACTIVE
    }

    public void SetPlatformState(bool active)
    {
        isActive = active;

        // Change alpha value instead of color
        float targetAlpha = active ? activeAlpha : inactiveAlpha;
        SetPlatformAlpha(targetAlpha);

        // Enable or disable the collider
        tilemap.gameObject.GetComponent<TilemapCollider2D>().enabled = active;
    }

    private void SetPlatformAlpha(float alpha)
    {
        Color currentColor = tilemap.color;
        tilemap.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
}
