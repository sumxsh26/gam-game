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
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockBack : new Vector2 (-knockBack.x, knockBack.y);
            // hit the target
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);


            if (gotHit)
                Debug.Log(collision.name + " hit for " +  attackDamage);
        }
    }
}
