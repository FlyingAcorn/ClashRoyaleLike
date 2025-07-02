using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAiManager : Singleton<EnemyAiManager>
{
    [SerializeField] private PlayZone allyZone;
    [SerializeField] private PlayZone enemyZone;
    [SerializeField] private List<Card> currentHand;
    [SerializeField] private Card chosenCard;
    private Vector3 _pointOfSummon;
    
    public static event Action<AiState> OnAiStateChanged;
    public enum AiState
    {
        Wait,
        Decide,
        Play
    }

    public AiState state;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
    }
    
    public void UpdateAiState(AiState newState)
    {
        state = newState;
        if (newState == AiState.Wait)
        {
            var enemyDeck = GameManager.Instance.enemyDeck;
            UpdateCards(enemyDeck[0],enemyDeck[1],enemyDeck[2],enemyDeck[3]);
            StartCoroutine(SelectCard());
        }
        if (newState == AiState.Decide)
        {
            //ışınlanacağı pozizyonu seçecek seçtiği pozizyonun etrafına randomrange ile hafif offset verecek
            //:TODO ya detaylı her kartın kendi spawn behaviorunu oluştur yada daha basit sistem yaz
            //basit sistem benim zoneda düşman varmı(canı azmı),invade edebiliyormuyum,menzillimiyim
            //gibi sorularla konumu ayarlayacak (düşman üstüne düşman kuleye yada node cevresine gibi)
            Debug.Log(chosenCard.name);
        }
        if (newState == AiState.Play)
        {
            foreach (var t in chosenCard.entities)
            {
                t.isAlly = false;
            }
            Instantiate(chosenCard, _pointOfSummon, Quaternion.identity, EntityManager.Instance.transform);
            GameManager.Instance.EnemyMana -= chosenCard.cardInfo.mana;
            GameManager.Instance.enemyDeck.Remove(chosenCard);
            GameManager.Instance.enemyPlayedCards.Add(chosenCard);
        }
        OnAiStateChanged?.Invoke(newState);
    }

    private void UpdateCards(Card leftMost, Card leftMiddle, Card middleRight,Card rightMost)
    {
        currentHand[0] = leftMost;
        currentHand[1] = leftMiddle;
        currentHand[2] = middleRight;
        currentHand[3] = rightMost;
        if (GameManager.Instance.enemyDeck.Count ==5)
        {
            GameManager.Instance.EnemyReDrawPile();
        }
    }

    private IEnumerator SelectCard()
    {
        var orderedHand = currentHand.OrderByDescending(t => t.cardInfo.mana).Reverse().ToList();
        var randomCard = Random.Range(0,4);
        while (GameManager.Instance.EnemyMana > orderedHand[randomCard].cardInfo.mana)
        {
            yield return null;
        }
        chosenCard = orderedHand[Random.Range(0, randomCard)];
        UpdateAiState(AiState.Decide);
    }
    
}
