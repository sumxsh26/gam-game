using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem fallParticle;
    [SerializeField] ParticleSystem waterSplashParticle;

    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod;

    [SerializeField] Rigidbody2D _rb;

    float counter;
    bool isOnGround;

    private void Update()
    {
        counter += Time.deltaTime;

        if (isOnGround && Mathf.Abs(_rb.linearVelocity.x) > occurAfterVelocity)
        {
            if (counter > dustFormationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            fallParticle.Play();
            isOnGround = true;
        }
        else if (collision.CompareTag("Water"))
        {
            waterSplashParticle.Play();
        }
    }

    // Optional: stop movement trail when off ground
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Ground"))
    //     {
    //         isOnGround = false;
    //     }
    // }
}

