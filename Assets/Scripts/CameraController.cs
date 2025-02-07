using UnityEngine;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{

    private Vector3 targetPoint = Vector3.zero;

    public PlayerController player;

    public float lookAheadDistance = 5f, lookAheadSpeed = 3f;

    private float lookOffset;

    private bool isFalling;

    public float maxVertOffset = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    private void LateUpdate()
    {
        //targetPoint.y = player.transform.position.y;
        if (player.touchingDirections.IsGrounded)
        {
            targetPoint.y = player.transform.position.y;
        }

        if(transform.position.y - player.transform.position.y > maxVertOffset)
        {
            isFalling = true;
        }

        if (isFalling)
        {
            targetPoint.y = player.transform.position.y;

            if (player.touchingDirections.IsGrounded) 
            { 
                isFalling = false;
            }
        }

        //if (targetPoint.y < 0)
        //{
        //    targetPoint.y = 0;
        //}

        if (player.rb.linearVelocity.x > 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }

        if (player.rb.linearVelocity.x < 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, -lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }
        targetPoint.z = -10;
        targetPoint.x = player.transform.position.x + lookOffset;

        float playerMoveSpeed = player.CurrentMoveSpeed;
        transform.position = Vector3.Lerp(transform.position, targetPoint, playerMoveSpeed * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        //cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
    }
}
