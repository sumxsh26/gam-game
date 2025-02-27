using System.Collections;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform leftDoor;  // Assign Door_Left in Inspector
    public Transform rightDoor; // Assign Door_Right in Inspector

    public float slideDistance = 2f;  // Distance each half moves
    public float slideSpeed = 3f;    // Speed of sliding
    public float fadeSpeed = 2f;     // Speed of fading effect

    private Vector3 leftDoorClosedPos, rightDoorClosedPos;
    private Vector3 leftDoorOpenPos, rightDoorOpenPos;
    private SpriteRenderer leftRenderer, rightRenderer;

    //private void Start()
    //{
    //    // Store the original positions
    //    leftDoorClosedPos = leftDoor.position;
    //    rightDoorClosedPos = rightDoor.position;

    //    // Move the left door left, right door right
    //    leftDoorOpenPos = leftDoorClosedPos + Vector3.left * slideDistance;
    //    rightDoorOpenPos = rightDoorClosedPos + Vector3.right * slideDistance;

    //    // Get sprite renderers
    //    leftRenderer = leftDoor.GetComponent<SpriteRenderer>();
    //    rightRenderer = rightDoor.GetComponent<SpriteRenderer>();
    //}

    private void Start()
    {
        // Find the child objects dynamically
        leftDoor = transform.Find("DoorLeft");
        rightDoor = transform.Find("DoorRight");

        // Ensure they are found
        if (leftDoor == null) Debug.LogError("DoorLeft not found in children!");
        if (rightDoor == null) Debug.LogError("DoorRight not found in children!");

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
    }


    public void OpenDoor()
    {
        StartCoroutine(SlideAndFadeOut(leftDoor, leftDoorOpenPos, leftRenderer));
        StartCoroutine(SlideAndFadeOut(rightDoor, rightDoorOpenPos, rightRenderer));
    }

    //private IEnumerator SlideAndFadeOut(Transform door, Vector3 targetPosition, SpriteRenderer renderer)
    //{
    //    Color color = renderer.color;

    //    while (Vector3.Distance(door.position, targetPosition) > 0.01f || color.a > 0f)
    //    {
    //        // Move the door
    //        door.position = Vector3.MoveTowards(door.position, targetPosition, slideSpeed * Time.deltaTime);

    //        // Fade out
    //        color.a = Mathf.Max(0, color.a - fadeSpeed * Time.deltaTime);
    //        renderer.color = color;

    //        yield return null;
    //    }

    //    // Ensure it's fully faded out
    //    renderer.color = new Color(color.r, color.g, color.b, 0);
    //}

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
