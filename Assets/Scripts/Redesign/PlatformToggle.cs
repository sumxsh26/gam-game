using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformToggle : MonoBehaviour
{
    public bool isBluePlatform;

    private Tilemap tilemap;
    private bool isActive = false; // Platforms start INACTIVE
    private Color untoggledColor;
    private Color toggledColor;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        if (isBluePlatform)
        {
            untoggledColor = new Color32(2, 0, 255, 255); // #0200FF
            toggledColor = new Color32(0, 255, 232, 255); // #00FFE8
        }
        else
        {
            untoggledColor = new Color32(255, 0, 0, 255);  // #FF0000
            toggledColor = new Color32(255, 0, 145, 255); // #FF0091
        }

        SetPlatformState(isActive); // Ensure initial state is INACTIVE
    }

    public void SetPlatformState(bool active)
    {
        isActive = active;
        tilemap.color = active ? toggledColor : untoggledColor;
        tilemap.gameObject.GetComponent<TilemapCollider2D>().enabled = active;
    }
}
