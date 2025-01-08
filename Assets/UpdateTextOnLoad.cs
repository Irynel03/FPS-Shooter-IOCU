using TMPro;
using UnityEngine;

public class UpdateTextOnLoad : MonoBehaviour
{
    public TextMeshProUGUI textMesh; // Assign this in the inspector
    private string scoreKey = "score"; // Path to the text file

    void Start()
    {
        textMesh.text = PlayerPrefs.GetInt(scoreKey).ToString(); // Append additionalText to the existing text
    }
}