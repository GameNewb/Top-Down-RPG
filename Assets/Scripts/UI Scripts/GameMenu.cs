using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        if (theMenu == null)
        {
            // Get child menu component
            theMenu = gameObject.transform.Find("Menu").gameObject;
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

        if (playerStats[selected].equippedWeapon != "")
        {
            statsWpn.text = playerStats[selected].equippedWeapon;
        }
        statsWpnPower.text = playerStats[selected].wpnPwr.ToString();

        if (playerStats[selected].equippedWeapon != "")
        {
            statsArmor.text = playerStats[selected].armrPwr.ToString();
        }
        
        statsArmorPower.text = playerStats[selected].armrPwr.ToString();
        statsExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
        statsImage.sprite = playerStats[selected].charImage;
    }

    public void ShowItems()
    {
        // Set the items section number in the grid
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            // Item is held / in player
            if (GameManager.instance.itemsHeld[i] != "")
            {
                var itemHeld = GameManager.instance.itemsHeld[i];

                // Activate the image, set appropriate sprite, and set the amount we have
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemHeld).itemSprite;
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

    public void CloseMenu()
    {
        // Close any open menus
        for (int i = 0; i < windows.Length; i++) {
            windows[i].SetActive(false);
        }

        // Deactivate
        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
    }
}
