using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilemapToggle : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;

    public float displayTime = 3f;
    public float cooldownTime = 30f;
    private float lastToggleTime = -30f;

    public Slider cooldownBar;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>();

        tilemapRenderer.enabled = false; // Start invisible
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
                    cooldownBar.value = 0;
                }
            }
            else
            {
                Debug.Log("Cooldown in progress! Please wait.");
            }
        }

        if (cooldownBar != null && cooldownBar.value < cooldownTime)
        {
            cooldownBar.value += Time.deltaTime;
        }
    }

    void TogglePlatform()
    {
        bool isCurrentlyVisible = tilemapRenderer.enabled;

        if (!isCurrentlyVisible)
        {
            EnablePlatform();
        }
        else
        {
            DisablePlatform();
        }
    }

    void EnablePlatform()
    {
        tilemapRenderer.enabled = true;
        SwitchPlayerCollisionMode(true); // Player can walk through when platform is invisible

        Invoke("DisablePlatform", displayTime);
    }

    void DisablePlatform()
    {
        tilemapRenderer.enabled = false;
        SwitchPlayerCollisionMode(false); // Player collides with platform when visible
    }


    void SwitchPlayerCollisionMode(bool passThrough)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (passThrough)
            {
                player.layer = LayerMask.NameToLayer("GhostPlayer");
                Debug.Log("[LAYER SWITCH] Player is now in GhostPlayer mode (can walk through platforms).");
            }
            else
            {
                player.layer = LayerMask.NameToLayer("Player"); // Player should collide with platforms when visible
                Debug.Log("[LAYER SWITCH] Player is now in Player mode (collides with platforms).");
            }
        }
    }


}
