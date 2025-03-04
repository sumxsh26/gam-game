using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TypewriterMultiLine : MonoBehaviour
{
    public TextMeshProUGUI line1Text;
    public TextMeshProUGUI line2Text;
    public TextMeshProUGUI line3Text;
    public Image fadeScreen;
    public float typingSpeed = 0.05f;
    public float fadeDuration = 0.5f;
    public float delayBetweenLines = 1.5f;
    public string nextScene = "Level1"; // Change this to your first level scene name

    private void Start()
    {
        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue()
    {
        yield return StartCoroutine(TypeAndFade(line1Text, "They threw you down here like you’re nothing."));
        yield return StartCoroutine(TypeAndFade(line2Text, "But you’re not out of lives yet."));
        yield return StartCoroutine(TypeAndFade(line3Text, "Find your way out. Find your Key to Purrdom."));
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeToBlackAndLoadScene());
    }

    private IEnumerator TypeAndFade(TextMeshProUGUI textElement, string text)
    {
        textElement.text = ""; // Ensure it's empty at start
        textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, 1f); // Fully visible

        // Typewriter Effect
        for (int i = 0; i < text.Length; i++)
        {
            textElement.text = text.Substring(0, i + 1);
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.5f); // Small pause after typing
        yield return StartCoroutine(FadeOutText(textElement));
        yield return new WaitForSeconds(delayBetweenLines);
    }

    private IEnumerator FadeOutText(TextMeshProUGUI textElement)
    {
        float timer = 0f;
        Color textColor = textElement.color;

        while (timer < fadeDuration)
        {
            textColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            textElement.color = textColor;
            timer += Time.deltaTime;
            yield return null;
        }

        textColor.a = 0f; // Ensure it fully disappears
        textElement.color = textColor;
    }

    private IEnumerator FadeToBlackAndLoadScene()
    {
        float timer = 0f;
        Color fadeColor = fadeScreen.color;

        while (timer < fadeDuration)
        {
            fadeColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeScreen.color = fadeColor;
            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(nextScene);
    }
}
