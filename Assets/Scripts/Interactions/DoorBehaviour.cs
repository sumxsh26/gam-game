using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    public bool _isDoorOpen = false;
    Vector3 _doorClosedPos;
    Vector3 _doorOpenPos;

    //control door opening speed
    float _doorSpeed = 10f;

    void Awake()
    {
        _doorClosedPos = transform.position;
        _doorOpenPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
    }

    // Update
    void Update()
    {
        if (_isDoorOpen)
        {
            OpenDoor();
        }
        else if (!_isDoorOpen)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (transform.position != _doorOpenPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _doorOpenPos, _doorSpeed * Time.deltaTime);
        }
    }

    void CloseDoor()
    {
        if (transform.position != _doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _doorClosedPos, _doorSpeed * Time.deltaTime);
        }
    }

}
