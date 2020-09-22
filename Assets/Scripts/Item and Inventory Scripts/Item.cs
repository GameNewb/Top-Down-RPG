using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Items/Create Item")]
public class Item : ScriptableObject
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;
    public bool isRangedWeapon;
    public bool isOffhand;
    public bool isGloves;
    public bool isBoots;
    public bool isAccessory;

    [Header("Item Information")]
    public string itemName;
    public string itemDescription;
    public int value;
    public Sprite itemSprite;

    [Header("Item Drop Properties")]
    public float itemDropRate;
    public int amountToDrop;

    [Header("Item Effects")]
    // Used for adding or subtracting to values
    public int amountToChange;
    public bool affectHP, affectMP, affectStr, revivalItem;

    [Header("Equippable Stats")]
    public int weaponStrength;
    [Tooltip("Crit Multiplier that is multiplied to the actual damage")] public float weaponCritMultiplier;
    [Tooltip("Crit Chance in Percentage")] public float weaponCritChance;
    public int armorStrength;

    public void Use(CharacterStats charToUseOn)
    {
        CharacterStats selectedChar = charToUseOn;
        
        // If it's a consumable, check which one it affects
        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.currentHP += amountToChange;
                if (selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if (affectMP)
            {
                selectedChar.currentMP += amountToChange;
                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if (affectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }

        // Weapon equipment
        if (isWeapon)
        {
            if (selectedChar.equippedWeapon != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedWeapon, 1);
            }

            selectedChar.equippedWeapon = this;
            selectedChar.wpnPwr = weaponStrength;
        }

        // Armor equipment
        if (isArmor)
        {
            if (selectedChar.equippedArmor != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedArmor, 1);

            }

            selectedChar.equippedArmor = this;
            selectedChar.armrPwr = armorStrength;
        }

        // Off-hand / shield equipment
        // TODO - refactor bow equipment
        if (isOffhand)
        {
            if (selectedChar.equippedShield != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedShield, 1);
                selectedChar.armrPwr -= selectedChar.equippedShield.armorStrength;
            }

            selectedChar.equippedShield = this;
            selectedChar.armrPwr += armorStrength;
        }

        // Gloves equipment
        if (isGloves)
        {
            if (selectedChar.equippedGloves != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedGloves, 1);
                selectedChar.armrPwr -= selectedChar.equippedGloves.armorStrength;
            }

            selectedChar.equippedGloves = this;
            selectedChar.armrPwr += armorStrength;
        }

        // Boots equipment
        if (isBoots)
        {
            if (selectedChar.equippedBoots != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedBoots, 1);
                selectedChar.armrPwr -= selectedChar.equippedBoots.armorStrength;
            }

            selectedChar.equippedBoots = this;
            selectedChar.armrPwr += armorStrength;
        }

        // Accessory equipment
        if (isAccessory)
        {
            // TODO - Accessory swapping
            if (selectedChar.equippedAccessoryLeft != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedAccessoryLeft, 1);

            }

            selectedChar.equippedAccessoryLeft = this;
            selectedChar.armrPwr += armorStrength;
        }

        GameManager.instance.RemoveItem(this, 1);
    }

    public void UseInBattle(GameObject charToUseOn)
    {
        // If it's a consumable, check which one it affects
        if (isItem)
        {
            var characterScriptable = charToUseOn.GetComponent<ScriptableObjectProperties>();

            if (affectHP)
            {
                // Revive player 
                if (characterScriptable.hasDied && characterScriptable.currentHP <= 0 && revivalItem)
                {
                    characterScriptable.hasDied = false;
                    characterScriptable.objectSpriteRenderer.sprite = characterScriptable.aliveSprite;
                }
                
                characterScriptable.currentHP += amountToChange;

                if (characterScriptable.currentHP > characterScriptable.maxHP)
                {
                    characterScriptable.currentHP = characterScriptable.maxHP;
                }
            }

            if (affectMP)
            {
                characterScriptable.currentMP += amountToChange;

                if (characterScriptable.currentMP > characterScriptable.maxMP)
                {
                    characterScriptable.currentMP = characterScriptable.maxMP;
                }
            }

        }
        
        GameManager.instance.RemoveItem(this, 1);
    }
}
