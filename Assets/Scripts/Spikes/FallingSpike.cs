//using System.Collections;
//using UnityEngine;

//public class FallingSpikes : MonoBehaviour
//{
//    public GameObject spikePrefab;
//    public Transform[] spawnPoints;
//    public float minSpawnInterval = 0.5f;
//    public float maxSpawnInterval = 1.5f;
//    private bool isPlayerInZone = false;

//    void Start()
//    {
//        StartCoroutine(SpawnSpikes());
//    }

//    IEnumerator SpawnSpikes()
//    {
//        while (true)
//        {
//            if (isPlayerInZone)
//            {
//                int spikeCount = Random.Range(1, 2); // 2-3 spikes at a time

//                for (int i = 0; i < spikeCount; i++)
//                {
//                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
//                    GameObject spike = Instantiate(spikePrefab, spawnPoint.position, Quaternion.identity);

//                    Rigidbody2D rb = spike.GetComponent<Rigidbody2D>();
//                    if (rb == null)
//                    {
//                        rb = spike.AddComponent<Rigidbody2D>();
//                    }

//                    rb.gravityScale = 2; // Ensure spikes fall
//                    rb.linearVelocity = Vector2.down * 0.1f; // Small push to ensure falling
//                }
//            }
//            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isPlayerInZone = true;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isPlayerInZone = false;
//        }
//    }
//}


// one spike falling with visuals 
//using UnityEngine;

//public class FallingSpike : MonoBehaviour
//{
//    [Header("Fall Settings")]
//    public float fallDelay = 0.1f;
//    public float gravityScale = 3f;

//    [Header("Damage Settings")]
//    public int spikeDamage = 1;
//    public Vector2 knockBack = new Vector2(2f, 8f); // Upward knockback

//    private bool hasFallen = false;
//    private Rigidbody2D rb;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.bodyType = RigidbodyType2D.Kinematic;
//    }

//    public void TriggerFall()
//    {
//        if (hasFallen) return;

//        hasFallen = true;
//        Invoke(nameof(ActivateFall), fallDelay);
//    }

//    private void ActivateFall()
//    {
//        rb.bodyType = RigidbodyType2D.Dynamic;
//        rb.gravityScale = gravityScale;
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        Collider2D hitCollider = collision.collider;

//        Debug.Log($"[FallingSpike] Collided with: {hitCollider.name} on layer {LayerMask.LayerToName(hitCollider.gameObject.layer)}");

//        // Traverse up to find Damageable on root object
//        Transform root = hitCollider.transform.root;
//        Damageable damageable = root.GetComponent<Damageable>();

//        if (damageable != null)
//        {
//            PlayerMovement player = root.GetComponent<PlayerMovement>();
//            Vector2 deliveredKnockback = knockBack;

//            if (player != null)
//            {
//                bool isLeftOfSpike = (player.transform.position.x < transform.position.x);
//                deliveredKnockback.x = isLeftOfSpike ? -Mathf.Abs(knockBack.x) : Mathf.Abs(knockBack.x);
//            }

//            Debug.Log($"[FallingSpike] Attempting to damage {root.name} via {hitCollider.name} | Knockback: {deliveredKnockback}");

//            bool gotHit = damageable.Hit(spikeDamage, deliveredKnockback);
//            if (gotHit)
//            {
//                Debug.Log($"[FallingSpike] {root.name} hit by falling spike for {spikeDamage}");

//                // Play spike hit sound
//                AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
//                if (audioManager != null && audioManager.spikeHit != null)
//                {
//                    audioManager.PlaySFX(audioManager.spikeHit);
//                }
//            }
//            else
//            {
//                Debug.Log($"[FallingSpike] {root.name} was not damaged (maybe invincible or already dead)");
//            }
//        }
//        else
//        {
//            Debug.Log($"[FallingSpike] No Damageable found on {root.name}");
//        }
//    }
//}

