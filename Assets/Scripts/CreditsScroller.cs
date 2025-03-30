using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform topLogo;
    public RectTransform creditsText;
    public RectTransform bottomLogo;

    [Tooltip("Parent that wraps all content (top + text + bottom)")]
    public RectTransform scrollParent;

    [Tooltip("Scene to return to after credits finish")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Scroll Settings")]
    public float scrollSpeed = 50f;

    private float totalHeight;
    private Vector2 initialPosition;
    private bool finished = false;

    private void Start()
    {
        if (!scrollParent || !topLogo || !creditsText || !bottomLogo)
        {
            Debug.LogError("[CreditsScroller] One or more RectTransform references are missing.");
            enabled = false;
            return;
        }

        // Get initial anchored position
        initialPosition = scrollParent.anchoredPosition;

        // Calculate total height of content
        totalHeight =
            topLogo.rect.height +
            creditsText.rect.height +
            bottomLogo.rect.height +
            200f; // buffer padding

        Debug.Log($"[CreditsScroller] Total scroll height: {totalHeight}");
    }

    private void Update()
    {
        if (finished) return;

        float step = scrollSpeed * Time.deltaTime;
        scrollParent.anchoredPosition += new Vector2(0, step);

        if (scrollParent.anchoredPosition.y >= totalHeight)
        {
            finished = true;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
