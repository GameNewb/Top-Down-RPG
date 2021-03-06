﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    
    [SerializeField] Text hpStat, mpStat, expStat, strStat, vitStat, intStat, dexStat, luckStat, wpnStat, offHandStat, armrStat, gloveStat, shoesStat, accessory1Stat, accessory2Stat;
    [SerializeField] Image statsImage;
    [SerializeField] Image wpnImage, offHandImage, armorImage, gloveImage, shoesImage, accessory1Image, accessory2Image;

    [SerializeField] ItemButton[] itemButtons;

    [SerializeField] string selectedItem;
    [SerializeField] Item activeItem;
    [SerializeField] Text itemName, itemDescription, useButtonText;
    [SerializeField] GameObject actionPanel;
    [SerializeField] GameObject accessoryPanel;

    // Variables for the Discard panel
    [SerializeField] private GameObject discardPanel;
    private GameObject amountFieldObj;
    private InputField inputField;
    private int amountText;

    // Variables for using an item on a specific character
    public GameObject itemToUseOnMenu;
    public Text[] itemToUseOnNames;

    public Text gilText;
    
    private InventoryHelper inventoryHelper;
    private int accessorySlot = 1;
    private int selectedChar = 1;
    
    // Variables to clean the image
    private Color transparentColor = new Color(0f, 0f, 0f, 0f);
    private Color opaqueColor = new Color(1f, 1f, 1f, 1f);

    // Variables for Stat management
    [Header("Stat Increase/Decrease")]
    [SerializeField] private GameObject strengthObjStat;
    [SerializeField] private GameObject vitalityObjStat;
    [SerializeField] private GameObject intelligenceObjStat;
    [SerializeField] private GameObject dexterityObjStat;
    [SerializeField] private GameObject luckObjStat;
    [SerializeField] private GameObject[] statObjects;
    [SerializeField] private Text statPointText;


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

        inventoryHelper = new InventoryHelper();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape) && disableControls == false 
            && !GameManager.instance.dialogActive 
            && !GameManager.instance.activeBattle)
        {
            if (theMenu.activeInHierarchy)
            {
                CloseMenu();
            }
            else
            {
                ControlMenu(true);
            }

            AudioManager.instance.PlaySFX(5);
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

        gilText.text = GameManager.instance.currentGil.ToString() + "g";
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
        itemToUseOnMenu.SetActive(false);
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

        itemToUseOnMenu.SetActive(false);
    }

    // Function to open each individual character stat page
    public void OpenStats()
    {
        // Update stats automatically when stats are opened
        UpdateMainStats();

        // Automatically populate with first character
        this.SetStatValues(0);

        // Update stats info
        for (int i = 0; i < statsButtons.Length; i++)
        {
            // Set the buttons based on active characters
            statsButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);

            // Update button text
            statsButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    // Function to update the Char Stat section of the menu
    public void SetStatValues(int selected)
    {
        // Set HP/MP/EXP
        hpStat.text = playerStats[selected].currentHP.ToString() + "/" + playerStats[selected].maxHP;
        mpStat.text = playerStats[selected].currentMP.ToString() + "/" + playerStats[selected].maxMP;
        expStat.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();

        // Set image
        statsImage.sprite = playerStats[selected].charImage;

        // Update stats details based on character data
        strStat.text = playerStats[selected].strength.ToString();
        vitStat.text = playerStats[selected].vitality.ToString();
        intStat.text = playerStats[selected].intelligence.ToString();
        dexStat.text = playerStats[selected].dexterity.ToString();
        luckStat.text = playerStats[selected].luck.ToString();

        // Set Equipped Weapon Text
        if (playerStats[selected].equippedWeapon)
        {
            wpnStat.text = playerStats[selected].equippedWeapon.itemName;
            wpnImage.sprite = playerStats[selected].equippedWeapon.itemSprite;
            wpnImage.color = opaqueColor;
        }
        else
        {
            wpnStat.text = "None";
            wpnImage.sprite = null;
            wpnImage.color = transparentColor;
        }

        // Set Equipped Armor Text
        if (playerStats[selected].equippedArmor)
        {
            armrStat.text = playerStats[selected].equippedArmor.itemName;
            armorImage.sprite = playerStats[selected].equippedArmor.itemSprite;
            armorImage.color = opaqueColor;
        }
        else
        {
            armrStat.text = "None";
            armorImage.sprite = null;
            armorImage.color = transparentColor;
        }

        // Set Equipped Shield Text
        if (playerStats[selected].equippedShield)
        {
            offHandStat.text = playerStats[selected].equippedShield.itemName;
            offHandImage.sprite = playerStats[selected].equippedShield.itemSprite;
            offHandImage.color = opaqueColor;
        }
        else
        {
            offHandStat.text = "None";
            offHandImage.sprite = null;
            offHandImage.color = transparentColor;
        }

        // Set Equipped Gloves Text
        if (playerStats[selected].equippedGloves)
        {
            gloveStat.text = playerStats[selected].equippedGloves.itemName;
            gloveImage.sprite = playerStats[selected].equippedGloves.itemSprite;
            gloveImage.color = opaqueColor;
        }
        else
        {
            gloveStat.text = "None";
            gloveImage.sprite = null;
            gloveImage.color = transparentColor;
        }

        // Set Equipped Shoes Text
        if (playerStats[selected].equippedBoots)
        {
            shoesStat.text = playerStats[selected].equippedBoots.itemName;
            shoesImage.sprite = playerStats[selected].equippedBoots.itemSprite;
            shoesImage.color = opaqueColor;
        }
        else
        {
            shoesStat.text = "None";
            shoesImage.sprite = null;
            shoesImage.color = transparentColor;
        }

        // Set Equipped Shoes Text
        if (playerStats[selected].equippedAccessoryLeft)
        {
            accessory1Image.sprite = playerStats[selected].equippedAccessoryLeft.itemSprite;
            accessory1Image.color = opaqueColor;
        }
        else
        {
            accessory1Image.sprite = null;
            accessory1Image.color = transparentColor;
        }

        // Set Equipped Shoes Text
        if (playerStats[selected].equippedAccessoryRight)
        {
            accessory2Image.sprite = playerStats[selected].equippedAccessoryRight.itemSprite;
            accessory2Image.color = opaqueColor;
        }
        else
        {
            accessory2Image.sprite = null;
            accessory2Image.color = transparentColor;
        }

        // Set the stats +/- buttons
        strengthObjStat.GetComponent<StatManager>().selectedCharacter = selected;
        vitalityObjStat.GetComponent<StatManager>().selectedCharacter = selected;
        intelligenceObjStat.GetComponent<StatManager>().selectedCharacter = selected;
        dexterityObjStat.GetComponent<StatManager>().selectedCharacter = selected;
        luckObjStat.GetComponent<StatManager>().selectedCharacter = selected;

        this.UpdateDetailedStat(selected);
    }

    // Function to update the stat page
    public void UpdateDetailedStat(int selected)
    {
        // Update stats details based on character data
        strStat.text = playerStats[selected].strength.ToString();
        vitStat.text = playerStats[selected].vitality.ToString();
        intStat.text = playerStats[selected].intelligence.ToString();
        dexStat.text = playerStats[selected].dexterity.ToString();
        luckStat.text = playerStats[selected].luck.ToString();
        statPointText.text = playerStats[selected].statPoints.ToString();

        // Reactivate all + button when player has statpoints
        if (playerStats[selected].statPoints > 0)
        {
            for (int i = 0; i < statObjects.Length; i++)
            {
                statObjects[i].transform.GetChild(1).GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            for (int i = 0; i < statObjects.Length; i++)
            {
                statObjects[i].transform.GetChild(1).GetComponent<Button>().interactable = false;
            }
        }

        // Reactivate subtract button when opening a new Character page
        statObjects[0].transform.GetChild(0).GetComponent<Button>().interactable = playerStats[selected].strength > 0 ? true : false;
        statObjects[1].transform.GetChild(0).GetComponent<Button>().interactable = playerStats[selected].vitality > 0 ? true : false;
        statObjects[2].transform.GetChild(0).GetComponent<Button>().interactable = playerStats[selected].intelligence > 0 ? true : false;
        statObjects[3].transform.GetChild(0).GetComponent<Button>().interactable = playerStats[selected].dexterity > 0 ? true : false;
        statObjects[4].transform.GetChild(0).GetComponent<Button>().interactable = playerStats[selected].luck > 0 ? true : false;
    }

    // Function to show inventory when "Items" button is clicked on the menu
    public void ShowItems()
    {
        GameManager.instance.SortItems();
        var playerInventory = GameManager.instance.playerInventory;

        // Display all the items that the player has
        inventoryHelper.PopulateInventoryButtons(playerInventory, itemButtons);
    }

    // Function that updates which item is selected by the player in the inventory menu
    public void SelectItem(Item selectedItem)
    {
        // Reset values when selecting an empty slot in the inventory
        if (selectedItem == null)
        {
            activeItem = null;
            actionPanel.SetActive(false);

            itemName.text = "";
            itemDescription.text = "";
        }
        else
        {
            activeItem = selectedItem;

            // If action panel has been disable, re-enable it
            if (!actionPanel.activeInHierarchy)
            {
                actionPanel.SetActive(true);
            }

            inventoryHelper.SelectItem(activeItem, useButtonText, itemName, itemDescription);
        }
        
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
                        this.CloseItemToUseOnChoice();
                    }
                    break;
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

        this.ClosePanel(discardPanel);
        this.CloseItemToUseOnChoice();
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

    // Function to close any panel object
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    // Function to set the accessory slot
    public void SetAccessorySlot(int slot)
    {
        accessorySlot = slot;
        activeItem.Use(playerStats[selectedChar], accessorySlot);

        this.CloseItemToUseOnChoice();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    // Function to take the player to the main menu
    public void QuitGame()
    {
        SceneManager.LoadScene("Main Menu");

        // Destroy appropriate objects
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
   
    // Function to open the dialog for consumable items
    public void OpenItemToUseOnChoice()
    {
        itemToUseOnMenu.SetActive(true);

        for (int i = 0; i < itemToUseOnNames.Length; i++)
        {
            var playerStats = GameManager.instance.playerStats[i];

            // Set button text
            itemToUseOnNames[i].text = playerStats.charName;
            itemToUseOnNames[i].transform.parent.gameObject.SetActive(playerStats.gameObject.activeInHierarchy);

            // Disable the Character selection if item is for revival and character is still alive
            if (activeItem.revivalItem && !playerStats.hasDied)
            {
                itemToUseOnNames[i].transform.parent.gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                itemToUseOnNames[i].transform.parent.gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }
    
    // Function to close the dialog for consumable items
    public void CloseItemToUseOnChoice()
    {
        itemToUseOnMenu.SetActive(false);
        accessoryPanel.SetActive(false);
        actionPanel.SetActive(false);
        
        // Fix for items being selectable even after use - leads to equipment duplication
        activeItem = null;
        selectedItem = "";

        itemName.text = "";
        itemDescription.text = "";
    }

    public void UseItem(int characterSelected)
    {
        if (activeItem)
        {
            // If accessory, allow player to select which slot
            if (activeItem.isAccessory)
            {
                selectedChar = characterSelected;
                accessoryPanel.SetActive(true);
            }
            else
            {
                activeItem.Use(playerStats[characterSelected], accessorySlot);

                this.CloseItemToUseOnChoice();
            }
        }
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }
}