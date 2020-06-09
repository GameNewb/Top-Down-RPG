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
