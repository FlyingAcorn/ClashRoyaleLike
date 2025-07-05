using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "ScriptableObj/CardInfo")]
public class CardScriptableObject : ScriptableObject
{
    public Card prefab;
    public Sprite image;
    public PreviewModel previewModel;
    public int mana;
    public bool canInvade;
    public bool hasRanged;
}