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

    [Header("Battle Sprites")]
    public SpriteRenderer objectSpriteRenderer;
    public Sprite deadSprite, aliveSprite;

    [Header("Battle Death Properties")]
    private bool shouldFade;
    public float fadeSpeed = 1f;

    [Header("Item Drop Properties")]
    public List<Item> itemsToDrop;
    public int gilDropAmount;

    [Header("EXP Properties")]
    public int expPoints;

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

    public void Update()
    {
        if (shouldFade && gameObject.activeInHierarchy)
        {
            objectSpriteRenderer.color = new Color(Mathf.MoveTowards(objectSpriteRenderer.color.r, 1f, fadeSpeed * Time.deltaTime),
                                                   Mathf.MoveTowards(objectSpriteRenderer.color.g, 0f, fadeSpeed * Time.deltaTime),
                                                   Mathf.MoveTowards(objectSpriteRenderer.color.b, 0f, fadeSpeed * Time.deltaTime),
                                                   Mathf.MoveTowards(objectSpriteRenderer.color.a, 0f, fadeSpeed * Time.deltaTime));

            // Disable object after fading
            if (objectSpriteRenderer.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Fade()
    {
        shouldFade = true;
    }
}
