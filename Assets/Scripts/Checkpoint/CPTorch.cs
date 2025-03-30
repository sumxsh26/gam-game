using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CPTorch : MonoBehaviour
{
    private Animator animator;
    private bool hasLit = false;
    private Light2D torchLight;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Look for the child GameObject named "Light"
        Transform lightTransform = transform.Find("Light");
        if (lightTransform != null)
        {
            torchLight = lightTransform.GetComponent<Light2D>();
        }
    }

    private void Start()
    {
        ResetTorch();
    }
    public void TriggerLightup()
    {
        if (!hasLit)
        {
            hasLit = true;
            Debug.Log("[CP Torch] Triggering light-up animation");

            animator.Play("lightup_Front", 0, 0f);

            // Start coroutine to enable light after animation finishes
            StartCoroutine(EnableLightAfterDelay(animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    private IEnumerator EnableLightAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (torchLight != null)
        {
            torchLight.enabled = true;
            Debug.Log("[CP Torch] Light enabled after animation");
        }
    }



    public void ResetTorch()
    {
        hasLit = false;

        if (animator != null)
        {
            animator.Play("unlit_Front", 0, 0f);
        }

        if (torchLight != null)
        {
            torchLight.enabled = false;
        }
    }
}
