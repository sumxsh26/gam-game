using System.Collections;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    [SerializeField] DoorBehaviour _doorBehaviour;

    [SerializeField] bool _isDoorOpenSwitch;
    [SerializeField] bool _isDoorCloseSwitch;

    //contain half the size of the button
    float _switchSizeY;
    Vector3 _switchUpPos;
    Vector3 _switchDownPos;
    float _switchSpeed = 1f;

    //delay for button to go back up, bfr allowed to click again
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
        if (_isPressingSwitch)
        {
            MoveSwitchDown();
        }
        else if (!_isPressingSwitch)
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
        if (collision.CompareTag("Player"))
        {
            //toggle-ing; if true make false, if false make true
            _isPressingSwitch = !_isPressingSwitch;

            //if door is close, and switch open
            if (_isDoorOpenSwitch && !_doorBehaviour._isDoorOpen)
            {
                _doorBehaviour._isDoorOpen = !_doorBehaviour._isDoorOpen;
            }
            else if (_isDoorCloseSwitch && _doorBehaviour._isDoorOpen)
            {
                _doorBehaviour._isDoorOpen = !_doorBehaviour._isDoorOpen;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SwitchUpDelay(_switchDelay));
        }
    }

    IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isPressingSwitch = false;
    }

}
