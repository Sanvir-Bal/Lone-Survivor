using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Defeat_Menu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject defeatMenuUI;
    public bool playerDeath;

    void Update()
    {
        playerDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerDeath;
        if (playerDeath)
        {
            Pause();
        }
    }

    void Pause()
    {
        defeatMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Restart()
    {
        defeatMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerDeath = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Lone Survivor");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
