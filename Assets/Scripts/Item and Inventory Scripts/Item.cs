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

    [Header("Item Information")]
    public string itemName;
    public string itemDescription;
    public int value;
    public Sprite itemSprite;

    [Header("Item Effects")]
    // Used for adding or subtracting to values
    public int amountToChange;
    public bool affectHP, affectMP, affectStr;

    [Header("Equippable Stats")]
    public int weaponStrength;
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

        if (isWeapon)
        {
            if (selectedChar.equippedWeapon != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedWeapon, 1);
            }

            selectedChar.equippedWeapon = this;
            selectedChar.wpnPwr = weaponStrength;
        }

        if (isArmor)
        {
            if (selectedChar.equippedArmor != null)
            {
                GameManager.instance.AddItem(selectedChar.equippedArmor, 1);

            }

            selectedChar.equippedArmor = this;
            selectedChar.armrPwr = armorStrength;
        }

        GameManager.instance.RemoveItem(this, 1);
    }
}
