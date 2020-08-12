using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class that populates SO objects
// Used from BattleManager to prevent long lines of code in the Manager class
public class BattleManagerHelper
{
    public void InitializeBattleData(GameObject objectPrefab, bool isPlayer, BattleScriptableObject battleScriptable, CharacterStats characterStats)
    {
        // Grab the appropriate scriptable object
        //var scriptableObject = GameManager.instance.FindScriptableObject(objectName, isPlayer);

        // Set the Object to create using the ScriptableObject
        objectPrefab.GetComponent<ScriptableObjectProperties>().objectToCreate = battleScriptable;

        // Set sorting layers
        objectPrefab.GetComponent<SpriteRenderer>().sortingLayerName = "Battle Characters";
        objectPrefab.GetComponent<SpriteRenderer>().sprite = battleScriptable.objectSprite;

        // Set stats
        if (isPlayer)
        {
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectName = characterStats.charName;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectSprite = characterStats.charImage;
            objectPrefab.GetComponent<ScriptableObjectProperties>().currentHP = characterStats.currentHP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().currentMP = characterStats.currentMP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().maxHP = characterStats.maxHP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().maxMP = characterStats.maxMP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().wpnPwr = characterStats.wpnPwr;
            objectPrefab.GetComponent<ScriptableObjectProperties>().armrPwr = characterStats.armrPwr;
            objectPrefab.GetComponent<ScriptableObjectProperties>().strength = characterStats.strength;
            objectPrefab.GetComponent<ScriptableObjectProperties>().vitality = characterStats.vitality;
            objectPrefab.GetComponent<ScriptableObjectProperties>().hasDied = characterStats.hasDied;
            objectPrefab.GetComponent<ScriptableObjectProperties>().movesAvailable = battleScriptable.movesAvailable;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectSpriteRenderer = objectPrefab.GetComponent<SpriteRenderer>();
            objectPrefab.GetComponent<ScriptableObjectProperties>().aliveSprite = battleScriptable.aliveSprite;
            objectPrefab.GetComponent<ScriptableObjectProperties>().deadSprite = battleScriptable.deadSprite;
            objectPrefab.GetComponent<ScriptableObjectProperties>().isPlayer = true;
        }
        else
        {
            // Set enemy data
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectName = battleScriptable.objectName;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectDescription = battleScriptable.objectDescription;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectSprite = battleScriptable.objectSprite;
            objectPrefab.GetComponent<ScriptableObjectProperties>().currentHP = battleScriptable.currentHP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().currentMP = battleScriptable.currentMP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().maxHP = battleScriptable.maxHP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().maxMP = battleScriptable.maxMP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().strength = battleScriptable.strength;
            objectPrefab.GetComponent<ScriptableObjectProperties>().vitality = battleScriptable.vitality;
            objectPrefab.GetComponent<ScriptableObjectProperties>().hasDied = battleScriptable.hasDied;
            objectPrefab.GetComponent<ScriptableObjectProperties>().movesAvailable = battleScriptable.movesAvailable;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectSpriteRenderer = objectPrefab.GetComponent<SpriteRenderer>();
            objectPrefab.GetComponent<ScriptableObjectProperties>().itemsToDrop = battleScriptable.itemsToDrop;
            objectPrefab.GetComponent<ScriptableObjectProperties>().gilDropAmount = Random.Range(battleScriptable.gilMinDropAmount, battleScriptable.gilMaxDropAmount);
            objectPrefab.GetComponent<ScriptableObjectProperties>().expPoints = battleScriptable.expPoints;
            objectPrefab.GetComponent<ScriptableObjectProperties>().isPlayer = false;
        }

        // Add to the combat list
        BattleManager.instance.activeCombatants.Add(objectPrefab);
    }
    
    /*public void EndBattle()
    {
        GameManager.instance.activeBattle = false;
        activeBattle = false;

        // Destroy object after battle is done 
        foreach (var combatants in activeCombatants)
        {
            Destroy(combatants);
        }

        activeCombatants.Clear();

        // Allow usage of menu again
        GameManager.instance.activeBattle = false;

        // Disable BG
        AudioManager.instance.StopMusic();
    }*/
}
