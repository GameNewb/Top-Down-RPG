using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemButton : MonoBehaviour
{
    public GameObject targetMenu;

    public Button[] characterNameButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        // Activate menu to use item on
        targetMenu.SetActive(true);
        
        for (int i = 0; i < characterNameButtons.Length; i++)
        {
            // Only show UI for active characters
            var playerStats = GameManager.instance.playerStats[i];
            if (playerStats && playerStats.gameObject.activeInHierarchy)
            {
                characterNameButtons[i].gameObject.SetActive(true);
                characterNameButtons[i].GetComponentInChildren<Text>().text = playerStats.charName.ToString();
            }
            else
            {
                characterNameButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
