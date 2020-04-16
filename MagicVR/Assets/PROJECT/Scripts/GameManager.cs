using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using TMPro;
public class GameManager : SingletonBase<GameManager>
{
    public int killCount;
    public int winningKillCount;
    bool gameStarted = false; 

    public GameObject menu;

    public GameObject gameOverMenu;
    public GameObject victoryMenu;

    public GameObject player;
    public Transform spawn;
    public UnityEvent OnComplete;

    public int TimeLimit;
    float currentTimeTaken;
    public TextMeshProUGUI TimerText;
    void Awake()
    {
        AIManager.Instance.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        currentTimeTaken += Time.deltaTime;

        if (currentTimeTaken > TimeLimit)
            GameOver();

        if (gameStarted)
        UpdateTimer();
    }

    void UpdateTimer()
    {
        float TimeLeft = TimeLimit - Mathf.Round(currentTimeTaken);

        int minutes = 0;
        float minuteTime = TimeLeft;
        
        while (minuteTime > 60)
        {
            minuteTime -= 60;
            minutes++;
        }

        TimerText.text = minutes + ":" + minuteTime; 
    }

    public void StartGame()
    {
        AIManager.Instance.gameObject.SetActive(true);
        menu.SetActive(false);
        player.transform.position = spawn.position;
        gameStarted = true; 
    }

    public void GameOver ()
    {
        
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);

        gameOverMenu.transform.position = player.transform.position + (player.transform.forward * 1.5f) + (Vector3.up * 1.5f);
    }

    public void Victory ()
    {
        Time.timeScale = 0;
        victoryMenu.SetActive(true);

        victoryMenu.transform.position = player.transform.position + (player.transform.forward * 1.5f) + (Vector3.up * 1.5f) ;
    }

    public void GameRestart ()
    {
        Time.timeScale = 1;
        gameOverMenu.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
