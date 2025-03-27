//using UnityEngine;

//public class CPTorch : MonoBehaviour
//{
//    private Animator animator;
//    private bool hasLit = false;

//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//    }

//    private void Start()
//    {
//        // Ensure it always starts unlit
//        ResetTorch();
//    }

//    public void TriggerLightup()
//    {
//        if (!hasLit)
//        {
//            hasLit = true;
//            animator.Play("lightup_Front", 0, 0f);
//        }
//    }

//    public void ResetTorch()
//    {
//        hasLit = false;
//        if (animator != null)
//        {
//            animator.Play("unlit_Front", 0, 0f); // Reset to unlit idle
//        }
//    }
//}


// with lighting
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
            animator.Play("lightup_Front", 0, 0f);

            if (torchLight != null)
            {
                torchLight.enabled = true;
            }
            else
            {
                Debug.LogWarning("[CPTorch] No Light2D found under 'Light' child!");
            }
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
