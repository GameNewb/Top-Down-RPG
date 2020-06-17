using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlots
{
    public Item item;
    public int amount;

    public InventorySlots() { }

    // Constructor
    public InventorySlots(Item itemName, int itemAmount)
    {
        item = itemName;
        amount = itemAmount;
    }
}
