using UnityEngine;

public class AstronautController : MonoBehaviour
{

    public float speed = 5f;
    public float jumpSpeed = 5f;
    private float direction = 0f;
    private Rigidbody2D player;
    public CoinManager cm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxis("Horizontal");
        Debug.Log(direction);

        //move right
        if (direction > 0f)
        {
            player.linearVelocity = new Vector2(direction * speed, player.linearVelocity.y);
        }
        //move left
        else if (direction < 0f)
        {
            player.linearVelocity = new Vector2(direction * speed, player.linearVelocity.y);
        }
        //keep still
        else
        {
            player.linearVelocity = new Vector2(0, player.linearVelocity.y);
        }

        //jump
        if (Input.GetButtonDown("Jump"))
        {
            player.linearVelocity = new Vector2((player.linearVelocity).x, jumpSpeed);
        } 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
    }
}
