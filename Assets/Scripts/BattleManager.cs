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
            StartBattle(new string[] { "Slime", "Skeleton" });
        }
    }

    public void StartBattle (string[] enemiesToSpawn)
    {
        // Check first if we're not in a battle already
        if (!activeBattle)
        {
            activeBattle = true;

            GameManager.instance.activeBattle = true;

            Debug.Log("X: " + Camera.main.transform.position.x);

            Debug.Log("Y: " + Camera.main.transform.position.y);

            // Set the appropriate BG position
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);
        }
    }
}
