using UnityEngine;

public class Spike : MonoBehaviour
{
    public int spikeDamage = 1; // Damage per hit
    public Vector2 knockBack = new Vector2(2f, 8f); // Stronger upwards knockback

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            Vector2 deliveredKnockback = knockBack;

            if (player != null)
            {
                // Ensure knockback direction is opposite of where player is facing
                deliveredKnockback.x *= player.IsFacingRight ? -1 : 1;
            }

            bool gotHit = damageable.Hit(spikeDamage, deliveredKnockback);

            if (gotHit)
                Debug.Log(collision.name + " hit by spikes for " + spikeDamage);
        }
    }
}
