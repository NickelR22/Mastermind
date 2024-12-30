using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
   public GameObject slotPrefab;
   public Transform guessGrid;
   public int columns;
   public int colors;
   private List<int> secretCode = new List<int>();
   private Dictionary<int, int> playerGuessDict = new Dictionary<int, int>();
   private int currentTurn = 0;
   private List<GridSlot> gridSlots = new List<GridSlot>();
   public TextMeshProUGUI correctText;  // Reference for correct position count
   public TextMeshProUGUI misplacedText;  // Reference for misplaced color count
   public TextMeshProUGUI gameStatusText;
   private bool isGameOver = false;




   void Start()
   {
       columns = PlayerPrefs.GetInt("Columns", 4);
       colors = PlayerPrefs.GetInt("Colors", 4);


       Debug.Log("Columns: " + columns + ", Colors: " + colors);


       ColorPaletteManager paletteManager = FindFirstObjectByType<ColorPaletteManager>();
       if (paletteManager != null)
       {
           paletteManager.UpdatePalette(colors);
       }


       GenerateSecretCode();
       CreateGuessGrid(); // Initial grid creation
       UpdateGameStatus("Make your guess!", Color.gray);
   }


   void GenerateSecretCode()
   {
       secretCode.Clear();
       for (int i = 0; i < columns; i++)
       {
           secretCode.Add(Random.Range(0, colors));
       }


       Debug.Log("Secret Code: " + string.Join(", ", secretCode));
   }


   void CreateGuessGrid()
   {
       if (gridSlots.Count == 0)
       {
           int totalSlots = columns * 10; // Assume 10 rows for the grid
           for (int i = 0; i < totalSlots; i++)
           {
               GameObject slot = Instantiate(slotPrefab, guessGrid);
               GridSlot gridSlot = slot.GetComponent<GridSlot>();


               if (gridSlot != null)
               {
                   gridSlot.rowIndex = i / columns; // Calculate the row index based on the slot's position
                   gridSlot.columnIndex = i % columns; // Calculate the column index based on the slot's position
                   gridSlot.gameManager = this;
                   gridSlot.interactable = gridSlot.rowIndex == currentTurn;
                   gridSlots.Add(gridSlot); // Store reference to the slot
               }
           }
       }
       else
       {
           // Update existing slots for the current turn
           foreach (var gridSlot in gridSlots)
           {
               gridSlot.interactable = gridSlot.rowIndex == currentTurn;
           }
       }
   }


   public void NextTurn()
   {
       if (isGameOver) return;
       currentTurn++;
       if (currentTurn >= 10) // Prevent going beyond the last row
       {
           currentTurn = 9;
       }


       // Update the interactability of existing slots
       foreach (var gridSlot in gridSlots)
       {
           gridSlot.interactable = gridSlot.rowIndex == currentTurn;
           gridSlot.SetInteractable();
       }
       playerGuessDict.Clear();
   }


   public void SubmitGuess()
   {
       if (isGameOver) return;
       // Ensure the player has filled all slots in the current row
       if (playerGuessDict.Count < columns)
       {
           Debug.Log("Complete the row before submitting.");
           return;
       }


       // Assemble the guess in column order
       List<int> playerGuess = new List<int>();
       for (int i = 0; i < columns; i++)
       {
           playerGuess.Add(playerGuessDict[i]);
       }


       // Calculate feedback
       int correctPosition = 0;
       int correctColor = 0;


       // Create a copy of the secret code to track matches
       List<int> tempCode = new List<int>(secretCode);


       // Check for correct positions
       for (int i = 0; i < playerGuess.Count; i++)
       {
           if (playerGuess[i] == tempCode[i])
           {
               correctPosition++;
               tempCode[i] = -1; // Mark as matched
               playerGuess[i] = -2; // Mark as processed
           }
       }


       // Check for correct colors in the wrong position
       for (int i = 0; i < playerGuess.Count; i++)
       {
           if (playerGuess[i] != -2) // Skip already matched
           {
               int index = tempCode.IndexOf(playerGuess[i]);
               if (index != -1)
               {
                   correctColor++;
                   tempCode[index] = -1; // Mark as matched
               }
           }
       }


       if (correctText != null && misplacedText != null)
       {
           correctText.text += $"<color=red>{correctPosition}</color>\n"; // Append correct position count
           misplacedText.text += $"<color=white>{correctColor}</color>\n"; // Append misplaced color count
       }


       if (correctPosition == columns)
       {
           isGameOver = true;
           UpdateGameStatus("You win!", Color.green);
       }
       else if (currentTurn >= 9) // Last turn, and still not won
       {
           isGameOver = true;
           ShowLoseMessageWithSecretCode();
       }
       else
       {
           // Optionally: Add feedback visualization here
           NextTurn();
           playerGuess.Clear(); // Reset player guess
       }
   }


   // Called when a slot is clicked
   public void AddToPlayerGuess(int columnIndex, int colorIndex)
   {
       // Update the guess for the specific column
       playerGuessDict[columnIndex] = colorIndex;
   }


   void UpdateFeedback(int correctPosition, int correctColor)
   {
       // Update the feedback text next to the current row
       correctText.text = correctPosition.ToString();  // Display correct positions
       misplacedText.text = correctColor.ToString();  // Display misplaced colors
   }


   public void ExitGame()
   {
       ResetGame();
       SceneManager.LoadScene("HomeScene");
   }


   private void ResetGame()
   {
       currentTurn = 0;
       playerGuessDict.Clear();
       secretCode.Clear();
       GenerateSecretCode();
   }


   private void UpdateGameStatus(string message, Color textColor)
   {
       if (gameStatusText != null)
       {
           gameStatusText.text = message;
           gameStatusText.color = textColor;
       }
   }


   private void ShowLoseMessageWithSecretCode()
   {
       // Convert the secret code to a string with color representation
       string secretCodeMessage = "Secret Code: ";
       for (int i = 0; i < secretCode.Count; i++)
       {
           Color color;
           string colorName = GetColorForIndex(secretCode[i], out color);
           secretCodeMessage += $"{colorName} ";
       }


       UpdateGameStatus($"You lose. \n {secretCodeMessage}", Color.red);
   }


   // Get the color associated with the color index
   private string GetColorForIndex(int colorIndex, out Color color)
   {
       // Return the appropriate color based on the index (you can adjust this based on your color palette)
       switch (colorIndex)
       {
           case 0:
               color = Color.red;
               return "Red";
           case 1:
               color = Color.green;
               return "Green";
           case 2:
               color = Color.blue;
               return "Blue";
           case 3:
               color = Color.yellow;
               return "Yellow";
           case 4:
               color = new Color(1f, 0.647f, 0f); // Orange
               return "Orange";
           case 5:
               color = new Color(1f, 0.41f, 0.71f); // Pink
               return "Pink";
           case 6:
               color = Color.cyan;
               return "Cyan";
           case 7:
               color = new Color(0.6f, 0.4f, 0.2f); // Brown
               return "Brown";
           default:
               color = Color.white;
               return "White"; // Default to white if the index is invalid
       }
   }
}

