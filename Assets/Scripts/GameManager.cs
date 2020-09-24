using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager instance;

    public CharacterStats[] playerStats;

    // Control the player controls when any of this is active/true
    public bool gameMenuOpen;
    public bool dialogActive;
    public bool activeBattle;

    // Item variables
    public Item[] referenceItems;

    public List<InventorySlots> playerInventory = new List<InventorySlots>();
   
    private int nextEmptySlot;

    public int currentGil;
    public int maxGil;

    // Player references
    public BattleScriptableObject[] playerReferences;
    public Dictionary<string, BattleScriptableObject> playerScriptables = new Dictionary<string, BattleScriptableObject>();

    // Enemy references
    public BattleScriptableObject[] enemyReferences;
    public Dictionary<string, BattleScriptableObject> enemyScriptables = new Dictionary<string, BattleScriptableObject>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerController = PlayerController.instance;

        // Initialize ref items
        this.InitializeRefItems();

        // Initialize the data for player and enemy objects
        this.InitializePlayerObjects();
        this.InitializeEnemyObjects();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Player can only move if nothing is opened
        // TODO - Optimize
        if (gameMenuOpen || dialogActive || activeBattle)
        {
            playerController.canMove = false;
        }
        else
        {
            playerController.canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            this.SaveData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            this.LoadData();
        }
    }

    private void InitializeRefItems()
    {
        List<string> fullResourcePath = new List<string>();
        string resourcePath = Application.dataPath + "/Resources/Items";
        string[] directories = Directory.GetDirectories(resourcePath, "*", SearchOption.AllDirectories);
        
        // Loop through each Item folder/subfolder
        foreach (var folder in directories)
        {
            string itemSubfolder = folder.Substring(resourcePath.Length + 1);

            DirectoryInfo directory = new DirectoryInfo("Assets/Resources/Items/" + itemSubfolder);
            FileInfo[] itemFiles = directory.GetFiles("*.asset");

            // For each item in the subfolder, add it to the reference items list
            foreach (var item in itemFiles)
            {
                fullResourcePath.Add("Items/" + itemSubfolder + "/" + item.Name.Replace(".asset", ""));
            }
        }

        string[] itemResource = fullResourcePath.ToArray();
        referenceItems = new Item[itemResource.Length];

        for (var i = 0; i < itemResource.Length; i++) 
        {
            // Load each item
            referenceItems[i] = (Item)Resources.Load(itemResource[i]);
        }

    }

    // Get Item based on the item object that's passed
    public Item GetItemDetails(Item itemToGet)
    {
        // Get the item we're looking for
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if(referenceItems[i] == itemToGet)
            {
                return referenceItems[i];
            }
        }

        // No item found
        return null;
    }

    // Get Item based on the name of the item object that's passed
    public Item GetItemDetailsByName(string itemToGet)
    {
        // Get the item we're looking for
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].name == itemToGet)
            {
                return referenceItems[i];
            }
        }

        // No item found
        return null;
    }

    // Sorts the player inventory using two-point algorithm
    public void SortItems()
    {
        // Variable to keep track of what's the next empty slot
        int emptySlot = 0;

        // Iterate through each inventory slot that we have
        for (int i = 0; i < playerInventory.Count; i++)
        {
            // If we see an available item in the inventory, move it to an empty slot
            if (playerInventory[i].item != null)
            {
                playerInventory[emptySlot].item = playerInventory[i].item;
                playerInventory[emptySlot].amount = playerInventory[i].amount;
                emptySlot++;
            }
        }

        // Global variable that keeps track of our next empty slot
        nextEmptySlot = emptySlot;

        // Populate inventory with "empty" items
        // We start at the next empty slot, and just iterate through the remaining slots left
        for (int i = emptySlot; i < playerInventory.Count; i++)
        {
            // Reset the values
            playerInventory[i].item = null;
            playerInventory[i].amount = 0;
        }
    }

    public void AddItem(Item itemToAdd, int amount)
    {
        bool hasItem = false;

        // Sort Items first to easily distinguish the next empty slot
        this.SortItems();

        // Iterate through the current Player inventory to see if we have the item
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (playerInventory[i].item == itemToAdd)
            {
                playerInventory[i].amount += amount;
                hasItem = true;
                break;
            }
        }

        // If player doesn't have item, add it to next empty slot
        if (!hasItem)
        {
            playerInventory[nextEmptySlot].item = itemToAdd;
            playerInventory[nextEmptySlot].amount = amount;
        }
        
        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(Item itemToRemove, int amount)
    {
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (playerInventory[i].item == itemToRemove)
            {
                playerInventory[i].amount -= amount;
                
                // Remove the item from our inventory if the amount is less than 0
                if (playerInventory[i].amount <= 0)
                {
                    playerInventory[i].item = null;
                    playerInventory[i].amount = 0;
                }

                break;
            }
        }

        GameMenu.instance.ShowItems();
    }

    public void SaveData()
    {
        // Store scene data
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);

        // Store player position
        PlayerPrefs.SetFloat("Player_Position_X", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_Y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_Z", PlayerController.instance.transform.position.z);

        // Save Player stats
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Active", 0);
            }

            // Store data for each character object
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxLevel", playerStats[i].maxLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentEXP", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_StatPoints", playerStats[i].statPoints);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Vitality", playerStats[i].vitality);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Intelligence", playerStats[i].intelligence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Dexterity", playerStats[i].dexterity);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Luck", playerStats[i].luck);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WeaponPower", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmorPower", playerStats[i].armrPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_HasDied", playerStats[i].hasDied.ToString());

            // Check for nulls
            // Weapon
            if (playerStats[i].equippedWeapon != null)
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWeapon", playerStats[i].equippedWeapon.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWeapon", "");
            }

            // Armor
            if (playerStats[i].equippedArmor != null)
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmor", playerStats[i].equippedArmor.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmor", "");
            }

            // Shield / off-hand
            if (playerStats[i].equippedShield != null)
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedShield", playerStats[i].equippedShield.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedShield", "");
            }

            // Gloves
            if (playerStats[i].equippedGloves != null)
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedGloves", playerStats[i].equippedGloves.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedGloves", "");
            }

            // Boots
            if (playerStats[i].equippedBoots != null)
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedBoots", playerStats[i].equippedBoots.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedBoots", "");
            }

            // Left accessory
            if (playerStats[i].equippedAccessoryLeft != null)
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedAccessoryLeft", playerStats[i].equippedAccessoryLeft.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedAccessoryLeft", "");
            }

            // Right accessory
            if (playerStats[i].equippedAccessoryRight != null)
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedAccessoryRight", playerStats[i].equippedAccessoryRight.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedAccessoryRight", "");
            }
        }

        // Store Inventory data
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (playerInventory[i].item != null)
            {
                PlayerPrefs.SetString("ItemInInventory_" + i, playerInventory[i].item.name);
                PlayerPrefs.SetInt("ItemAmount_" + i, playerInventory[i].amount);
            }
            else
            {
                PlayerPrefs.SetString("ItemInInventory_" + i, "");
                PlayerPrefs.SetInt("ItemAmount_" + i, 0);
            }
        }
    }

    public void LoadData()
    {
        string activeScene = PlayerPrefs.GetString("Current_Scene");

        // Load scene
        StartCoroutine(LoadNewScene(activeScene));

        // Retrieve Player stat data
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            // Get data for each character object
            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].maxLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxLevel");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentEXP");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].statPoints = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_StatPoints");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].vitality = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Vitality");
            playerStats[i].intelligence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Intelligence");
            playerStats[i].dexterity = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Dexterity");
            playerStats[i].luck = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Luck");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WeaponPower");
            playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmorPower");
            
            // Null checks
            if (PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_HasDied") != null &&
                PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_HasDied") != "")
            {
                playerStats[i].hasDied = bool.Parse(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_HasDied"));
            }
            else
            {
                playerStats[i].hasDied = (playerStats[i].currentHP <= 0);
            }

            if (PlayerPrefs.HasKey("Player_" + playerStats[i].charName + "_EquippedWeapon"))
            {
                playerStats[i].equippedWeapon = this.GetItemDetailsByName(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWeapon"));
            }

            if (PlayerPrefs.HasKey("Player_" + playerStats[i].charName + "_EquippedArmor"))
            {
                playerStats[i].equippedArmor = this.GetItemDetailsByName(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmor"));
            }

            if (PlayerPrefs.HasKey("Player_" + playerStats[i].charName + "_EquippedShield"))
            {
                playerStats[i].equippedShield = this.GetItemDetailsByName(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedShield"));
            }

            if (PlayerPrefs.HasKey("Player_" + playerStats[i].charName + "_EquippedGloves"))
            {
                playerStats[i].equippedGloves = this.GetItemDetailsByName(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedGloves"));
            }
            
            if (PlayerPrefs.HasKey("Player_" + playerStats[i].charName + "_EquippedBoots"))
            {
                playerStats[i].equippedBoots = this.GetItemDetailsByName(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedBoots"));
            }

            if (PlayerPrefs.HasKey("Player_" + playerStats[i].charName + "_EquippedAccessoryLeft"))
            {
                playerStats[i].equippedAccessoryLeft = this.GetItemDetailsByName(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedAccessoryLeft"));
            }

            if (PlayerPrefs.HasKey("Player_" + playerStats[i].charName + "_EquippedAccessoryRight"))
            {
                playerStats[i].equippedAccessoryRight = this.GetItemDetailsByName(PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedAccessoryRight"));
            }
        }

        // Retrieve player saved inventory
        for (int i = 0; i < playerInventory.Count; i++)
        {
            playerInventory[i].item = this.GetItemDetailsByName(PlayerPrefs.GetString("ItemInInventory_" + i));
            playerInventory[i].amount = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }

    IEnumerator LoadNewScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        //UIFade.instance.FadeToBlack();
        PlayerController.instance.sceneTransitionFrom = "";
        playerController.isLoading = true;
        
        
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        //UIFade.instance.FadeFromBlack();
        asyncOperation.allowSceneActivation = true;
        
        playerController.isLoading = false;

        // Load previous player position
        var playerPosition = new Vector3(PlayerPrefs.GetFloat("Player_Position_X"), PlayerPrefs.GetFloat("Player_Position_Y"), PlayerPrefs.GetFloat("Player_Position_Z"));
        PlayerController.instance.transform.position = playerPosition;
    }

    private void InitializePlayerObjects()
    { 
        // Initialize the player dictionary for lookup purposes later
        for (int i = 0; i < playerReferences.Length; i++)
        {
            playerScriptables.Add(playerReferences[i].objectName, playerReferences[i]);
        }
    }

    private void InitializeEnemyObjects()
    {
        // Initialize the enemy dictionary for lookup purposes later
        for (int i = 0; i < enemyReferences.Length; i++)
        {
            enemyScriptables.Add(enemyReferences[i].objectName, enemyReferences[i]);
        }
    }

    public BattleScriptableObject FindScriptableObject(string name, bool isPlayer)
    {
        if (isPlayer)
        {
            return playerScriptables[name];
        }

        return enemyScriptables[name];
    }
}
