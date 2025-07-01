using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiCard : MonoBehaviour,IEndDragHandler,IBeginDragHandler,IDragHandler
{
    public Card currentCard;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private RectTransform myTransform;
     public Image myImage;
    private Vector3 _localPosition;
    private Vector2 _anchoredPosition;
    private Vector2 _sizeDelta;
    private Vector2 _anchorMin;
    private Vector2 _anchorMax;
    private Vector2 _pivot;
    private Vector3 _scale;
    private Quaternion _rotation;

    private void Start()
    {
        _localPosition = myTransform.localPosition;
        _anchorMin = myTransform.anchorMin;
        _anchorMax = myTransform.anchorMax;
        _anchoredPosition = myTransform.anchoredPosition;
        _sizeDelta = myTransform.sizeDelta;
        _pivot = myTransform.pivot;
        _scale = myTransform.localScale;
        _rotation = myTransform.localRotation;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        

    }
    public void OnDrag(PointerEventData eventData)
    {
        myTransform.anchoredPosition += eventData.delta/myCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        myTransform.localPosition =_localPosition;
        myTransform.anchorMin =_anchorMin;
        myTransform.anchorMax = _anchorMax;
        myTransform.anchoredPosition =_anchoredPosition;
        myTransform.sizeDelta =_sizeDelta;
        myTransform.pivot = _pivot;
        myTransform.localScale = _scale;
        myTransform.localRotation = _rotation;
        // manası yetmezse return
        if (currentCard.cardInfo.mana <= GameManager.Instance.AlliedMana)
        {
            GameManager.Instance.AlliedMana -= currentCard.cardInfo.mana;
            GameManager.Instance.alliedDeck.Remove(currentCard);
            GameManager.Instance.allyPlayedCards.Add(currentCard);
            var allydeck = GameManager.Instance.alliedDeck;
            UIManager.Instance.choosePanel.UpdateCards(allydeck[0],allydeck[1],allydeck[2],allydeck[3],allydeck[4]);
        }
    }
}
