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
    [SerializeField] private LayerMask layerMask;


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
            UpdateCards(enemyDeck[0], enemyDeck[1], enemyDeck[2], enemyDeck[3]);
            StartCoroutine(SelectCard());
        }

        if (newState == AiState.Decide)
        {
            //ışınlanacağı pozizyonu seçecek seçtiği pozizyonun etrafına randomrange ile hafif offset verecek
            //:TODO ya detaylı her kartın kendi spawn behaviorunu oluştur yada daha basit sistem yaz
            //basit sistem benim zoneda düşman varmı(canı azmı),invade edebiliyormuyum,menzillimiyim
            //gibi sorularla konumu ayarlayacak (düşman üstüne düşman kuleye yada node cevresine gibi)
            StartCoroutine(SelectPosition());
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
            UpdateAiState(AiState.Wait);
        }

        OnAiStateChanged?.Invoke(newState);
    }

    private void UpdateCards(Card leftMost, Card leftMiddle, Card middleRight, Card rightMost)
    {
        currentHand[0] = leftMost;
        currentHand[1] = leftMiddle;
        currentHand[2] = middleRight;
        currentHand[3] = rightMost;
        if (GameManager.Instance.enemyDeck.Count == 5)
        {
            GameManager.Instance.EnemyReDrawPile();
        }
    }

    private IEnumerator SelectCard()
    {
        var orderedHand = currentHand.OrderByDescending(t => t.cardInfo.mana).Reverse().ToList();
        var randomCard = Random.Range(0, 4);
        while (GameManager.Instance.EnemyMana < orderedHand[randomCard].cardInfo.mana)
        {
            yield return null;
        }

        chosenCard = orderedHand[Random.Range(0, randomCard)];
        UpdateAiState(AiState.Decide);
    }

    private IEnumerator SelectPosition()
    {
        Vector3 chosenPoint;
        if (chosenCard.cardInfo.canInvade)
        {
            chosenPoint = Random.Range(0, 4) <= 2
                ? EntityManager.Instance.allies.First(t => t.Health == EntityManager.Instance.allies.Min(x => x.Health))
                    .transform.position
                : allyZone.nodes[Random.Range(0, allyZone.nodes.Count)].transform.position;
        }
        else
        {
            bool trespassers = enemyZone.Trespassers.Any();
            chosenPoint = (chosenCard.cardInfo.hasRanged, trespassers) switch
            {
                (true, true) => enemyZone.Trespassers
                    .First(t => t.Health == enemyZone.Trespassers.Min(x => x.Health))
                    .transform.position,
                (false, true) => Random.Range(0, 4) <= 2
                    ? enemyZone.Trespassers.First(t => t.Health == enemyZone.Trespassers.Min(x => x.Health))
                        .transform.position
                    : enemyZone.nodes[Random.Range(0, enemyZone.nodes.Count)].transform.position,
                (true, false) => enemyZone.nodes[Random.Range(0, enemyZone.nodes.Count)].transform.position,
                (false, false) => enemyZone.nodes[Random.Range(0, enemyZone.nodes.Count)].transform.position
            };
        }

        var randomRange = Random.onUnitSphere * chosenCard.entities[0].entityClassType.rangeRadius;
        var offset = new Vector3(randomRange.x, 0, randomRange.z);
        Ray ray = new Ray(Camera.main.transform.position,
            (chosenPoint + offset - Camera.main.transform.position).normalized);
        while (!Physics.Raycast(ray, out var hit, 100, layerMask) ||
               hit.transform.TryGetComponent(out PlayZone zone) && chosenCard.cardInfo.canInvade
                   ? !this
                   : zone.isAllyZone) // this kullanma nedenin empty yapamadın
        {
            randomRange = Random.onUnitSphere * chosenCard.entities[0].entityClassType.rangeRadius;
            offset = new Vector3(randomRange.x, 0, randomRange.z);
            ray = new Ray(Camera.main.transform.position,
                (chosenPoint + offset - Camera.main.transform.position).normalized);
            // yield return null;
        }

        _pointOfSummon = chosenPoint + offset;
        UpdateAiState(AiState.Play);
        yield return null;
    }
}