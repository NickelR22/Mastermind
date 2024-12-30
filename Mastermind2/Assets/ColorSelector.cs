using UnityEngine;
using UnityEngine.UI;


public class ColorSelector : MonoBehaviour
{
   public Button[] colorButtons; // Buttons for selecting colors
   public Color[] availableColors; // List of possible colors
   private int selectedColorIndex = 0; // Tracks the selected color


   void Start()
   {
       // Set up the buttons and assign colors
       //UpdateColorButtons();


       for (int i = 0; i < colorButtons.Length; i++)
       {
           int colorIndex = i; // Capture index for button
           colorButtons[i].onClick.AddListener(() => SelectColor(colorIndex));
       }
   }


   // Update which color buttons are visible and assign colors
   public void UpdateColorButtons()
   {
       int numberOfColors = availableColors.Length; // Adjust based on dynamic input


       for (int i = 0; i < colorButtons.Length; i++)
       {
           if (i < numberOfColors)
           {
               colorButtons[i].gameObject.SetActive(true); // Show button
               colorButtons[i].GetComponent<Image>().color = availableColors[i]; // Assign color
           }
           else
           {
               colorButtons[i].gameObject.SetActive(false); // Hide extra buttons
           }
       }
   }


   // Select a color when the button is clicked
   public void SelectColor(int index)
   {
       selectedColorIndex = index;
   }


   public Color GetSelectedColor()
   {
       return availableColors[selectedColorIndex];
   }


   public int GetSelectedColorIndex()
   {
       return selectedColorIndex;
   }
}
