using UnityEngine;
using UnityEngine.UI;


public class GridSlot : MonoBehaviour
{
   private Image slotImage; // To change the slot's color
   private Button button; // Reference to the Button component
   private ColorSelector colorSelector; // Reference to the ColorSelector script
   public int rowIndex; // Row index for each slot
   public int columnIndex; // Column index for each slot
   public bool interactable;
   private Color originalColor;
   public GameManager gameManager; // Reference to GameManager


   void Start()
   {
       slotImage = GetComponent<Image>(); // Get the Image component of the slot
       button = GetComponent<Button>(); // Get the Button component
       if (slotImage == null)
       {
           Debug.LogError($"Slot {gameObject.name} is missing an Image component!");
       }
       if (button == null)
       {
           Debug.LogError($"Slot {gameObject.name} is missing a Button component!");
       }


       colorSelector = FindFirstObjectByType<ColorSelector>(); // Find the ColorSelector in the scene
      
       SetInteractable();
   }


   public void OnSlotClicked()
   {
       if (colorSelector != null && button.interactable) // Check if the slot is interactable
       {
           // Get the selected color
           int selectedColorIndex = colorSelector.GetSelectedColorIndex();
           Color selectedColor = colorSelector.GetSelectedColor();
           selectedColor.a = 1.0f;


           // Update the slot's color
           slotImage.color = selectedColor;
           originalColor = slotImage.color;
           gameManager.AddToPlayerGuess(columnIndex, selectedColorIndex);


       }
   }


   public void SetInteractable()
   {
       button.interactable = interactable; // Enable or disable the button's interactability
       if (!interactable)
       {
           Color dimmedColor = originalColor * 0.75f;
           slotImage.color = new Color(dimmedColor.r, dimmedColor.g, dimmedColor.b, 1.0f);
       }
   }
}
