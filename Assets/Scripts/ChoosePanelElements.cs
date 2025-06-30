using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class ChoosePanelElements : MonoBehaviour
{
    [SerializeField] private Slider manaSlider;
    [SerializeField] private List<UiCard> uiCards;
    [SerializeField] private Image nextCardImage;
    [SerializeField] private TextMeshProUGUI currentManaText;
    [SerializeField] private TextMeshProUGUI timerText;
    // Game managerden aldığı verilere gore bu paneldeki şeylerin değerlerini değiştirecek
    //ui cards classı kendi en end vs durumlarında mana ve liste değerlerini değiştirecek
    // deck eleman sayısı 5 ise played cards shuffle at ve geri ekle yapacak
   
}
