using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadSceneAsync(1);
    }

    public void MainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadSceneAsync(0);
        PauseMenu.isPaused = false;
    }

    public void ExitGame()
    {
        // Quit the game
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
