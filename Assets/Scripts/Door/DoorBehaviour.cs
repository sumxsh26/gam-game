using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool _isDoorOpen = false;

    public float moveSpeed = 3f;
    public float fadeSpeed = 2f;
    public float slideHeight = 6f; // How high the door slides up (adjust as needed)

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private float alpha = 1f;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private AudioManager audioManager;
    private bool hasPlayedOpenSound = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        closedPosition = transform.position;
        openPosition = transform.position + Vector3.up * slideHeight;

        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (_isDoorOpen)
        {
            // Slide up
            transform.position = Vector3.MoveTowards(transform.position, openPosition, moveSpeed * Time.deltaTime);

            // Fade out
            if (alpha > 0f)
            {
                alpha -= fadeSpeed * Time.deltaTime;
                SetAlpha(alpha);
            }

            // Disable collider when fully invisible
            if (alpha <= 0f)
            {
                col.enabled = false;
            }

            // Play audio once
            if (!hasPlayedOpenSound && audioManager?.exitDoor != null)
            {
                audioManager.PlaySFX(audioManager.exitDoor);
                hasPlayedOpenSound = true;
            }
        }
        else
        {
            // Slide down
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, moveSpeed * Time.deltaTime);

            // Fade in
            if (alpha < 1f)
            {
                alpha += fadeSpeed * Time.deltaTime;
                SetAlpha(alpha);
            }

            // Enable collider when fully visible
            if (alpha >= 1f)
            {
                col.enabled = true;
            }

            hasPlayedOpenSound = false;
        }
    }

    private void SetAlpha(float a)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(a));
        }
    }

    public void SetDoorState(bool open)
    {
        _isDoorOpen = open;
    }

    public void ResetDoor()
    {
        _isDoorOpen = false;
        alpha = 1f;
        transform.position = closedPosition;
        col.enabled = true;
        hasPlayedOpenSound = false;
        SetAlpha(alpha);
    }

}



