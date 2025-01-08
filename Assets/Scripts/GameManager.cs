using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string scoreKey = "score";

    public void StartGame()
    {
        PauseMenu.isPaused = false;
        // Load the game scene
        PlayerPrefs.SetInt(scoreKey, 150);

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

    public void Level2()
    {
        //load the next level
        SceneManager.LoadSceneAsync(2);

        PauseMenu.isPaused = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
