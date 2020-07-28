﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemButton : MonoBehaviour
{
    public GameObject targetMenu;

    public Button[] characterNameButtons;

    public void Press()
    {
        // Activate menu to use item on
        targetMenu.SetActive(true);
        
        // Retrieve the item details and set it to be used for the character
        Item item = GameManager.instance.GetItemDetailsByName(gameObject.name.ToString());
        BattleManager.instance.itemToUse = item;

        for (int i = 0; i < characterNameButtons.Length; i++)
        {
            // Only show UI for active characters
            var playerStats = GameManager.instance.playerStats[i];
            if (playerStats && playerStats.gameObject.activeInHierarchy)
            {
                characterNameButtons[i].gameObject.SetActive(true);
                characterNameButtons[i].GetComponentInChildren<Text>().text = playerStats.charName.ToString();
            }
            else
            {
                characterNameButtons[i].gameObject.SetActive(false);
            }
        }
    }
}