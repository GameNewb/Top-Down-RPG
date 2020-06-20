﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    public GameObject[] windows;
    public static GameMenu instance;

    private CharacterStats[] playerStats;
    private bool disableControls = false;

    [SerializeField] Text[] nameText, hpText, mpText, lvlText, expText;
    [SerializeField] Slider[] expSlider;
    [SerializeField] Image[] charImage;
    [SerializeField] GameObject[] charStatHolder;

    [SerializeField] GameObject[] statsButtons;

    [SerializeField] Text statsName, statsHP, statsMP, statsStr, statsDef, statsWpn, statsWpnPower, statsArmor, statsArmorPower, statsExp;
    [SerializeField] Image statsImage;

    [SerializeField] ItemButton[] itemButtons;

    [SerializeField] string selectedItem;
    [SerializeField] Item activeItem;
    [SerializeField] Text itemName, itemDescription, useButtonText;

    // Variables for the Discard panel
    [SerializeField] private GameObject discardPanel;
    private GameObject amountFieldObj;
    private InputField inputField;
    private int amountText;

    // Start is called before the first frame update
    void Start()
    {
        if (theMenu == null)
        {
            // Get child menu component
            theMenu = gameObject.transform.Find("Menu").gameObject;
        }

        // Set Listener for amount text field
        if (discardPanel != null)
        {
            var eventListener = new InputField.SubmitEvent();
            amountFieldObj = discardPanel.transform.GetChild(1).gameObject;
            inputField = amountFieldObj.GetComponent<InputField>();
            eventListener.AddListener(SetDiscardAmount);
            inputField.onEndEdit = eventListener;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape) && disableControls == false)
        {
            if (theMenu.activeInHierarchy)
            {
                CloseMenu();
            }
            else
            {
                ControlMenu(true);
            }
            
        } 
    }

    // Function to update the stats section of the menu
    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            // Activate / inactive each individual character stat section
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                // Update corresponding values
                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Lvl: " + playerStats[i].playerLevel;
                expText[i].text = playerStats[i].currentEXP.ToString() + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentEXP;
                charImage[i].sprite = playerStats[i].charImage;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }
    }

    // Control the menu when different actions are performed 
    public void ControlMenu(bool control)
    {
        theMenu.SetActive(control);
        UpdateMainStats();
        GameManager.instance.gameMenuOpen = control;
    }

    public void CloseMenu()
    {
        // Close any open menus
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        // Deactivate
        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
    }

    // Inactivate the menu when transitioning/loading between scenes
    // Called from SceneTransition
    public void DisableControl(bool control)
    {
        disableControls = control;
    }

    // Toggle between windows (Items, Stats, etc.)
    public void ToggleWindow(int windowNumber)
    {
        // If we're in the item or any other menu, update the stats automatically (e.g. using potion)
        UpdateMainStats();

        for(int i = 0; i < windows.Length; i++)
        {
            // Activate/inactivate the windows
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                // Close other windows
                windows[i].SetActive(false);
            }
        }
    }

    // Function to open each individual character stat page
    public void OpenStats()
    {
        // Update stats automatically when stats are opened
        UpdateMainStats();

        // Automatically populate with first character
        CharStats(0);

        // Update stats info
        for(int i = 0; i < statsButtons.Length; i++)
        {
            // Set the buttons based on active characters
            statsButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);

            // Update button text
            statsButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    // Function to update the Char Stat section of the menu
    public void CharStats(int selected)
    {
        // Update stats details based on character data
        statsName.text = playerStats[selected].charName;
        statsHP.text = playerStats[selected].currentHP.ToString() + "/" + playerStats[selected].maxHP;
        statsMP.text = playerStats[selected].currentMP.ToString() + "/" + playerStats[selected].maxMP;
        statsStr.text = playerStats[selected].strength.ToString();
        statsDef.text = playerStats[selected].vitality.ToString();

        if (playerStats[selected].equippedWeapon)
        {
            statsWpn.text = playerStats[selected].equippedWeapon.itemName;
        }
        statsWpnPower.text = playerStats[selected].wpnPwr.ToString();

        if (playerStats[selected].equippedWeapon)
        {
            statsArmor.text = playerStats[selected].armrPwr.ToString();
        }
        
        statsArmorPower.text = playerStats[selected].armrPwr.ToString();
        statsExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
        statsImage.sprite = playerStats[selected].charImage;
    }

    // Function to show inventory when "Items" button is clicked on the menu
    public void ShowItems()
    {
        GameManager.instance.SortItems();
        var playerInventory = GameManager.instance.playerInventory;

        for (int i = 0; i < playerInventory.Count; i++)
        {
            itemButtons[i].buttonValue = i;
            
            if (playerInventory[i].item != null)
            {
                // Activate the image, set appropriate sprite, and set the amount we have
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.playerInventory[i].item).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.playerInventory[i].amount.ToString();
            }
            else
            {
                // Inactivate if we don't have the item
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    // Function that updates which item is selected by the player in the inventory menu
    public void SelectItem(Item selectedItem)
    {
        activeItem = selectedItem;
        
        // Item is either an equippable item or consumable; update text
        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }
        else
        {
            useButtonText.text = "Use";
        }

        // Update the info
        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.itemDescription;
    }
    
    // Function to Discard an item from inventory
    public void DiscardItem()
    {
        // Discards an item
        if (activeItem != null)
        {
            var playerInventory = GameManager.instance.playerInventory;
            
            for (int i = 0; i < playerInventory.Count; i++)
            {
                if (activeItem == playerInventory[i].item)
                {
                    // If item amount is > 1, open a dialog box to allow player to drop X amount instead of 1
                    if (playerInventory[i].amount > 1)
                    {      
                        discardPanel.SetActive(true);
                    } 
                    else
                    {
                        // Remove 1 item amount from inventory
                        GameManager.instance.RemoveItem(activeItem, 1);
                    }
                }
            }
        }
    }
    
    private void SetDiscardAmount(string amount)
    {
        if (amount != "")
        {
            amountText = int.Parse(amount);
        }
    }

    // Function to display the discard panel for specifying amount to discard
    public void ConfirmDiscardAmount()
    {
        // Remove the item, reset all values, close the panel
        GameManager.instance.RemoveItem(activeItem, amountText);
        activeItem = null;
        itemName.text = null;
        itemDescription.text = null;
        this.CloseDiscardPanel();
    }

    // Closes the Discard Panel
    public void CloseDiscardPanel()
    {
        // Revert to placeholder values - set to 2 since "Input Caret" is the 1st child
        //var amountProperties = inputField.transform.GetChild(2).gameObject.GetComponent<Text>();
        //amountText = 1;
        //amountProperties.text = amountText.ToString();
        //Debug.Log(amountProperties.text);
        discardPanel.SetActive(false);
    }
   
}

/* OLD CODE */
/*
 *  // Function to show inventory when "Items" button is clicked on the menu
    // uses itemsHeld variable
    public void ShowItems()
    {
        GameManager.instance.SortItems();

        // Set the items section number in the grid
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            // Item is held / in player
            if (GameManager.instance.itemsHeld[i])
            {
                var itemHeld = GameManager.instance.itemsHeld[i];

                // Activate the image, set appropriate sprite, and set the amount we have
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                // Inactivate if we don't have the item
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }
 * 
 * 
 * 
 * 
 * 
 * 
 */
