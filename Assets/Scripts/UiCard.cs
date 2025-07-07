using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiCard : MonoBehaviour
{
    public Card currentCard;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private RectTransform myTransform;
    [SerializeField] private LayerMask mask;
    [SerializeField] private TextMeshProUGUI infoText;
    private bool _canSummon;
    private Vector3 _pointOfSummon;
    private PreviewModel _previewModel;


    public Image myImage;
    public TextMeshProUGUI myManaText;
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

    public void OnBeginDrag()
    {
        _previewModel = Instantiate(currentCard.cardInfo.previewModel);
        _previewModel.gameObject.SetActive(false);
        infoText.gameObject.SetActive(true);
    }

    public void OnDrag(Touch eventData)
    {
        _canSummon = false;
        myTransform.anchoredPosition += eventData.deltaPosition / myCanvas.scaleFactor;
        infoText.gameObject.transform.position = eventData.position + new Vector2(0, 125);
        Ray ray = GameManager.Instance.mainCamera.ScreenPointToRay(eventData.position);
        if (!Physics.Raycast(ray, out var hit, 100, mask))
        {
            myImage.enabled = true;
            myManaText.enabled = true;
            _previewModel.gameObject.SetActive(false);
            infoText.text = "";
            return;
        }

        myManaText.enabled = false;
        myImage.enabled = false;
        _previewModel.gameObject.transform.position = hit.point;
        _previewModel.gameObject.SetActive(true);
        if (!currentCard.cardInfo.canInvade && !hit.transform.GetComponent<PlayZone>().isAllyZone)
        {
            _previewModel.material.color = _previewModel.toRed;
            infoText.text = "You cannot summon.";
            return;
        }

        if (GameManager.Instance.AlliedMana < currentCard.cardInfo.mana)
        {
            _previewModel.material.color = _previewModel.toRed;
            infoText.text = "You need" + " " + (int)GameManager.Instance.AlliedMana + "/" + currentCard.cardInfo.mana +
                            " to summon this.";
        }
        else
        {
            _previewModel.material.color = _previewModel.toGreen;
            _pointOfSummon = hit.point;
            _canSummon = true;
            infoText.text = "You can summon.";
        }
    }

    public void ClickEvent()
    {
        InputManager.Instance.selectedUiCard = this;
    }

    public void OnEndDrag()
    {
        myManaText.enabled = true;
        myTransform.localPosition = _localPosition;
        myTransform.anchorMin = _anchorMin;
        myTransform.anchorMax = _anchorMax;
        myTransform.anchoredPosition = _anchoredPosition;
        myTransform.sizeDelta = _sizeDelta;
        myTransform.pivot = _pivot;
        myTransform.localScale = _scale;
        myTransform.localRotation = _rotation;
        infoText.text = "";
        infoText.gameObject.SetActive(false);
        myImage.enabled = true;
        _previewModel.gameObject.SetActive(false);
        Destroy(_previewModel);
        _previewModel = null;
        if (!_canSummon) return;
        if (!(currentCard.cardInfo.mana <= GameManager.Instance.AlliedMana)) return;
        foreach (var t in currentCard.entities) // objeyi ally yapıyor
        {
            t.isAlly = true;
        }

        Instantiate(currentCard, _pointOfSummon, Quaternion.identity, EntityManager.Instance.entitiesOnMap.transform);
        GameManager.Instance.AlliedMana -= currentCard.cardInfo.mana;
        GameManager.Instance.alliedDeck.Remove(currentCard);
        GameManager.Instance.allyPlayedCards.Add(currentCard);
        var allydeck = GameManager.Instance.alliedDeck;
        UIManager.Instance.choosePanel.UpdateCards(allydeck[0], allydeck[1], allydeck[2], allydeck[3], allydeck[4]);
    }
}