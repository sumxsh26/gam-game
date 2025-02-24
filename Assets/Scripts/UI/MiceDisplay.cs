//using UnityEngine;
//using UnityEngine.UI;

//public class MiceDisplay : MonoBehaviour
//{
//    public Sprite emptyMouse;  // No mouse
//    public Sprite storedMouse; // Mouse placed in cage (filled)

//    public Image[] mouseIcons; // UI slots for displaying mice

//    public Cage cage;  // Reference to the cage

//    void Update()
//    {
//        int storedCount = cage.GetStoredMice(); // Mice stored in cage

//        for (int i = 0; i < mouseIcons.Length; i++)
//        {
//            if (i < storedCount)
//            {
//                mouseIcons[i].enabled = true;
//                mouseIcons[i].sprite = storedMouse;  // Show stored mouse (filled)
//            }
//            else
//            {
//                mouseIcons[i].enabled = true;
//                mouseIcons[i].sprite = emptyMouse; // Show empty mouse slot
//            }
//        }
//    }

//    public void UpdateDisplay()
//    {
//        // Update UI display when a mouse is stored in the cage
//        Update();
//    }
//}
