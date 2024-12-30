using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeScreenController : MonoBehaviour
{
    public InputField columnsInputField;
    public InputField colorsInputField;
    public Text errorMessage; // Text to display error messages

    public void StartGame()
    {
        if (int.TryParse(columnsInputField.text, out int columns) && int.TryParse(colorsInputField.text, out int colors))
        {
            // Check if the input values are within the allowed range
            if (columns >= 4 && columns <= 8 && colors >= 4 && colors <= 10)
            {
                // Save the values for the next scene
                PlayerPrefs.SetInt("Columns", columns);
                PlayerPrefs.SetInt("Colors", colors);

                // Load the game scene
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                // Display an error message
                errorMessage.text = "Columns and Colors must be between 4 and 8.";
            }
        }
        else
        {
            // Display an error message for invalid input
            errorMessage.text = "Please enter valid numbers.";
        }
    }
}