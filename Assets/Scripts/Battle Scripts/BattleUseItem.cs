using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUseItem : MonoBehaviour
{
    public int buttonIndex;

    public void UseItem()
    {
        // Target menu
        var parentMenu = gameObject.transform.parent.gameObject;

        // BattleManager instance
        var bmInstance = BattleManager.instance;

        // Set the selected player and grab the item to use
        var currentSelectedPlayer = GameManager.instance.playerStats[buttonIndex];
        Item itemToUse = bmInstance.itemToUse;

        // Add usage here for items
        if (itemToUse.name != null)
        {
            itemToUse.Use(currentSelectedPlayer);
            parentMenu.SetActive(false);
            bmInstance.itemMenu.SetActive(false);

            // For each individually created item button, delete after using an item so we don't get duplicates
            for (int i = 0; i < bmInstance.itemMenu.transform.childCount; i++)
            {
                Destroy(bmInstance.itemMenu.transform.GetChild(i).gameObject);
            }
        }

    }
    
}
