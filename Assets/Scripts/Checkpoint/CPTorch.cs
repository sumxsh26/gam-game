using UnityEngine;

public class CPTorch : MonoBehaviour
{
    private Animator animator;
    private bool hasLit = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Ensure it always starts unlit
        ResetTorch();
    }

    public void TriggerLightup()
    {
        if (!hasLit)
        {
            hasLit = true;
            animator.Play("lightup_Front", 0, 0f);
        }
    }

    public void ResetTorch()
    {
        hasLit = false;
        if (animator != null)
        {
            animator.Play("unlit_Front", 0, 0f); // Reset to unlit idle
        }
    }
}
