using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private BattleManagerHelper bmHelper = new BattleManagerHelper();
    public static BattleManager instance;

    private bool activeBattle;

    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleData[] playerPrefabs;
    public BattleData[] enemyPrefabs;
    public GameObject objectScriptablePrefab;

    public List<BattleData> activeBattlers = new List<BattleData>();
    public List<GameObject> activeCombatants = new List<GameObject>();

    public int currentTurn;
    public bool waitingForATurn;

    // UI for player actions
    public GameObject uiButtonsHolder;

    // Attack sets and effect
    public BattleMoveset[] movesets;
    public GameObject enemyParticleEffect;

    public BattleDamageNumber damageNumberEffect;

    public Text[] playerName, playerHP, playerMP;

    // UI for target buttons
    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            this.StartBattle(new string[] { "Slime", "Skeleton", "Goblin" });
        }

        if (activeBattle)
        {
            if (waitingForATurn)
            {  
                // Activate the button for players
                if (activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>().isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    // Enemy attack
                    StartCoroutine(EnemyMove());
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                this.NextTurn();
            }
        }
    }

    public void StartBattle (string[] enemiesToSpawn)
    {
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
                        // Create the player character battle object
                        // Allow movement of player objects
                        GameObject newPlayer = Instantiate(objectScriptablePrefab, playerPositions[i].position, playerPositions[i].rotation);
                        newPlayer.transform.parent = playerPositions[i];

                        bmHelper.InitializeBattleData(newPlayer, true, playerStats.charName, playerStats);
                    }
                }
            }

            // Load enemies
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != null && enemiesToSpawn[i] != "")
                {
                    if (GameManager.instance.enemyScriptables.ContainsKey(enemiesToSpawn[i]))
                    {
                        // TODO: Merge into one scriptable object
                        GameObject newEnemy = Instantiate(objectScriptablePrefab, enemyPositions[i].position, enemyPositions[i].rotation);
                        newEnemy.transform.parent = enemyPositions[i];

                        bmHelper.InitializeBattleData(newEnemy, false, enemiesToSpawn[i], null);
                    }
                }
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
            // TODO: Merge into one scriptable object
            // Set HP to 0 if they received more damage than max hp
            if (activeCombatants[i].GetComponent<ScriptableObjectProperties>().currentHP <= 0)
            {
                activeCombatants[i].GetComponent<ScriptableObjectProperties>().currentHP = 0;

                // Handle dead battler
            }
            else
            {
                if (activeCombatants[i].GetComponent<ScriptableObjectProperties>().isPlayer)
                {
                    allPlayersDead = false;
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
            }
            else
            {
                // Game over
            }

            battleScene.SetActive(false);
            GameManager.instance.activeBattle = false;
            activeBattle = false;

            // Destroy object after battle is done 
            foreach(var combatants in activeCombatants)
            {
                Destroy(combatants);
            }

            activeCombatants.Clear();
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

    public IEnumerator EnemyMove()
    {
        waitingForATurn = false;

        yield return new WaitForSeconds(1f);

        this.EnemyAttack();

        yield return new WaitForSeconds(1f);

        this.NextTurn();
    }

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
        // TODO: Fix bug where if player presses N continuously during enemy turn, IOB happens
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
        this.DealDamage(selectedTarget, movesetPower);
    }

    // Function to deal damage to objects
    public void DealDamage(int target, int movesetPower)
    {
        var currentUser = activeCombatants[currentTurn].GetComponent<ScriptableObjectProperties>();
        var targetUser = activeCombatants[target].GetComponent<ScriptableObjectProperties>();
        float atkPower = currentUser.strength + currentUser.wpnPwr;
        float vitPower = targetUser.vitality + targetUser.armrPwr;

        // Calculate the damage to take
        float damageCalculation = (atkPower / vitPower) * movesetPower * Random.Range(.9f, 1.1f);
        int damageToTake = Mathf.RoundToInt(damageCalculation);
        
        targetUser.currentHP -= damageToTake;

        // Set status
        if (targetUser.currentHP <= 0)
        {
            targetUser.hasDied = true;
        }

        // Instantiate the damage numbers on screen
        Instantiate(damageNumberEffect, activeCombatants[target].transform.position, activeCombatants[target].transform.rotation).SetDamage(damageToTake);
        
        // Update player UI stats
        this.UpdateUIStats();
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

        this.DealDamage(selectedTarget, movesetPower);

        // Prevent player from clicking the buttons multiple times
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        this.NextTurn();
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
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
