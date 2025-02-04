using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    [SerializeField] private int _maxHealth = 100;

    public int MaxHealth 
    { 
        get 
        { 
            return _maxHealth; 
        } 
        set 
        { 
            _maxHealth = value; 
        } 
    }

    [SerializeField] private int _health = 100;
    
    public int Health
    { 
        get 
        { 
            return _health;
        } 
        set
        { 
            _health = value;

            // if health drops below or equals to 0, character is no longer alive
            if (Health <= 0)
            {
                IsAlive = false;
            }
        } 
    }

    [SerializeField] private bool _isAlive = true;

    public bool IsAlive 
    { 
        get 
        { 
            return _isAlive; 
        } 
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);

            Debug.Log("IsAlive set " + value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [SerializeField] private bool isInvincible = false;


    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            // notify other subscribed components that the damageable was hit to handle the knockback and such
            animator.SetTrigger(AnimationStrings.hitTrigger);
            damageableHit?.Invoke(damage, knockback);

            return true;
        }

        // unable to be hit
        return false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                // remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }

        
    }
}
