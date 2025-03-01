//using System.Collections;
//using UnityEngine;

//public class SlidingDoor : MonoBehaviour
//{
//    public Transform leftDoor;  // Assign DoorLeft in Inspector
//    public Transform rightDoor; // Assign DoorRight in Inspector
//    public GameObject completeZone; // Reference to the Complete Zone

//    public float slideDistance = 2f;  // Distance each half moves
//    public float slideSpeed = 3f;    // Speed of sliding
//    public float fadeSpeed = 2f;     // Speed of fading effect

//    private Vector3 leftDoorClosedPos, rightDoorClosedPos;
//    private Vector3 leftDoorOpenPos, rightDoorOpenPos;
//    private SpriteRenderer leftRenderer, rightRenderer;

//    public bool IsDoorFullyOpen { get; private set; } = false; // Track when doors fully open

//    private void Start()
//    {
//        // Find the child objects dynamically
//        leftDoor = transform.Find("DoorLeft");
//        rightDoor = transform.Find("DoorRight");
//        completeZone = transform.Find("CompleteZone")?.gameObject; // Find the CompleteZone inside Exit

//        // Ensure they are found
//        if (leftDoor == null) Debug.LogError("DoorLeft not found in children!");
//        if (rightDoor == null) Debug.LogError("DoorRight not found in children!");
//        if (completeZone == null) Debug.LogError("CompleteZone not found in children!");

//        // Get the SpriteRenderer components
//        leftRenderer = leftDoor?.GetComponent<SpriteRenderer>();
//        rightRenderer = rightDoor?.GetComponent<SpriteRenderer>();

//        // Ensure SpriteRenderer is assigned
//        if (leftRenderer == null) Debug.LogError("SpriteRenderer is NULL for DoorLeft!");
//        if (rightRenderer == null) Debug.LogError("SpriteRenderer is NULL for DoorRight!");

//        // Store the original positions
//        leftDoorClosedPos = leftDoor.position;
//        rightDoorClosedPos = rightDoor.position;

//        // Move the left door left, right door right
//        leftDoorOpenPos = leftDoorClosedPos + Vector3.left * slideDistance;
//        rightDoorOpenPos = rightDoorClosedPos + Vector3.right * slideDistance;

//        // Ensure CompleteZone is disabled at the start
//        if (completeZone != null)
//        {
//            completeZone.SetActive(false);
//        }
//    }

//    public void OpenDoor()
//    {
//        StartCoroutine(SlideAndFadeOut(leftDoor, leftDoorOpenPos, leftRenderer));
//        StartCoroutine(SlideAndFadeOut(rightDoor, rightDoorOpenPos, rightRenderer));

//        // Start a coroutine to activate CompleteZone once doors are fully opened
//        StartCoroutine(ActivateCompleteZoneWhenDoorOpens());
//    }

//    private IEnumerator SlideAndFadeOut(Transform door, Vector3 targetPosition, SpriteRenderer renderer)
//    {
//        if (door == null)
//        {
//            Debug.LogError("ERROR: Door is NULL in SlideAndFadeOut!");
//            yield break;
//        }

//        if (renderer == null)
//        {
//            Debug.LogError("ERROR: SpriteRenderer is NULL for door: " + door.name);
//            yield break;
//        }

//        Color color = renderer.color;

//        while (Vector3.Distance(door.position, targetPosition) > 0.01f || color.a > 0f)
//        {
//            // Move the door
//            door.position = Vector3.MoveTowards(door.position, targetPosition, slideSpeed * Time.deltaTime);

//            // Fade out
//            color.a = Mathf.Max(0, color.a - fadeSpeed * Time.deltaTime);
//            renderer.color = color;

//            yield return null;
//        }

//        // Ensure it's fully faded out
//        renderer.color = new Color(color.r, color.g, color.b, 0);
//    }

//    private IEnumerator ActivateCompleteZoneWhenDoorOpens()
//    {
//        // Wait until both doors finish moving and fading out
//        yield return new WaitForSeconds(slideDistance / slideSpeed + 0.5f); // Adjust based on door animation

//        IsDoorFullyOpen = true; // Mark the door as fully open

//        // Enable the Complete Zone
//        if (completeZone != null)
//        {
//            completeZone.SetActive(true);
//            Debug.Log("Complete Zone Activated!");
//        }
//    }

//    public void AssignDoors(Transform left, Transform right)
//    {
//        leftDoor = left;
//        rightDoor = right;
//        leftRenderer = leftDoor.GetComponent<SpriteRenderer>();
//        rightRenderer = rightDoor.GetComponent<SpriteRenderer>();

//        if (leftRenderer == null || rightRenderer == null)
//        {
//            Debug.LogError("ERROR: SpriteRenderer is missing in DoorLeft or DoorRight.");
//        }
//    }
//}


