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
                    if(GameManager.instance.enemyScriptables.ContainsKey(enemiesToSpawn[i]))
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
                    }

                    /*for (int j = 0; j < enemyPrefabs.Length; j++)
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
        }
    }
}
