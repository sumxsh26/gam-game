using UnityEngine;

public class Spike : MonoBehaviour
{
    public int spikeDamage = 1;
    public Vector2 knockBack = new Vector2(2f, 8f); // Upward knockback

    private void ApplyDamage(Collider2D collider)
    {
        Damageable damageable = collider.GetComponent<Damageable>();

        if (damageable != null)
        {
            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            Vector2 deliveredKnockback = knockBack;

            if (player != null)
            {
                bool isLeftOfSpike = (player.transform.position.x < transform.position.x);
                deliveredKnockback.x = isLeftOfSpike ? -Mathf.Abs(knockBack.x) : Mathf.Abs(knockBack.x);
            }

            Debug.Log($"Spike hit {collider.name}, Knockback: {deliveredKnockback}");

            bool gotHit = damageable.Hit(spikeDamage, deliveredKnockback);
            if (gotHit) Debug.Log(collider.name + " hit by spikes for " + spikeDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { ApplyDamage(other); }
    private void OnCollisionEnter2D(Collision2D collision) { ApplyDamage(collision.collider); }
}