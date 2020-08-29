using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private BattleManagerHelper bmHelper = new BattleManagerHelper();
    public static BattleManager instance;

    public bool activeBattle;

    public GameObject battleScene;

    [Header("Object Prefabs")]
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleData[] playerPrefabs;
    public BattleData[] enemyPrefabs;
    public GameObject objectScriptablePrefab;

    [Header("Active Battlers")]
    public List<GameObject> activeCombatants = new List<GameObject>();

    public int currentTurn;
    public bool waitingForATurn;

    // UI for player actions
    public GameObject uiButtonsHolder;

    // Attack sets and effect
    [Header("Movesets")]
    public BattleMoveset[] movesets;
    public GameObject enemyParticleEffect;

    public BattleDamageNumber damageNumberEffect;

    [Header("Player Stats and Text")]
    public Text[] playerName, playerHP, playerMP;

    // UI for target buttons
    [Header("Target Menu")]
    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    // UI for menu buttons
    [Header("Magic Menu")]
    public GameObject magicMenu;
    public BattleMagicSelection[] magicButtons;

    // UI for item buttons and reference to the Item to be used
    [Header("Item Menu")]
    public GameObject itemMenu;
    public GameObject itemButtonToInstantiate;
    public GameObject targetUseMenu;
    public Item itemToUse;

    // UI for item buttons and reference to the Item to be used
    [Header("Flee Menu")]
    public GameObject fleeButton;

    [Header("Battle Notification")]
    public BattleNotification battleNotice;

    public int chanceToFlee = 35;
    private bool fleeing;

    public string gameOverScene;

    private int expReward;
    private int gilReward;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeBattle)
        {
            if (waitingForATurn)
            {  
                // Activate the button for players
                if (activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>().isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                    
                    // Close any open menu
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        targetMenu.SetActive(false);
                        itemMenu.SetActive(false);
                        magicMenu.SetActive(false);
                        targetUseMenu.SetActive(false);

                        // For each individually created item button
                        for (int i = 0; i < itemMenu.transform.childCount; i++)
                        {
                            // Destroy after pressing esc so we don't get duplicates
                            Destroy(itemMenu.transform.GetChild(i).gameObject);
                        }
                    }
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    // Enemy attack
                    StartCoroutine(EnemyMove());
                }
            }
        }
    }

    public void StartBattle (BattleScriptableObject[] enemiesToSpawn)
    {
        GameManager.instance.activeBattle = true;
        bool bossBattle = false;

        // Check first if we're not in a battle already
        if (!activeBattle)
        {
            activeBattle = true;

            GameManager.instance.activeBattle = true;

            // Set the appropriate BG position
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            // Activate and load each player character into the battle scene
            for (int i = 0; i < playerPositions.Length; i++)
            {
                var playerStats = GameManager.instance.playerStats[i];
                if (playerStats && playerStats.gameObject.activeInHierarchy)
                {
                    if (GameManager.instance.playerScriptables.ContainsKey(playerStats.charName))
                    {
                        BattleScriptableObject character = GameManager.instance.playerScriptables[playerStats.charName];

                        // Create the player character battle object
                        // Allow movement of player objects
                        GameObject newPlayer = Instantiate(objectScriptablePrefab, playerPositions[i].position, playerPositions[i].rotation);
                        newPlayer.transform.parent = playerPositions[i];

                        bmHelper.InitializeBattleData(newPlayer, true, character, playerStats);
                    }
                }
            }

            // Load enemies
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != null && enemiesToSpawn[i].objectName != "")
                {
                    if (GameManager.instance.enemyScriptables.ContainsKey(enemiesToSpawn[i].objectName))
                    {
                        // Instantiate a new enemy prefab using the SO
                        GameObject newEnemy = Instantiate(objectScriptablePrefab, enemyPositions[i].position, enemyPositions[i].rotation);
                        newEnemy.transform.parent = enemyPositions[i];

                        bmHelper.InitializeBattleData(newEnemy, false, enemiesToSpawn[i], null);
                        
                        if (enemiesToSpawn[i].isBoss)
                        {
                            bossBattle = true;
                        }
                    }

                }
            }

            // If boss fight, disable Flee button
            if (bossBattle)
            { 
                fleeButton.GetComponent<Button>().interactable = false;
            }

            waitingForATurn = true;
            currentTurn = Random.Range(0, activeCombatants.Count);

            this.UpdateUIStats();
        }
    }

    // Function to change turns
    public void NextTurn()
    {
        currentTurn++;

        if (currentTurn >= activeCombatants.Count)
            currentTurn = 0;

        waitingForATurn = true;

        this.UpdateBattle();
        this.UpdateUIStats();
    }

    // Function to process the battle data after each turn
    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeCombatants.Count; i++)
        {
            // TODO: Merge into one scriptable object, refactor this mess
            // Set HP to 0 if they received more damage than max hp
            if (activeCombatants[i].GetComponent<ScriptableObjectProperties>().currentHP <= 0)
            {
                activeCombatants[i].GetComponent<ScriptableObjectProperties>().currentHP = 0;

                // Handle dead battler
                if (!activeCombatants[i].GetComponent<ScriptableObjectProperties>().isPlayer)
                {
                    activeCombatants[i].GetComponent<ScriptableObjectProperties>().Fade();
                }
            }
            else
            {
                if (activeCombatants[i].GetComponent<ScriptableObjectProperties>().isPlayer)
                {
                    allPlayersDead = false;

                    // Set to idle animation if player has been revived
                    if (activeCombatants[i].GetComponent<Animator>().runtimeAnimatorController && activeCombatants[i].GetComponent<Animator>().GetBool("isDead"))
                    {
                        activeCombatants[i].GetComponent<Animator>().SetBool("isDead", false);
                    }
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        // Battle complete or Game Over
        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                // End battle in victory
                StartCoroutine(EndBattle());
            }
            else
            {
                // Game over
                StartCoroutine(GameOver());
            }
        } 
        else
        {
            // If the current combatant is already dead, skip their turn
            // Iteratively loop through dead characters to increment current turn
            while (activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>().hasDied)
            {
                currentTurn++;
                
                if (currentTurn >= activeCombatants.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    // Function to update the player stats UI
    public void UpdateUIStats()
    {
        // Iterate through each text element
        for (int i = 0; i < playerName.Length; i++)
        {
            if (activeCombatants.Count > i)
            {
                var playerObject = activeCombatants[i].GetComponent<ScriptableObjectProperties>();

                if (playerObject.isPlayer)
                {
                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerObject.objectName;
                    playerHP[i].text = Mathf.Clamp(playerObject.currentHP, 0, int.MaxValue) + "/" + playerObject.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerObject.currentMP, 0, int.MaxValue) + "/" + playerObject.maxMP;
                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    // Function to allow enemies to move/do attack
    public IEnumerator EnemyMove()
    {
        waitingForATurn = false;

        yield return new WaitForSeconds(1f);

        this.EnemyAttack();

        yield return new WaitForSeconds(1f);

        this.NextTurn();
    }

    // Function that controls enemy attack
    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        int selectedTarget = 0;

        for (int i = 0; i < activeCombatants.Count; i++)
        {
            var combatant = activeCombatants[i].GetComponent<ScriptableObjectProperties>();
            if (combatant.isPlayer && combatant.currentHP > 0)
            {
                // Add player to enemy targets
                players.Add(i);
            } 
        }
        
        if (players.Count > 0)
        {
            selectedTarget = players[Random.Range(0, players.Count)];
        }
        
        // TODO: Refactor
        // Find the correct attack object
        int selectAttack = Random.Range(0, activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>().movesAvailable.Length);
        int movesetPower = 0;

        for (int i = 0; i < movesets.Length; i++)
        {
            if (movesets[i].movesetName == activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>().movesAvailable[selectAttack])
            {
                Instantiate(movesets[i].movesetEffect, activeCombatants[selectedTarget].transform.position, activeCombatants[selectedTarget].transform.rotation);
                movesetPower = movesets[i].movesetDamage;
            }
        }

        // Instantiate the particle effect when enemy attacks
        Instantiate(enemyParticleEffect, activeCombatants[currentTurn].transform.position, activeCombatants[currentTurn].transform.rotation);

        // Calculate the damage
        this.DealDamage(selectedTarget, movesetPower, "Slash");
    }

    // Function to control players attack
    public void PlayerAttack(string moveName, int selectedTarget)
    {
        int movesetPower = 0;

        for (int i = 0; i < movesets.Length; i++)
        {
            if (movesets[i].movesetName == moveName)
            {
                Instantiate(movesets[i].movesetEffect, activeCombatants[selectedTarget].transform.position, activeCombatants[selectedTarget].transform.rotation);
                movesetPower = movesets[i].movesetDamage;
            }
        }

        // TODO: Change effect for player
        Instantiate(enemyParticleEffect, activeCombatants[currentTurn].transform.position, activeCombatants[currentTurn].transform.rotation);

        this.DealDamage(selectedTarget, movesetPower, moveName);

        // Prevent player from clicking the buttons multiple times
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        this.NextTurn();
    }

    // Function to deal damage to objects
    public void DealDamage(int target, int movesetPower, string moveName)
    {
        var currentUser = activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>();
        var targetUser = activeCombatants[target].GetComponent<ScriptableObjectProperties>();
        float atkPower = currentUser.strength + currentUser.wpnPwr;
        float vitPower = targetUser.vitality + targetUser.armrPwr;

        // Move to target if it's a physical/melee attack
        if (moveName == "Slash")
        {
            StartCoroutine(MoveTarget(currentTurn, target));
        }

        // Set Animation and Position change if animation exists
        if (activeCombatants[currentTurn].GetComponent<Animator>().runtimeAnimatorController)
        {
            StartCoroutine(PlayAttackAnimation(currentTurn, moveName));
        }

        // Calculate the damage to take
        float damageCalculation = (atkPower / vitPower) * movesetPower * Random.Range(.9f, 1.1f);
        int damageToTake = Mathf.RoundToInt(damageCalculation);
        
        targetUser.currentHP -= damageToTake;

        // Set status
        if (targetUser.currentHP <= 0)
        {
            targetUser.hasDied = true;

            // Set dead sprite if player character dies
            if (targetUser.isPlayer)
            {
                targetUser.objectSpriteRenderer.sprite = targetUser.deadSprite;
                activeCombatants[target].GetComponent<Animator>().SetBool("isDead", true);
            }
        }

        // Play damaged animation
        if (activeCombatants[target].GetComponent<Animator>().runtimeAnimatorController)
        {
            StartCoroutine(PlayDamagedAnimation(target));
        }

        // Instantiate the damage numbers on screen
        Instantiate(damageNumberEffect, activeCombatants[target].transform.position, activeCombatants[target].transform.rotation).SetDamage(damageToTake);

        // Update player UI stats
        this.UpdateUIStats();
    }
    
    // Function that opens the target menu when player attacks
    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> enemies = new List<int>();

        for (int i = 0; i < activeCombatants.Count; i++)
        {
            // Add enemy to target list
            if (!activeCombatants[i].GetComponent<ScriptableObjectProperties>().isPlayer)
            {
                enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (enemies.Count > i)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeCombatantTarget = enemies[i];
                targetButtons[i].targetName.text = activeCombatants[enemies[i]].GetComponent<ScriptableObjectProperties>().objectName;

                // Disable targeting for dead enemies
                if (activeCombatants[enemies[i]].GetComponent<ScriptableObjectProperties>().hasDied)
                {
                    targetButtons[i].GetComponentInParent<Button>().interactable = false;
                }
                else
                {
                    targetButtons[i].GetComponentInParent<Button>().interactable = true;
                }
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Function that opens the magic menu
    public void OpenMagicMenu() 
    {
        magicMenu.SetActive(true);

        for (int i = 0; i< magicButtons.Length; i++)
        {
            var battlerMoveset = activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>().movesAvailable;
            if (battlerMoveset.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                // Set the properties
                magicButtons[i].spellName = battlerMoveset[i];
                magicButtons[i].nameText.text = battlerMoveset[i];

                // TODO: Optimize
                for (int j = 0; j < movesets.Length; j++)
                {
                    if (movesets[j].movesetName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesets[j].movesetCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Function that opens the item menu
    public void OpenItemMenu()
    {
        itemMenu.SetActive(true);

        var playerInventory = GameManager.instance.playerInventory;

        // Loop through GameManager player inventory
        // Display a button for each one
        for (int i = 0; i < playerInventory.Count; i++)
        {
            // Only display items the player has (consumables only for now)
            if (playerInventory[i].item != null && playerInventory[i].item.isItem)
            {
                // Instantiate new button for that item
                GameObject itemButton = Instantiate(itemButtonToInstantiate);
                
                // Set the item name and image
                itemButton.transform.GetChild(0).GetComponentInChildren<Image>().sprite = playerInventory[i].item.itemSprite;
                itemButton.transform.GetChild(1).GetComponentInChildren<Text>().text = playerInventory[i].item.itemName;
                itemButton.transform.GetChild(2).GetComponentInChildren<Text>().text = playerInventory[i].amount.ToString();
                itemButton.transform.SetParent(itemMenu.transform, true);
                itemButton.name = playerInventory[i].item.name;
                itemButton.SetActive(true);

            }
        }
    }

    // Function to allow players to run from battle
    public void Flee()
    {
        int fleeSuccess = Random.Range(0, 100);

        if (fleeSuccess < chanceToFlee)
        {
            // End the battle
            fleeing = true;
            StartCoroutine(EndBattle());
        }
        else
        {
            fleeing = false;

            // Flee not successful
            this.NextTurn();
            battleNotice.theText.text = "Couldn't escape!";
            battleNotice.Activate();
        }
    }

    // Coroutine function that controls the battle when player is victorious
    public IEnumerator EndBattle()
    {
        // Variable to store all the items to drop at the end of combat
        Dictionary<Item, int> itemDrops = new Dictionary<Item, int>();
        
        // Deactivate anything related to Battle
        activeBattle = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);
        targetUseMenu.SetActive(false);
        battleNotice.gameObject.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();       

        // For each player, update the corresponding stats after battle
        for (int i = 0; i < activeCombatants.Count; i++)
        {
            var activeCombatant = activeCombatants[i].GetComponent<ScriptableObjectProperties>();
            if (activeCombatant.isPlayer)
            {
                // Assumption of playerStats[i] and activeCombatants[i] for each player has the same index (it should be)
                var playerStats = GameManager.instance.playerStats[i];

                // Update stats
                if (activeCombatant.objectName == playerStats.charName)
                {
                    GameManager.instance.playerStats[i].currentHP = activeCombatant.currentHP;
                    GameManager.instance.playerStats[i].currentMP = activeCombatant.currentMP;

                    // Set character stat to hasDied
                    if (activeCombatant.hasDied)
                    {
                        playerStats.hasDied = true;
                    }
                }
                
            }
            else
            {
                // Grab all the item drops from each enemy and store it
                foreach (var item in activeCombatant.itemsToDrop)
                {
                    // Randomize the drop rate
                    float percentDrop = Random.Range(0, 100);

                    // If droprate is greater than the Item drop rate, add it to the rewards
                    if (percentDrop > item.itemDropRate)
                    {
                        // If we're getting the same drops from the enemies, grab its amount and just combine it
                        // Else add the custom Item amount
                        if (itemDrops.ContainsKey(item))
                        {
                            var currentValue = itemDrops[item];

                            itemDrops[item] = currentValue + item.amountToDrop;
                        }
                        else
                        {
                            itemDrops.Add(item, item.amountToDrop);
                        }
                    }
                }

                // Calculate total exp to give
                expReward += activeCombatant.expPoints;
                gilReward += activeCombatant.gilDropAmount;
            }

            Destroy(activeCombatant.gameObject);
        }

        yield return new WaitForSeconds(1.5f);
        
        activeCombatants.Clear();
        currentTurn = 0;

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);

        // Allow usage of menu again
        if (fleeing)
        {
            GameManager.instance.activeBattle = false;
            fleeing = false;
        }
        else
        {
            BattleRewards.instance.OpenRewardScreen(expReward, gilReward, itemDrops);
        }

        // Reset EXP/Gil/Item gain
        expReward = 0;
        gilReward = 0;

        // Disable Battle BG
        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    // Function that controls the UI when player loses a battle
    public IEnumerator GameOver()
    {
        activeBattle = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(gameOverScene);
    }

    // Function to change the attack animation of the current user
    private IEnumerator PlayAttackAnimation(int currentUser, string moveName)
    {
        string attackType = "magicAttack";

        // Change animation bool based on the attack
        if (moveName == "Slash")
        {
            attackType = "physicalAttack";
        }

        activeCombatants[currentUser].GetComponent<Animator>().SetBool(attackType, true);
        yield return new WaitForSeconds(1.1f);

        // TODO: Fix later after adding victory fan fare
        // Bug: accessing object when all enemies are dead
        if (activeCombatants[currentTurn])
        {
            activeCombatants[currentUser].GetComponent<Animator>().SetBool(attackType, false);
        }
    }

    // Function to change the attack animation of the current user
    private IEnumerator PlayDamagedAnimation(int currentUser)
    {
        if (activeCombatants[currentTurn])
        {
            activeCombatants[currentUser].GetComponent<Animator>().SetBool("isDamaged", true);
        }

        yield return new WaitForSeconds(1f);

        if (activeCombatants[currentTurn])
        {
            activeCombatants[currentUser].GetComponent<Animator>().SetBool("isDamaged", false);
        }
    }

    // Function that moves the attacking object to the target 
    private IEnumerator MoveTarget(int currentUser, int targetUser)
    {
        float step = 0.5f * Time.deltaTime;
        var attackerPosition = activeCombatants[currentUser].transform.position;
        var defenderPosition = activeCombatants[targetUser].transform.position;
        
        // TODO Add some fancy schmany animation???
        // Move towards the target
        while (Vector2.Distance(activeCombatants[currentUser].transform.position, defenderPosition) > 2f)
        {
            activeCombatants[currentUser].transform.position = Vector2.MoveTowards(activeCombatants[currentUser].transform.position, defenderPosition, step);
        }

        yield return new WaitForSeconds(0.75f);

        // TODO: Fix later after adding victory fan fare
        // Bug: accessing object when all enemies are dead
        if (activeCombatants[currentTurn])
        {
            // Move back to the previous location
            while (Vector2.Distance(activeCombatants[currentUser].transform.position, attackerPosition) > 0f)
            {
                activeCombatants[currentUser].transform.position = Vector2.MoveTowards(activeCombatants[currentUser].transform.position, attackerPosition, step);
            }
        }
    } 
}
