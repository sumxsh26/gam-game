using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    public Damageable damageable;

    void Update()
    {
        int health = damageable.Health;
        int maxHealth = damageable.MaxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].enabled = true;

                if (i < health)
                {
                    hearts[i].sprite = fullHeart;  // Show full heart if within current health
                }
                else
                {
                    hearts[i].sprite = emptyHeart; // Show empty heart if health is lost
                }
            }
            else
            {
                hearts[i].enabled = false;  // Hide hearts beyond max health
            }
        }
    }
}
