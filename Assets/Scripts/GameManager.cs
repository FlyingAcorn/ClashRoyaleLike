using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Camera mainCamera;
    [SerializeField] private bool inAMatch;
    private float _alliedMana;

    public float AlliedMana
    {
        get => _alliedMana;
        set
        {
            if (value > 10)
            {
                _alliedMana = 10;
            }
            else
            {
                _alliedMana = value;
            }
        }
    }

    private float _enemyMana;

    public float EnemyMana
    {
        get => _enemyMana;
        set
        {
            if (value > 10)
            {
                _enemyMana = 10;
            }
            else
            {
                _enemyMana = value;
            }
        }
    }

    [SerializeField] private float manaRegenRate;
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
            if (inAMatch == false)
            {
                //reset values shuffle deck
                AlliedMana = 0;
                EnemyMana = 0;
                alliedDeck.Shuffle();
                enemyDeck.Shuffle();
                UIManager.Instance.choosePanel.UpdateCards
                    (alliedDeck[0], alliedDeck[1], alliedDeck[2], alliedDeck[3], alliedDeck[4]);
                inAMatch = true;
            }

            StartCoroutine(StartMana());
            EnemyAiManager.Instance.UpdateAiState(EnemyAiManager.AiState.Wait);
        }

        if (newState == GameState.Settings)
        {
        }

        if (newState == GameState.Victory)
        {
            inAMatch = false;
        }

        if (newState == GameState.Defeat)
        {
            inAMatch = false;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private IEnumerator StartMana()
    {
        while (state == GameState.Play)
        {
            AlliedMana += manaRegenRate * Time.deltaTime;
            EnemyMana += manaRegenRate * Time.deltaTime;
            UIManager.Instance.choosePanel.manaSlider.value = _alliedMana;
            var mana = (int)_alliedMana; // verimsiz olabilir
            UIManager.Instance.choosePanel.currentManaText.text = mana.ToString();
            yield return null;
        }
    }

    public void AllyReDrawPile()
    {
        allyPlayedCards.Shuffle();
        foreach (var t in allyPlayedCards.ToList())
        {
            alliedDeck.Add(t);
            allyPlayedCards.Remove(t);
        }
    }

    public void EnemyReDrawPile()
    {
        enemyPlayedCards.Shuffle();
        foreach (var t in enemyPlayedCards.ToList())
        {
            enemyDeck.Add(t);
            enemyPlayedCards.Remove(t);
        }
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}