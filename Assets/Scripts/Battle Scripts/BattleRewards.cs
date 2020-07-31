using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewards : MonoBehaviour
{
    public static BattleRewards instance;

    public Text xpText, itemText;
    public GameObject rewardsScreen;

    public string[] rewardItems;
    public int xpEarned;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(54, new string[] { "HP Potion", "Iron Armor" });
        }
    }

    public void OpenRewardScreen(int xp, string[] rewards)
    {
        xpEarned = xp;
        rewardItems = rewards;

        xpText.text = "Earned " + xpEarned + " XP";
        itemText.text = "";

        // Display text for each item obtained
        for (int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardsScreen.SetActive(true);
    }

    public void CloseRewardScreen()
    {
        var playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            // Add EXP to active characters and alive characters only
            if (playerStats[i].gameObject.activeInHierarchy && !playerStats[i].hasDied)
            {
                GameManager.instance.playerStats[i].AddExp(xpEarned);
            }
        }
        
        // TODO: maybe refactor rewardItems to use Item instead of string???
        // Add rewards to player inventory
        for (int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(GameManager.instance.GetItemDetailsByName(rewardItems[i]), 1);
        }


        rewardsScreen.SetActive(false);

        GameManager.instance.activeBattle = false;
    }
}
