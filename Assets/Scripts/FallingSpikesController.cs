using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    // find a way to make fewer fall at a time
    public float wait = 3.5f;
    public float repeatEvery = 2f;
    public GameObject fallingObject;

    public bool _isSpikeZone;

    
    void Start()
    {
        InvokeRepeating("Fall", wait, repeatEvery);
    }

    void Fall()
    {
        Instantiate(fallingObject, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);
    }
}