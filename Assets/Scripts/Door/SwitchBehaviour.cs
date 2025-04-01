//using System.Collections;
//using UnityEngine;

//public class SwitchBehaviour : MonoBehaviour
//{
//    [SerializeField] DoorBehaviour _doorBehaviour;

//    [SerializeField] bool _isDoorOpenSwitch;
//    [SerializeField] bool _isDoorCloseSwitch;

//    //contain half the size of the button
//    float _switchSizeY;
//    Vector3 _switchUpPos;
//    Vector3 _switchDownPos;
//    float _switchSpeed = 1f;

//    //delay for button to go back up, bfr allowed to click again
//    float _switchDelay = 0.2f;

//    bool _isPressingSwitch = false;


//    void Awake()
//    {
//        //get half size of this switch
//        _switchSizeY = transform.localScale.y / 2;

//        _switchUpPos = transform.position;
//        _switchDownPos = new Vector3(transform.position.x, transform.position.y - _switchSizeY, transform.position.z);

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (_isPressingSwitch)
//        {
//            MoveSwitchDown();
//        }
//        else if (!_isPressingSwitch)
//        {
//            MoveSwitchUp();
//        }
//    }

//    void MoveSwitchDown()
//    {
//        if (transform.position != _switchDownPos)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, _switchDownPos, _switchSpeed * Time.deltaTime);
//        }
//    }

//    void MoveSwitchUp()
//    {
//        if (transform.position != _switchUpPos)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, _switchUpPos, _switchSpeed * Time.deltaTime);
//        }
//    }


//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            _isPressingSwitch = true;
//            _doorBehaviour.SetDoorState(!_doorBehaviour._isDoorOpen);
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player") && gameObject.activeInHierarchy)
//        {
//            StartCoroutine(SwitchUpDelay(_switchDelay));
//        }
//    }


//    IEnumerator SwitchUpDelay(float waitTime)
//    {
//        yield return new WaitForSeconds(waitTime);
//        _isPressingSwitch = false;
//    }

//}


using System.Collections;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    [SerializeField] DoorBehaviour _doorBehaviour;

    // Remove the _isDoorCloseSwitch and related logic, we only need to open the door
    [SerializeField] bool _isDoorOpenSwitch;

    //contain half the size of the button
    float _switchSizeY;
    Vector3 _switchUpPos;
    Vector3 _switchDownPos;
    float _switchSpeed = 1f;

    //delay for button to go back up, before allowed to click again
    float _switchDelay = 0.2f;

    bool _isPressingSwitch = false;

    void Awake()
    {
        //get half size of this switch
        _switchSizeY = transform.localScale.y / 2;

        _switchUpPos = transform.position;
        _switchDownPos = new Vector3(transform.position.x, transform.position.y - _switchSizeY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Always allow movement to complete (even if door is open)
        if (_isPressingSwitch)
        {
            MoveSwitchDown();
        }
        else
        {
            MoveSwitchUp();
        }
    }


    void MoveSwitchDown()
    {
        if (transform.position != _switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _switchDownPos, _switchSpeed * Time.deltaTime);
        }
    }

    void MoveSwitchUp()
    {
        if (transform.position != _switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _switchUpPos, _switchSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_doorBehaviour._isDoorOpen)
        {
            _isPressingSwitch = true;
            _doorBehaviour.SetDoorState(true); // Open the door
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            StartCoroutine(SwitchUpDelay(_switchDelay));
        }
    }

    IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isPressingSwitch = false;
    }
    public void ResetSwitch()
    {
        _isPressingSwitch = false;

        // Also reset door state
        _doorBehaviour.SetDoorState(false);

        // Force move the switch back to its original up position
        transform.position = _switchUpPos;
    }


}