using System.Collections;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallDelay = 0.1f;
    public float gravityScale = 3f;
    public float breakableDelay = 0.15f; // Grace time before collisions count

    [Header("Damage Settings")]
    public int spikeDamage = 1;
    public Vector2 knockBack = new Vector2(2f, 8f);

    [Header("Visuals")]
    public Sprite debrisSprite;


    private bool hasFallen = false;
    private bool hasDealtDamage = false;
    private bool isBroken = false;
    private bool canBreak = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;
    private Camera mainCamera;

    private Color particleColor = new Color32(0x42, 0x43, 0x43, 255); // hex #424343

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
        if (boxCol != null) boxCol.enabled = false;

        mainCamera = Camera.main;
    }

    public void TriggerFall()
    {
        if (hasFallen) return;
        hasFallen = true;

        StartCoroutine(ShakeBeforeFall(0.3f, 0.05f)); // duration, magnitude
    }


    private void ActivateFall()
    {
        if (rb == null || boxCol == null) return;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;
        rb.linearVelocity = new Vector2(0f, -5f); // instant downward speed


        StartCoroutine(EnableColliderAfterDelay());
    }

    private System.Collections.IEnumerator EnableColliderAfterDelay()
    {
        yield return new WaitForSeconds(0.05f); // Wait 1-2 frames
        boxCol.enabled = true;

        yield return new WaitForSeconds(breakableDelay); // Grace period before breaking
        canBreak = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken || !canBreak) return;

        GameObject other = collision.collider.gameObject;
        Transform root = other.transform.root;
        Damageable damageable = root.GetComponent<Damageable>();

        bool hitPlayer = false;

        if (!hasDealtDamage && damageable != null)
        {
            PlayerMovement player = root.GetComponent<PlayerMovement>();
            Vector2 deliveredKnockback = knockBack;

            if (player != null)
            {
                bool isLeft = player.transform.position.x < transform.position.x;
                deliveredKnockback.x = isLeft ? -Mathf.Abs(knockBack.x) : Mathf.Abs(knockBack.x);
            }

            bool gotHit = damageable.Hit(spikeDamage, deliveredKnockback);
            hasDealtDamage = true;
            hitPlayer = gotHit;

            if (gotHit)
            {
                AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
                if (audioManager != null && audioManager.spikeHit != null)
                    audioManager.PlaySFX(audioManager.spikeHit);
            }
        }

        // If it hit something that's NOT the player, play the impact sound
        if (!hitPlayer)
        {
            AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
            if (audioManager != null && audioManager.fallingSpikeHit != null)
                audioManager.PlaySFX(audioManager.fallingSpikeHit);
        }

        BreakSpike();
    }


    private void BreakSpike()
    {
        if (isBroken) return;
        isBroken = true;

        // Break visuals
        CreateBreakParticles();

        if (sr != null) sr.enabled = false;
        if (boxCol != null) boxCol.enabled = false;

        // Don't destroy — just deactivate
        gameObject.SetActive(false);
    }


    private void CreateBreakParticles()
    {
        GameObject particles = new GameObject("SpikeBreakParticles");

        // Spawn from bottom tip of the spike
        Vector3 spikeBottom = transform.position + Vector3.down * (GetComponent<SpriteRenderer>().bounds.size.y / 2f);
        particles.transform.position = spikeBottom;

        var ps = particles.AddComponent<ParticleSystem>();

        // Main particle settings
        var main = ps.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 0.8f); // varied life
        main.startSpeed = new ParticleSystem.MinMaxCurve(4f, 7f);        // more outward force
        main.startSize = new ParticleSystem.MinMaxCurve(0.4f, 0.6f);     // varied size
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, 360f);   // random rotation
        main.loop = false;
        main.maxParticles = 50;
        main.playOnAwake = true;

        // Fade out over time
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
            new GradientColorKey(Color.white, 0f),
            new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[] {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = grad;

        // Wider spread angle for chaos
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 80f;
        shape.radius = 0.2f;
        shape.randomDirectionAmount = 1f;

        // Emission burst
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 25) });

        // Renderer and material using assigned sprite
        var renderer = ps.GetComponent<ParticleSystemRenderer>();

        if (debrisSprite != null)
        {
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.mainTexture = debrisSprite.texture;
            renderer.material = mat;
        }
        else
        {
            Debug.LogWarning("[FallingSpike] No debris sprite assigned — particle may not appear properly.");
        }

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.Play();
        Destroy(particles, 1f);
    }


    private void Update()
    {
        // Only check for off-screen destruction *after* the spike has started falling
        if (!isBroken && hasFallen && mainCamera != null)
        {
            Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);

            if (screenPos.y < -0.1f || screenPos.y > 1.1f || screenPos.x < -0.1f || screenPos.x > 1.1f)
            {
                Debug.Log("[FallingSpike] Out of bounds – breaking");
                BreakSpike();
            }
        }
    }


    private IEnumerator ShakeBeforeFall(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        ActivateFall(); // Start falling after shake ends
    }

    public void ResetSpikeState()
    {
        hasFallen = false;
        hasDealtDamage = false;
        isBroken = false;
        canBreak = false;

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (boxCol == null) boxCol = GetComponent<BoxCollider2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        if (boxCol != null) boxCol.enabled = false;
        if (sr != null) sr.enabled = true;

        gameObject.SetActive(true);
    }


}
