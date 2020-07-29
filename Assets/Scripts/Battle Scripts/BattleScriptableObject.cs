using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Create Battle Object")]
public class BattleScriptableObject : ScriptableObject
{
    [Header("Battle Type")]
    public bool isBoss;
    public bool isRare;
    public bool isCommon;
    public bool isPlayer;

    [Header("Battle Information")]
    public string objectName;
    public string objectDescription;
    public Sprite objectSprite;
    public int currentHP, maxHP, currentMP, maxMP, strength, vitality, wpnPwr, armrPwr;
    public bool hasDied;

    [Header("Attack Moves")]
    public string[] movesAvailable;

    [Header("Battle Sprites")]
    public SpriteRenderer objectSpriteRenderer;
    public Sprite deadSprite, aliveSprite;
}
