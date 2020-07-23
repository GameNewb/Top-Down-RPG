using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
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
            StartBattle(new string[] { "Slime", "Skeleton", "Goblin" });
        }

        if (activeBattle)
        {
            if (waitingForATurn)
            {
                // Activate the button for players
                if (activeCombatants[currentTurn].GetComponent<CreateScriptableObject>().objectToCreate.isPlayer)
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

                        // Set the appropriate enemy to create
                        newPlayer.GetComponent<CreateScriptableObject>().objectToCreate = GameManager.instance.FindScriptableObject(playerStats.charName, true);

                        // Ensure sorting layer is "Battle Characters"
                        newPlayer.GetComponent<SpriteRenderer>().sortingLayerName = "Battle Characters";
                        newPlayer.GetComponent<SpriteRenderer>().sprite = GameManager.instance.FindScriptableObject(playerStats.charName, true).objectSprite;

                        // Add to the battle list
                        activeCombatants.Add(newPlayer);

                        // Load data
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.currentHP = playerStats.currentHP;
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.currentMP = playerStats.currentMP;
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.maxHP = playerStats.maxHP;
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.maxMP = playerStats.maxMP;
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.strength = playerStats.strength;
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.vitality = playerStats.vitality;
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.wpnPwr = playerStats.wpnPwr;
                        activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.armrPwr = playerStats.armrPwr;
                    }
                    /*for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == playerStats.charName)
                        {
                            // Create the player character battle object
                            // Allow movement of player objects
                            BattleData newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];

                            // Add to the battle list
                            activeBattlers.Add(newPlayer);

                            // Load data
                            activeBattlers[i].currentHP = playerStats.currentHP;
                            activeBattlers[i].currentMP = playerStats.currentMP;
                            activeBattlers[i].maxHP = playerStats.maxHP;
                            activeBattlers[i].maxMP = playerStats.maxMP;
                            activeBattlers[i].strength = playerStats.strength;
                            activeBattlers[i].vitality = playerStats.vitality;
                            activeBattlers[i].wpnPwr = playerStats.wpnPwr;
                            activeBattlers[i].armrPwr = playerStats.armrPwr;
                        }
                    }*/
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
                        
                        // Set the appropriate enemy to create
                        newEnemy.GetComponent<CreateScriptableObject>().objectToCreate = GameManager.instance.FindScriptableObject(enemiesToSpawn[i], false);

                        // Ensure sorting layer is "Battle Characters"
                        newEnemy.GetComponent<SpriteRenderer>().sortingLayerName = "Battle Characters";
                        newEnemy.GetComponent<SpriteRenderer>().sprite = GameManager.instance.FindScriptableObject(enemiesToSpawn[i], false).objectSprite;

                        // Add enemy
                        activeCombatants.Add(newEnemy);
                    }

                    /*
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleData newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];

                            // Ensure sorting layer is "Battle Characters"
                            newEnemy.GetComponent<SpriteRenderer>().sortingLayerName = "Battle Characters";

                            // Add enemy
                            activeBattlers.Add(newEnemy);
                        }
                    }*/
                }
            }

            waitingForATurn = true;
            currentTurn = Random.Range(0, activeCombatants.Count);
        }
    }

    public void NextTurn()
    {
        currentTurn++;

        if (currentTurn >= activeCombatants.Count)
            currentTurn = 0;

        waitingForATurn = true;

        this.UpdateBattle();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeCombatants.Count; i++)
        {
            // TODO: Merge into one scriptable object
            // Set HP to 0 if they received more damage than max hp
            if (activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.currentHP <= 0)
            {
                activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.currentHP = 0;

                // Handle dead battler
            }
            else
            {
                if (activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate.isPlayer)
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
            var combatant = activeCombatants[i].GetComponent<CreateScriptableObject>().objectToCreate;
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
        int selectAttack = Random.Range(0, activeCombatants[currentTurn].GetComponent<CreateScriptableObject>().objectToCreate.movesAvailable.Length);
        int movesetPower = 0;

        for (int i = 0; i < movesets.Length; i++)
        {
            if (movesets[i].movesetName == activeCombatants[currentTurn].GetComponent<CreateScriptableObject>().objectToCreate.movesAvailable[selectAttack])
            {
                Instantiate(movesets[i].movesetEffect, activeCombatants[selectedTarget].transform.position, activeCombatants[selectedTarget].transform.rotation);
                movesetPower = movesets[i].movesetDamage;
            }
        }

        Instantiate(enemyParticleEffect, activeCombatants[currentTurn].transform.position, activeCombatants[currentTurn].transform.rotation);

        // Calculate the damage
        this.DealDamage(selectedTarget, movesetPower);
    }

    public void DealDamage(int target, int movesetPower)
    {
        var currentUser = activeCombatants[currentTurn].GetComponent<CreateScriptableObject>().objectToCreate;
        var targetUser = activeCombatants[target].GetComponent<CreateScriptableObject>().objectToCreate;
        float atkPower = currentUser.strength + currentUser.wpnPwr;
        float vitPower = targetUser.vitality + targetUser.armrPwr;

        float damageCalculation = (atkPower / vitPower) * movesetPower * Random.Range(.9f, 1.1f);
        int damageToTake = Mathf.RoundToInt(damageCalculation);

        Debug.Log(currentUser.objectName + " is dealing " + damageCalculation + "(" + damageToTake + ") damage to " + targetUser.objectName);

        targetUser.currentHP -= damageToTake;

        Instantiate(damageNumberEffect, activeCombatants[target].transform.position, activeCombatants[target].transform.rotation).SetDamage(damageToTake);
    }
    
}
