using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class ChoosePanelElements : MonoBehaviour
{
    public Slider manaSlider;
    public List<UiCard> uiCards;
    public Image nextCardImage;
    public TextMeshProUGUI currentManaText;
    public TextMeshProUGUI timerText;
    // Game managerden aldığı verilere gore bu paneldeki şeylerin değerlerini değiştirecek
    //ui cards classı kendi en end vs durumlarında mana ve liste değerlerini değiştirecek
    // deck eleman sayısı 5 ise played cards shuffle at ve geri ekle yapacak
    public void UpdateCards(Card leftMost, Card leftMiddle, Card middleRight,Card rightMost,Card nextCard)
    {
        uiCards[0].currentCard = leftMost;
        uiCards[1].currentCard = leftMiddle;
        uiCards[2].currentCard = middleRight;
        uiCards[3].currentCard = rightMost;
        nextCardImage.sprite = nextCard.cardInfo.image;
        foreach (var t in uiCards)
        {
            t.myManaText.text = t.currentCard.cardInfo.mana.ToString();
            t.myImage.sprite = t.currentCard.cardInfo.image;
        }
        if (GameManager.Instance.alliedDeck.Count ==5)
        {
            GameManager.Instance.AllyReDrawPile();
        }
    }
}
