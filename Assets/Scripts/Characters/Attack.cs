using UnityEngine;

public class Attack : MonoBehaviour
{

    Collider2D attackCollider;

    public int attackDamage = 10;

    public Vector2 knockBack = Vector2.zero;

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // see if it can be hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            // hit the target
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);


            if (gotHit)
                Debug.Log(collision.name + " hit for " + attackDamage);
        }
    }
}



//using UnityEngine;

//public class Attack : MonoBehaviour
//{
//    Collider2D attackCollider;

//    public int attackDamage = 1; // Spikes deal 1 heart damage
//    public Vector2 knockBack = new Vector2(3f, 0.5f); //  Stronger push sideways, lower height

//    private void Awake()
//    {
//        attackCollider = GetComponent<Collider2D>();
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        Damageable damageable = collision.GetComponent<Damageable>();

//        if (damageable != null)
//        {
//            Vector2 deliveredKnockback = transform.parent != null
//                ? (transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y))
//                : knockBack; // Default knockback if no parent

//            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback * 2f); // Stronger impact

//            if (gotHit)
//                Debug.Log(collision.name + " hit for " + attackDamage);
//        }
//    }

//}