using System.Collections;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform leftDoor;  // Assign DoorLeft in Inspector
    public Transform rightDoor; // Assign DoorRight in Inspector
    public GameObject completeZone; // Reference to the Complete Zone

    public float slideDistance = 2f;  // Distance each half moves
    public float slideSpeed = 3f;    // Speed of sliding
    public float fadeSpeed = 2f;     // Speed of fading effect
    public float walkIntoDoorDelay = 2f; // Extra delay before activating CompleteZone

    private Vector3 leftDoorClosedPos, rightDoorClosedPos;
    private Vector3 leftDoorOpenPos, rightDoorOpenPos;
    private SpriteRenderer leftRenderer, rightRenderer;

    public bool IsDoorFullyOpen { get; private set; } = false; // Track when doors fully open

    private void Start()
    {
        // Find the child objects dynamically
        leftDoor = transform.Find("DoorLeft");
        rightDoor = transform.Find("DoorRight");
        completeZone = transform.Find("CompleteZone")?.gameObject; // Find the CompleteZone inside Exit

        // Ensure they are found
        if (leftDoor == null) Debug.LogError("DoorLeft not found in children!");
        if (rightDoor == null) Debug.LogError("DoorRight not found in children!");
        if (completeZone == null) Debug.LogError("CompleteZone not found in children!");

        // Get the SpriteRenderer components
        leftRenderer = leftDoor?.GetComponent<SpriteRenderer>();
        rightRenderer = rightDoor?.GetComponent<SpriteRenderer>();

        // Ensure SpriteRenderer is assigned
        if (leftRenderer == null) Debug.LogError("SpriteRenderer is NULL for DoorLeft!");
        if (rightRenderer == null) Debug.LogError("SpriteRenderer is NULL for DoorRight!");

        // Store the original positions
        leftDoorClosedPos = leftDoor.position;
        rightDoorClosedPos = rightDoor.position;

        // Move the left door left, right door right
        leftDoorOpenPos = leftDoorClosedPos + Vector3.left * slideDistance;
        rightDoorOpenPos = rightDoorClosedPos + Vector3.right * slideDistance;

        // Ensure CompleteZone is disabled at the start
        if (completeZone != null)
        {
            completeZone.SetActive(false);
        }
    }

    public void OpenDoor()
    {
        StartCoroutine(SlideAndFadeOut(leftDoor, leftDoorOpenPos, leftRenderer));
        StartCoroutine(SlideAndFadeOut(rightDoor, rightDoorOpenPos, rightRenderer));

        // Start a coroutine to activate CompleteZone once doors are fully opened + delay
        StartCoroutine(ActivateCompleteZoneWithDelay());
    }

    private IEnumerator SlideAndFadeOut(Transform door, Vector3 targetPosition, SpriteRenderer renderer)
    {
        if (door == null)
        {
            Debug.LogError("ERROR: Door is NULL in SlideAndFadeOut!");
            yield break;
        }

        if (renderer == null)
        {
            Debug.LogError("ERROR: SpriteRenderer is NULL for door: " + door.name);
            yield break;
        }

        Color color = renderer.color;

        while (Vector3.Distance(door.position, targetPosition) > 0.01f || color.a > 0f)
        {
            // Move the door
            door.position = Vector3.MoveTowards(door.position, targetPosition, slideSpeed * Time.deltaTime);

            // Fade out
            color.a = Mathf.Max(0, color.a - fadeSpeed * Time.deltaTime);
            renderer.color = color;

            yield return null;
        }

        // Ensure it's fully faded out
        renderer.color = new Color(color.r, color.g, color.b, 0);
    }

    private IEnumerator ActivateCompleteZoneWithDelay()
    {
        // Wait until both doors finish moving and fading out
        yield return new WaitForSeconds(slideDistance / slideSpeed + 0.5f);

        IsDoorFullyOpen = true; // Mark the door as fully open

        Debug.Log("Doors fully opened. Waiting for player walk-in delay...");

        // Additional delay before activating the Complete Zone
        yield return new WaitForSeconds(walkIntoDoorDelay);

        // Enable the Complete Zone
        if (completeZone != null)
        {
            completeZone.SetActive(true);
            Debug.Log("Complete Zone Activated!");
        }
    }

    public void AssignDoors(Transform left, Transform right)
    {
        leftDoor = left;
        rightDoor = right;
        leftRenderer = leftDoor.GetComponent<SpriteRenderer>();
        rightRenderer = rightDoor.GetComponent<SpriteRenderer>();

        if (leftRenderer == null || rightRenderer == null)
        {
            Debug.LogError("ERROR: SpriteRenderer is missing in DoorLeft or DoorRight.");
        }
    }
}
