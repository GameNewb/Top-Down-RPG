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
    public GameObject enemyScriptablePrefab;

    public List<BattleData> activeBattlers = new List<BattleData>();
    public List<GameObject> activeEnemies = new List<GameObject>();

    public int currentTurn;
    public bool waitingForATurn;

    // UI for player actions
    public GameObject uiButtonsHolder;

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
            StartBattle(new string[] { "Slime", "Skeleton", "Goblin", "Slime", "Slime" });
        }

        if (activeBattle)
        {
            if (waitingForATurn)
            {
                // Activate the button for players
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    // Enemy attack
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
                    for (int j = 0; j < playerPrefabs.Length; j++)
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
                    }
                }
            }

            // Load enemies
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != null && enemiesToSpawn[i] != "")
                {
                    // TODO - optimizie using ScriptableObject or another data structure
                    // Lookup is too slow
                    /*if(GameManager.instance.enemyScriptables.ContainsKey(enemiesToSpawn[i]))
                    {
                        GameObject newEnemy = Instantiate(enemyScriptablePrefab, enemyPositions[i].position, enemyPositions[i].rotation);
                        newEnemy.transform.parent = enemyPositions[i];
                        
                        // Set the appropriate enemy to create
                        newEnemy.GetComponent<CreateScriptableObject>().enemyToCreate = GameManager.instance.FindScriptableObject(enemiesToSpawn[i]);

                        // Ensure sorting layer is "Battle Characters"
                        newEnemy.GetComponent<SpriteRenderer>().sortingLayerName = "Battle Characters";
                        newEnemy.GetComponent<SpriteRenderer>().sprite = GameManager.instance.FindScriptableObject(enemiesToSpawn[i]).enemySprite;

                        // Add enemy
                        activeEnemies.Add(newEnemy);
                    }*/

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
                    }
                }
            }

            waitingForATurn = true;
            currentTurn = Random.Range(0, activeBattlers.Count);
        }
    }

    public void NextTurn()
    {
        currentTurn++;

        if (currentTurn >= activeBattlers.Count)
            currentTurn = 0;

        waitingForATurn = true;

        this.UpdateBattle();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            // Set HP to 0 if they received more damage than max hp
            if (activeBattlers[i].currentHP <= 0)
            {
                activeBattlers[i].currentHP = 0;

                // Handle dead battler
            }
            else
            {
                if (activeBattlers[i].isPlayer)
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
        }
    }
}
