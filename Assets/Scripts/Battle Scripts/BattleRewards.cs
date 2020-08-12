using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewards : MonoBehaviour
{
    public static BattleRewards instance;

    public Text xpText, gilText, itemText;
    public GameObject rewardsScreen;

    public Dictionary<Item, int> rewardItems;
    private int xpEarned;
    private int gilReceived;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;    
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenRewardScreen(int xp, int gil, Dictionary<Item, int> rewards)
    {
        xpEarned = xp;
        gilReceived = gil;
        rewardItems = rewards;

        xpText.text = "Earned " + xpEarned + " XP";
        gilText.text = "Received " + gilReceived + "g";
        itemText.text = "";
        
        // Display text for each item obtained
        foreach (var item in rewardItems)
        {
            itemText.text += item.Key.itemName + "\t\t " + item.Value + "x\n";
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

        // Add gil
        GameManager.instance.currentGil += gilReceived;
        
        // Add rewards to player inventory
        foreach (var item in rewardItems) 
        {
            GameManager.instance.AddItem(GameManager.instance.GetItemDetails(item.Key), item.Value);
        }
        
        rewardsScreen.SetActive(false);

        GameManager.instance.activeBattle = false;

        // Reset
        this.ResetVariables();
    }

    private void ResetVariables()
    {
        xpText.text = "Earned " + xpEarned + " XP";
        gilText.text = "Received " + gilReceived + "g";
        itemText.text = "";

        xpEarned = 0;
        gilReceived = 0;
        rewardItems = null;
    }
}
