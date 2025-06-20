using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnGameStateChanged;

    public enum GameState
    {
        MainMenu,
        Play,
        Settings,
        Victory,
        Defeat
    }

    public GameState state;
    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }
    
    public void UpdateGameState(GameState newState)
    {
        state = newState;

        if (newState == GameState.MainMenu)
        {
            
        }

        if (newState == GameState.Play)
        {
            
        }

        if (newState == GameState.Settings)
        {
        }

        if (newState == GameState.Victory)
        {
            
        }
        if (newState == GameState.Defeat)
        {
            
        }
        OnGameStateChanged?.Invoke(newState);
    }
}
