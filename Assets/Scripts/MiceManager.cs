using UnityEngine;

public class RatManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    public bool isPickedUp;
    private Vector2 vel;
    public float smoothTime;

    // Start
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            transform.position = Vector2.SmoothDamp(transform.position, player.transform.position, ref vel, smoothTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&& !isPickedUp)
        {
            isPickedUp = true;
        }
    }
}
