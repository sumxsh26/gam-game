//using System.Collections;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.Rendering;
//using UnityEngine.UI;

//public class Damageable : MonoBehaviour
//{
//    public UnityEvent<int, Vector2> damageableHit;

//    Animator animator;

//    private float timeSinceHit = 0;
//    public float invincibilityTime = 0.25f;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//        if (isInvincible)
//        {
//            if (timeSinceHit > invincibilityTime)
//            {
//                // remove invincibility
//                isInvincible = false;
//                timeSinceHit = 0;
//            }

//            timeSinceHit += Time.deltaTime;
//        }


//    }

//    [SerializeField] private int _maxHealth = 100;

//    public int MaxHealth
//    {
//        get
//        {
//            return _maxHealth;
//        }
//        set
//        {
//            _maxHealth = value;
//        }
//    }

//    [SerializeField] private int _health = 100;

//    public int Health
//    {
//        get
//        {
//            return _health;
//        }
//        set
//        {
//            _health = value;

//            // if health drops below or equals to 0, character is no longer alive
//            if (Health <= 0)
//            {
//                IsAlive = false;
//            }
//        }
//    }

//    [SerializeField] private bool _isAlive = true;

//    public bool IsAlive
//    {
//        get { return _isAlive; }
//        set
//        {
//            _isAlive = value;
//            animator.SetBool(AnimationStrings.isAlive, value);
//            Debug.Log("IsAlive set " + value);

//            if (!_isAlive)
//            {
//                StartCoroutine(HandleDeathAnimation());
//            }
//        }
//    }

//    private IEnumerator HandleDeathAnimation()
//    {
//        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//        float animationDuration = stateInfo.length;

//        yield return new WaitForSeconds(animationDuration);

//        // Trigger the PlayerDied event after the animation finishes
//        GetComponent<PlayerController>().TriggerPlayerDeath();
//    }

//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//    }

//    public bool LockVelocity
//    {
//        get
//        {
//            return animator.GetBool(AnimationStrings.lockVelocity);
//        }
//        set
//        {
//            animator.SetBool(AnimationStrings.lockVelocity, value);
//        }
//    }

//    [SerializeField] private bool isInvincible = false;


//    public bool Hit(int damage, Vector2 knockback)
//    {
//        if (IsAlive && !isInvincible)
//        {
//            Health -= damage;
//            isInvincible = true;

//            // notify other subscribed components that the damageable was hit to handle the knockback and such
//            animator.SetTrigger(AnimationStrings.hitTrigger);
//            LockVelocity = true;
//            damageableHit?.Invoke(damage, knockback);

//            return true;
//        }

//        // unable to be hit
//        return false;
//    }
//}


// updated

//using System.Collections;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.Rendering;
//using UnityEngine.UI;

//public class Damageable : MonoBehaviour
//{
//    public UnityEvent<int, Vector2> damageableHit;

//    Animator animator;
//    private Rigidbody2D rb;

//    private float timeSinceHit = 0;
//    public float invincibilityTime = 0.25f;

//    [SerializeField] private int _maxHealth = 100;
//    public int MaxHealth
//    {
//        get { return _maxHealth; }
//        set { _maxHealth = value; }
//    }

//    [SerializeField] private int _health = 100;
//    public int Health
//    {
//        get { return _health; }
//        set
//        {
//            _health = value;
//            if (Health <= 0) IsAlive = false; // Handle death
//        }
//    }

//    [SerializeField] private bool _isAlive = true;
//    public bool IsAlive
//    {
//        get { return _isAlive; }
//        set
//        {
//            _isAlive = value;
//            animator.SetBool(AnimationStrings.isAlive, value);
//            Debug.Log("IsAlive set to " + value);
//            if (!_isAlive) StartCoroutine(HandleDeathAnimation());
//        }
//    }

//    [SerializeField] private bool isInvincible = false;

//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//        rb = GetComponent<Rigidbody2D>();
//    }

//    public bool LockVelocity
//    {
//        get { return animator.GetBool(AnimationStrings.lockVelocity); }
//        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
//    }

//    //  Keep Invincibility Timer
//    private void Update()
//    {
//        if (isInvincible)
//        {
//            if (timeSinceHit > invincibilityTime)
//            {
//                isInvincible = false;
//                timeSinceHit = 0;
//            }
//            timeSinceHit += Time.deltaTime;
//        }
//    }

//    //  More Dramatic Knockback, Stronger Fall
//    public bool Hit(int damage, Vector2 knockback)
//    {
//        if (IsAlive && !isInvincible)
//        {
//            Health -= damage;
//            isInvincible = true;

//            if (rb != null)
//            {
//                rb.linearVelocity = Vector2.zero;
//                rb.AddForce(knockback * 2.5f, ForceMode2D.Impulse); //  Stronger horizontal push

//                //  Immediately increase gravity for a more brutal slam down
//                StartCoroutine(ApplyFasterFall());
//            }

//            animator.SetTrigger(AnimationStrings.hitTrigger);
//            LockVelocity = true;
//            damageableHit?.Invoke(damage, knockback);

//            return true;
//        }
//        return false;
//    }

//    //  Faster and Harder Falling After Knockback
//    private IEnumerator ApplyFasterFall()
//    {
//        yield return new WaitForSeconds(0.1f); // Short delay before applying extra gravity
//        if (rb != null)
//        {
//            rb.gravityScale *= 2.2f; // **Super heavy fall effect**
//            yield return new WaitForSeconds(0.3f); // Keep it strong for 0.3s
//            rb.gravityScale = 1f; // Reset gravity
//        }
//    }

//    private IEnumerator HandleDeathAnimation()
//    {
//        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//        float animationDuration = stateInfo.length;
//        yield return new WaitForSeconds(animationDuration);
//        GetComponent<PlayerController>().TriggerPlayerDeath();
//    }
//}


using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;
    private Rigidbody2D rb;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    [SerializeField] private int _maxHealth = 100;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField] private int _health = 100;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (Health <= 0) IsAlive = false;
        }
    }

    [SerializeField] private bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set to " + value);
            if (!_isAlive) StartCoroutine(HandleDeathAnimation());
        }
    }

    [SerializeField] private bool isInvincible = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
    }

    // Keep Invincibility Timer
    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    // More Dramatic Knockback, Stronger Fall
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Reset before applying force
                rb.AddForce(knockback * 2f, ForceMode2D.Impulse); // Apply force
                StartCoroutine(ApplyFasterFall()); // Apply gravity effect
            }

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            return true;
        }
        return false;
    }


    // Faster and Harder Falling After Knockback
    private IEnumerator ApplyFasterFall()
    {
        yield return new WaitForSeconds(0.1f); // Short delay before applying extra gravity
        if (rb != null)
        {
            rb.gravityScale *= 2.2f; // Super heavy fall effect
            yield return new WaitForSeconds(0.3f); // Keep it strong for 0.3s
            rb.gravityScale = 1f; // Reset gravity
        }
    }

    private IEnumerator HandleDeathAnimation()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;
        yield return new WaitForSeconds(animationDuration);
        GetComponent<PlayerController>().TriggerPlayerDeath();
    }
}
