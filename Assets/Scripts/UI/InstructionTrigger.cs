//using UnityEngine;

//public class InstructionTrigger : MonoBehaviour
//{
//    [SerializeField] private GameObject instructionBox; // Unique instruction box for this trigger

//    private void Start()
//    {
//        if (instructionBox != null)
//        {
//            instructionBox.SetActive(false); // Ensure it's hidden at the start
//        }
//        else
//        {
//            Debug.LogError(gameObject.name + " is missing an Instruction Box! Assign one in the Inspector.");
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            if (instructionBox != null)
//            {
//                instructionBox.SetActive(true);
//                Debug.Log(gameObject.name + " activated " + instructionBox.name);
//            }
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            if (instructionBox != null)
//            {
//                instructionBox.SetActive(false);
//                Debug.Log(gameObject.name + " deactivated " + instructionBox.name);
//            }
//        }
//    }
//}


//using UnityEngine;

//public class InstructionTrigger : MonoBehaviour
//{
//    [SerializeField] private GameObject instructionPrefab; // Assign the floating UI prefab
//    [TextArea]
//    [SerializeField] private string instructionText; // The message to show

//    private GameObject spawnedBox;
//    private Transform player;

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && spawnedBox == null)
//        {
//            player = other.transform;

//            // Spawn the instruction box
//            spawnedBox = Instantiate(instructionPrefab, player.position + Vector3.up * 2f, Quaternion.identity);
//            spawnedBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = instructionText;

//            // Make it follow the player
//            spawnedBox.GetComponent<FollowPlayer>().target = player;
//        }
//    }

//    void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && spawnedBox != null)
//        {
//            Destroy(spawnedBox);
//        }
//    }
//}
