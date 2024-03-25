using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            GamePause();

        }
    }
    public void GamePause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void StartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");

    }
    public void ExitGme()
    {
        Application.Quit();
    }
}
