using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
