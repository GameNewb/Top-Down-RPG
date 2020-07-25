using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerHelper
{
    public void InitializeBattleData(GameObject objectPrefab, bool isPlayer, string objectName, CharacterStats characterStats)
    {
        // Grab the appropriate scriptable object
        var scriptableObject = GameManager.instance.FindScriptableObject(objectName, isPlayer);

        // Set the Object to create using the ScriptableObject
        objectPrefab.GetComponent<ScriptableObjectProperties>().objectToCreate = scriptableObject;

        // Set sorting layers
        objectPrefab.GetComponent<SpriteRenderer>().sortingLayerName = "Battle Characters";
        objectPrefab.GetComponent<SpriteRenderer>().sprite = scriptableObject.objectSprite;

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
            objectPrefab.GetComponent<ScriptableObjectProperties>().isPlayer = true;
        }
        else
        {
            // Set enemy data
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectName = scriptableObject.objectName;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectDescription = scriptableObject.objectDescription;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectSprite = scriptableObject.objectSprite;
            objectPrefab.GetComponent<ScriptableObjectProperties>().currentHP = scriptableObject.currentHP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().currentMP = scriptableObject.currentMP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().maxHP = scriptableObject.maxHP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().maxMP = scriptableObject.maxMP;
            objectPrefab.GetComponent<ScriptableObjectProperties>().strength = scriptableObject.strength;
            objectPrefab.GetComponent<ScriptableObjectProperties>().vitality = scriptableObject.vitality;
            objectPrefab.GetComponent<ScriptableObjectProperties>().hasDied = scriptableObject.hasDied;
            objectPrefab.GetComponent<ScriptableObjectProperties>().movesAvailable = scriptableObject.movesAvailable;
            objectPrefab.GetComponent<ScriptableObjectProperties>().isPlayer = false;
        }

        // Add to the combat list
        BattleManager.instance.activeCombatants.Add(objectPrefab);
    }
}
