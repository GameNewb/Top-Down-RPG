using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectProperties : MonoBehaviour
{
    public Item itemToCreate;
    public BattleScriptableObject objectToCreate;

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

    public void Awake()
    {
        // Set the correct sprite for the item
        if (itemToCreate != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemToCreate.itemSprite;
        }

        if (objectToCreate != null)
        {
            GetComponent<SpriteRenderer>().sprite = objectToCreate.objectSprite;
        }
    }
}
