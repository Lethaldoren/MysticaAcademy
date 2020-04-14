using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBase<GameManager>
{
    public int killCount;
    public int winningKillCount;

    public GameObject menu;
    public GameObject player;
    public Transform spawn;    
    public UnityEvent OnComplete;
    
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
    }
    
    public void StartGame()
    {
        AIManager.Instance.gameObject.SetActive(true);
        menu.SetActive(false);
        player.transform.position = spawn.position;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
