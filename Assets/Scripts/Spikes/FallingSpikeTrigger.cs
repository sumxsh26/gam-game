using UnityEngine;

public class FallingSpikeTrigger : MonoBehaviour
{
    private FallingSpike parentSpike;

    void Start()
    {
        parentSpike = GetComponentInParent<FallingSpike>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentSpike.TriggerFall();
        }
    }
}
