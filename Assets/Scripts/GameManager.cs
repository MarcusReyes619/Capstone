using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject startUI;
   
    [SerializeField] PlayerMovement player;
    [SerializeField] GameObject cam;

    public GameState gameState;

    public static event Action<GameState> OnGameStateChanged;
    public enum GameState
    {
        TITLE,
        STARTGAME,
        OPENWORLD,
        MISSION,
        ENDMISSON,
        PLAYERDEAD

    }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;

       

        OnGameStateChanged?.Invoke(newState);
    }
    // Start is called before the first frame update

    public void StartGame()
    {
        gameState = GameState.STARTGAME;
        Debug.Log("Working");
    }

    public void ExitApp()
    {
        Application.Quit();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.TITLE:
                break;
            case GameState.STARTGAME:
                startUI.SetActive(false);
                gameState = GameState.OPENWORLD;
                break;
            case GameState.OPENWORLD:
                player.enabled = true;
                cam.gameObject.SetActive(true);
                break;
            case GameState.MISSION:
                break;
            case GameState.ENDMISSON:
                break;
            case GameState.PLAYERDEAD:
                break;
        }
    }
}
