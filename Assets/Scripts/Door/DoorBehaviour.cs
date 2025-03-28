//using UnityEngine;

//public class DoorBehaviour : MonoBehaviour
//{

//    public bool _isDoorOpen = false;
//    Vector3 _doorClosedPos;
//    Vector3 _doorOpenPos;

//    //control door opening speed
//    float _doorSpeed = 10f;

//    void Awake()
//    {
//        _doorClosedPos = transform.position;
//        _doorOpenPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
//    }

//    // Update
//    void Update()
//    {
//        if (_isDoorOpen)
//        {
//            OpenDoor();
//        }
//        else if (!_isDoorOpen)
//        {
//            CloseDoor();
//        }
//    }

//    void OpenDoor()
//    {
//        if (transform.position != _doorOpenPos)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, _doorOpenPos, _doorSpeed * Time.deltaTime);
//        }
//    }

//    void CloseDoor()
//    {
//        if (transform.position != _doorClosedPos)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, _doorClosedPos, _doorSpeed * Time.deltaTime);
//        }
//    }

//}


//using UnityEngine;

//public class DoorBehaviour : MonoBehaviour
//{
//    public bool isDoorOpen = false;
//    private Vector3 doorClosedPos;
//    private Vector3 doorOpenPos;
//    private float doorSpeed = 10f;

//    void Awake()
//    {
//        doorClosedPos = transform.position;
//        doorOpenPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
//    }

//    void Update()
//    {
//        if (isDoorOpen)
//        {
//            OpenDoor();
//        }
//        else
//        {
//            CloseDoor();
//        }
//    }

//    void OpenDoor()
//    {
//        transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);
//    }

//    void CloseDoor()
//    {
//        transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
//    }
//}


// fade out instead of slide up 
//using UnityEngine;

//public class DoorBehaviour : MonoBehaviour
//{
//    public bool _isDoorOpen = false;
//    private float _fadeSpeed = 2f;
//    private float _alpha = 1f;
//    private Renderer _renderer;
//    private Collider2D _collider;

//    AudioManager audioManager;

//    void Awake()
//    {
//        _renderer = GetComponent<Renderer>();
//        _collider = GetComponent<Collider2D>();
//        //audioSFX
//        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

//    }

//    void Update()
//    {
//        if (_isDoorOpen)
//        {
//            FadeOutDoor();
//            audioManager.PlaySFX(audioManager.exitDoor); //audio sfx

//        }
//        else
//        {
//            FadeInDoor();
//        }
//    }

//    void FadeOutDoor()
//    {
//        if (_alpha > 0f)
//        {
//            _alpha -= _fadeSpeed * Time.deltaTime;
//            SetAlpha(_alpha);
//        }
//        else
//        {
//            _alpha = 0f;
//            _collider.enabled = false; // Disable collision when fully invisible
//        }
//    }

//    void FadeInDoor()
//    {
//        if (_alpha < 1f)
//        {
//            _alpha += _fadeSpeed * Time.deltaTime;
//            SetAlpha(_alpha);
//        }
//        else
//        {
//            _alpha = 1f;
//            _collider.enabled = true; // Enable collision when fully visible
//        }
//    }

//    void SetAlpha(float alpha)
//    {
//        if (_renderer is SpriteRenderer spriteRenderer) // For 2D
//        {
//            Color color = spriteRenderer.color;
//            color.a = alpha;
//            spriteRenderer.color = color;
//        }
//        else if (_renderer is MeshRenderer meshRenderer) // For 3D
//        {
//            foreach (Material mat in meshRenderer.materials)
//            {
//                Color color = mat.color;
//                color.a = alpha;
//                mat.color = color;
//            }
//        }
//    }
//}



// slide up and down - open and close
//using UnityEngine;

//public class DoorBehaviour : MonoBehaviour
//{
//    public bool _isDoorOpen = false;
//    public float slideDistance = 2f; // Distance to slide upward when opening
//    public float slideSpeed = 3f;    // Sliding speed

//    private Vector3 closedPos;
//    private Vector3 openPos;
//    private Collider2D _collider;
//    private AudioManager audioManager;

//    private bool hasPlayedOpenSFX = false;
//    private bool hasPlayedCloseSFX = false;

//    void Awake()
//    {
//        closedPos = transform.position;
//        openPos = new Vector3(closedPos.x, closedPos.y + slideDistance, closedPos.z);

//        _collider = GetComponent<Collider2D>();
//        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
//    }

//    void Update()
//    {
//        if (_isDoorOpen)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, openPos, slideSpeed * Time.deltaTime);
//            _collider.enabled = false;

//            if (!hasPlayedOpenSFX)
//            {
//                audioManager?.PlaySFX(audioManager.exitDoor);
//                hasPlayedOpenSFX = true;
//                hasPlayedCloseSFX = false;
//            }
//        }
//        else
//        {
//            transform.position = Vector3.MoveTowards(transform.position, closedPos, slideSpeed * Time.deltaTime);

//            if (transform.position == closedPos)
//            {
//                _collider.enabled = true;

//                if (!hasPlayedCloseSFX)
//                {
//                    audioManager?.PlaySFX(audioManager.exitDoor);
//                    hasPlayedCloseSFX = true;
//                    hasPlayedOpenSFX = false;
//                }
//            }
//        }
//    }

//    public void SetDoorState(bool isOpen)
//    {
//        _isDoorOpen = isOpen;
//    }
//}


// with visuals
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool _isDoorOpen = false;

    public float moveSpeed = 3f;
    public float fadeSpeed = 2f;
    public float slideHeight = 6f; // How high the door slides up (adjust as needed)

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private float alpha = 1f;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private AudioManager audioManager;
    private bool hasPlayedOpenSound = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        closedPosition = transform.position;
        openPosition = transform.position + Vector3.up * slideHeight;

        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (_isDoorOpen)
        {
            // Slide up
            transform.position = Vector3.MoveTowards(transform.position, openPosition, moveSpeed * Time.deltaTime);

            // Fade out
            if (alpha > 0f)
            {
                alpha -= fadeSpeed * Time.deltaTime;
                SetAlpha(alpha);
            }

            // Disable collider when fully invisible
            if (alpha <= 0f)
            {
                col.enabled = false;
            }

            // Play audio once
            if (!hasPlayedOpenSound && audioManager?.exitDoor != null)
            {
                audioManager.PlaySFX(audioManager.exitDoor);
                hasPlayedOpenSound = true;
            }
        }
        else
        {
            // Slide down
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, moveSpeed * Time.deltaTime);

            // Fade in
            if (alpha < 1f)
            {
                alpha += fadeSpeed * Time.deltaTime;
                SetAlpha(alpha);
            }

            // Enable collider when fully visible
            if (alpha >= 1f)
            {
                col.enabled = true;
            }

            hasPlayedOpenSound = false;
        }
    }

    private void SetAlpha(float a)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(a));
        }
    }

    public void SetDoorState(bool open)
    {
        _isDoorOpen = open;
    }
}
