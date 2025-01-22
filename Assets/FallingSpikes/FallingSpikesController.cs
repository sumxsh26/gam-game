using UnityEngine;

public class FallingSpikesController : MonoBehaviour
{
    float wait = 0.1f;
    public GameObject fallingSpike;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // to make the fall repeat, wait is time btw dropping objects
        InvokeRepeating("Fall", wait, wait);
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(fallingSpike, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);
    }
}
