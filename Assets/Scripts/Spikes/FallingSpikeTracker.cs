using UnityEngine;

public class FallingSpikeTracker : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetSpike()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
            transform.position = initialPosition;
            transform.rotation = initialRotation;

            FallingSpike spikeScript = GetComponent<FallingSpike>();
            if (spikeScript != null)
            {
                spikeScript.ResetSpikeState();
            }
        }
    }
}
