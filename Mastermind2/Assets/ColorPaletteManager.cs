using UnityEngine;
using UnityEngine.UI;


public class ColorPaletteManager : MonoBehaviour
{
   public GameObject[] colorButtons; // Array of buttons


   // Update buttons
   public void UpdatePalette(int colorCount)
   {
       Debug.Log("Updating palette with " + colorCount + " colors.");
       for (int i = 0; i < colorButtons.Length; i++)
       {
           bool isActive = i < colorCount;
           colorButtons[i].SetActive(isActive); // Show/hide the button
           Button buttonComponent = colorButtons[i].GetComponent<Button>();
           if (buttonComponent != null)
           {
               buttonComponent.interactable = isActive; // Disable interaction
           }
       }
   }
}
