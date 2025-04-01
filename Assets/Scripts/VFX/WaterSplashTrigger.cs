using UnityEngine;

public class WaterSplashTrigger : MonoBehaviour
{
    public ParticleSystem splashFX;
    public float splashThreshold = 15f; // degrees
    private float lastAngle;

    void Update()
    {
        float currentAngle = transform.eulerAngles.z;
        float angleDelta = Mathf.Abs(Mathf.DeltaAngle(currentAngle, lastAngle));

        if (angleDelta > splashThreshold)
        {
            splashFX.Play();
        }

        lastAngle = currentAngle;
    }
}
