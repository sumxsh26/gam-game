using UnityEngine;

public class CageScript : MonoBehaviour
{
    public bool locked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        locked = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mice"))
        {
            locked = false;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mice"))
        {
            locked = true;
        }
    }
}
