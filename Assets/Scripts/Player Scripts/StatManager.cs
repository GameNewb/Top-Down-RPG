using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    private CharacterStats[] playerStats;
    string statName;
    public int selectedCharacter = 0;

    [SerializeField] private GameObject subtractButton;
    [SerializeField] private GameObject addButton;

    private void Start()
    {
        playerStats = GameManager.instance.playerStats;
        statName = transform.name.ToLower();

        if (playerStats[selectedCharacter].statPoints <= 0)
        {
            addButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            addButton.GetComponent<Button>().interactable = true;
        }
    }

    // Function to add to the current stat
    public void AddStat()
    {
        // Only allow add when character has enough stat points
        if (playerStats[selectedCharacter].statPoints > 0)
        {
            addButton.GetComponent<Button>().interactable = true;

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

            playerStats[selectedCharacter].statPoints--;
        } 
        else
        {
            addButton.GetComponent<Button>().interactable = false;
        }

        // Update UI
        GameMenu.instance.UpdateDetailedStat(selectedCharacter);
    }

    // Function to subtract from the current stat
    public void SubtractStat()
    {
        switch (statName)
        {
            case "strength":
                playerStats[selectedCharacter].strength--;
                if (playerStats[selectedCharacter].strength <= 0) {
                    playerStats[selectedCharacter].strength = 0;
                    subtractButton.GetComponent<Button>().interactable = false;
                }
                break;

            case "vitality":
                playerStats[selectedCharacter].vitality--;
                if (playerStats[selectedCharacter].vitality <= 0) {
                    playerStats[selectedCharacter].vitality = 0;
                    subtractButton.GetComponent<Button>().interactable = false;
                }
                break;

            case "intelligence":
                playerStats[selectedCharacter].intelligence--;
                if (playerStats[selectedCharacter].intelligence <= 0) {
                    playerStats[selectedCharacter].intelligence = 0;
                    subtractButton.GetComponent<Button>().interactable = false;
                }
                break;

            case "dexterity":
                playerStats[selectedCharacter].dexterity--;
                if (playerStats[selectedCharacter].dexterity <= 0) {
                    playerStats[selectedCharacter].dexterity = 0;
                    subtractButton.GetComponent<Button>().interactable = false;
                }
                break;

            case "luck":
                playerStats[selectedCharacter].luck--;
                if (playerStats[selectedCharacter].luck <= 0) {
                    playerStats[selectedCharacter].luck = 0;
                    subtractButton.GetComponent<Button>().interactable = false;
                }
                break;
        }
        
        playerStats[selectedCharacter].statPoints++;

        // Update UI
        GameMenu.instance.UpdateDetailedStat(selectedCharacter);
    }
}
