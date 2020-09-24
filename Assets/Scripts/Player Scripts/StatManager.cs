using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    private CharacterStats[] playerStats;
    string statName;
    public int selectedCharacter = 0;

    private void Start()
    {
        playerStats = GameManager.instance.playerStats;
        statName = transform.name.ToLower();
    }

    // Function to add to the current stat
    public void AddStat()
    {
        switch (statName)
        {
            case "strength":
                playerStats[selectedCharacter].strength++;
                break;
            case "vitality":
                playerStats[selectedCharacter].vitality++;
                break;
            case "intelligence":
                playerStats[selectedCharacter].intelligence++;
                break;
            case "dexterity":
                playerStats[selectedCharacter].dexterity++;
                break;
            case "luck":
                playerStats[selectedCharacter].luck++;
                break;
        }

        // Update UI
        GameMenu.instance.UpdatDetailedStat(selectedCharacter);
    }

    // Function to subtract from the current stat
    public void SubtractStat()
    {
        switch (statName)
        {
            case "strength":
                playerStats[selectedCharacter].strength--;
                break;
            case "vitality":
                playerStats[selectedCharacter].vitality--;
                break;
            case "intelligence":
                playerStats[selectedCharacter].intelligence--;
                break;
            case "dexterity":
                playerStats[selectedCharacter].dexterity--;
                break;
            case "luck":
                playerStats[selectedCharacter].luck--;
                break;
        }

        // Update UI
        GameMenu.instance.UpdatDetailedStat(selectedCharacter);
    }
}
