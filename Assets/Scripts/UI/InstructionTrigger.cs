using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject instructionBox; // Unique instruction box for this trigger

    private void Start()
    {
        if (instructionBox != null)
        {
            instructionBox.SetActive(false); // Ensure it's hidden at the start
        }
        else
        {
            Debug.LogError(gameObject.name + " is missing an Instruction Box! Assign one in the Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (instructionBox != null)
            {
                instructionBox.SetActive(true);
                Debug.Log(gameObject.name + " activated " + instructionBox.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (instructionBox != null)
            {
                instructionBox.SetActive(false);
                Debug.Log(gameObject.name + " deactivated " + instructionBox.name);
            }
        }
    }
}
