using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHelper
{
    public void PopulateInventoryButtons(List<InventorySlots> inventory, ItemButton[] itemButtons)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            itemButtons[i].buttonValue = i;

            if (inventory[i].item != null)
            {
                // Activate the image, set appropriate sprite, and set the amount we have
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                if (GameManager.instance.GetItemDetails(inventory[i].item) != null)
                {
                    itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(inventory[i].item).itemSprite;
                }
                itemButtons[i].amountText.text = inventory[i].amount.ToString();
            }
            else
            {
                // Inactivate if we don't have the item
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item activeItem, Text useButton, Text itemName, Text itemDescription)
    {
        // Item is either an equippable item or consumable; update text
        if (activeItem.isItem)
        {
            useButton.text = "Use";
        }
        else
        {
            useButton.text = "Equip";
        }

        // Update the info
        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.itemDescription;
    }

    public Item GetItemDetails(Item itemToGet)
    {
        var referenceItems = GameManager.instance.referenceItems;

        // Get the item we're looking for
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i] == itemToGet)
            {
                return referenceItems[i];
            }
        }

        // No item found
        return null;
    }
}
