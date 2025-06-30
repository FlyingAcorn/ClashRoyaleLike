using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public float alliedMana;
    public float enemyMana;
    public List<Card> alliedDeck;
    public List<Card> allyPlayedCards;
    public List<Card> enemyDeck;
    public List<Card> enemyPlayedCards;
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
        UpdateGameState(GameState.Play);
    }
    
    public void UpdateGameState(GameState newState)
    {
        state = newState;

        if (newState == GameState.MainMenu)
        {
            
        }

        if (newState == GameState.Play)
        {
            //EntityManager.Instance.InitialSort();
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
