using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Enemy/Create Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Enemy Type")]
    public bool isBoss;
    public bool isRare;
    public bool isCommon;

    [Header("Enemy Information")]
    public string enemyName;
    public string enemyDescription;
    public Sprite enemySprite;
    public int currentHP, maxHP, currentMP, maxMP, strength, vitality, wpnPwr, armrPwr;
    public bool hasDied;

    [Header("Attack Moves")]
    public string[] movesAvailable;

}
