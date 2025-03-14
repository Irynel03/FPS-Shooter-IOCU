using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject completeLevelUI;
    private string scoreKey = "score";
    public int targetsRescued = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key was pressed");
            //check if game object with tag "rescue_target" is in the radius of the player, increase the targetsRescued by 1 and destroy the game object
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "rescue_target")
                {
                    targetsRescued += 1;
                    Destroy(hitCollider.gameObject);
                    if (targetsRescued >= 3)
                    {
                        completeLevelUI.SetActive(true);

                        PauseMenu.isPaused = true;
                        Time.timeScale = 0f;

                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;

                        //var currentScore = PlayerPrefs.GetInt(scoreKey);
                        PlayerPrefs.SetInt(scoreKey, PlayerController.playerScore);
                        PlayerController.playerScore = 150;
                    }
                }

                if (hitCollider.gameObject.tag == "water")
                {
                    Debug.Log("Player has fallen into the water");
                    SceneManager.LoadSceneAsync(2);
                }
                if (hitCollider.gameObject.tag == "Win_Condition")
                {
                    Debug.Log("Player has won");

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    var level1Score = PlayerPrefs.GetInt(scoreKey);
                    PlayerPrefs.SetInt(scoreKey, PlayerController.playerScore + level1Score);
                    SceneManager.LoadSceneAsync(4);
                }
            }
        }

    }
}
