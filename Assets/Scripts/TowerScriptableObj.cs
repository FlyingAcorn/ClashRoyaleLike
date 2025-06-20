using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Tower",menuName = "ScriptableObj/Tower")]
public class TowerScriptableObj : ScriptableObject
{
    public int health;
    public int damage;
    public int attackSpeed;
}
