using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject completeLevelUI;

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

    public void CompleteLevel1()
    {
        //load the next level
        SceneManager.LoadSceneAsync(3);
    }
}
