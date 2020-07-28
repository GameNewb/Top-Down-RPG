using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUseItem : MonoBehaviour
{
    public GameObject playerPosition;
    public int buttonIndex;

    private GameObject parentMenu;
    
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
            // Pass in the player object
            itemToUse.UseInBattle(playerPosition.transform.GetChild(0).gameObject);
            bmInstance.UpdateUIStats();
            parentMenu.SetActive(false);
            bmInstance.itemMenu.SetActive(false);

            // For each individually created item button
            for (int i = 0; i < bmInstance.itemMenu.transform.childCount; i++)
            {
                // Destroy after using the item so we don't get duplicates
                Destroy(bmInstance.itemMenu.transform.GetChild(i).gameObject);
            }

            // Do next turn
            bmInstance.NextTurn();
        }

    }
    
}
