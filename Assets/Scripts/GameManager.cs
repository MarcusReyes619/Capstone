using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        switch (newState)
        {
            case GameState.TITLE:
            break;
            case GameState.STARTGAME:
                break;
            case GameState.OPENWORLD:
                break;
            case GameState.MISSION:
                break;
            case GameState.ENDMISSON:
                break;
            case GameState.PLAYERDEAD:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
