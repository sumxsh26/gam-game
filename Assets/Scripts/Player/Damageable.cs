using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;
    private Coroutine deathRoutine = null;


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

    [SerializeField] private int _maxHealth = 3;

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

    [SerializeField] private int _health = 3;

    //public int Health
    //{
    //    get
    //    {
    //        return _health;
    //    }
    //    set
    //    {
    //        _health = value;

    //        // if health drops below or equals to 0, character is no longer alive
    //        if (Health <= 0)
    //        {
    //            IsAlive = false;
    //        }
    //    }
    //}

    public int Health
    {
        get => _health;
        set
        {
            _health = value;

            // Only trigger death if alive and just dropped to 0
            if (_health <= 0 && IsAlive)
            {
                IsAlive = false; // this will trigger the death coroutine ONCE
            }
        }
    }


    [SerializeField] private bool _isAlive = true;
    //public bool IsAlive
    //{
    //    get => _isAlive;
    //    set
    //    {
    //        if (_isAlive == value) return; // prevent redundant changes

    //        _isAlive = value;
    //        animator.SetBool(AnimationStrings.isAlive, value);
    //        Debug.Log("[Damageable] IsAlive set to " + value);

    //        if (deathRoutine == null)
    //        {
    //            deathRoutine = StartCoroutine(HandleDeathAnimation());
    //        }

    //    }
    //}

    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            if (_isAlive == value) return;

            _isAlive = value;
            Debug.Log("[Damageable] IsAlive set to " + value);

            if (!_isAlive)
            {
                StartCoroutine(HandleDeathAnimation());
            }
        }
    }



    //private IEnumerator HandleDeathAnimation()
    //{
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    float animationDuration = stateInfo.length;

    //    yield return new WaitForSeconds(animationDuration);

    //    // Trigger the PlayerDied event after the animation finishes
    //    GetComponent<PlayerMovement>().TriggerPlayerDeath();
    //}

    private bool deathHandled = false;
    private IEnumerator HandleDeathAnimation()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration);

        // Ensure we're still dead before triggering death
        if (!_isAlive)
        {
            GetComponent<PlayerMovement>().TriggerPlayerDeath();
        }

        deathRoutine = null; // Clear reference
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
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
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            return true;
        }

        // unable to be hit
        return false;
    }

    public void ResetHealth()
    {
        _health = MaxHealth;
        IsAlive = true;
        isInvincible = false;
        timeSinceHit = 0f;

        // Cancel death coroutine if still running
        if (deathRoutine != null)
        {
            StopCoroutine(deathRoutine);
            deathRoutine = null;
            Debug.Log("[Damageable] Cancelled lingering death coroutine on respawn");
        }
    }



}






