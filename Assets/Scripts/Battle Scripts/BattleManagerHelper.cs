using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class that populates SO objects
// Used from BattleManager to prevent long lines of code in the Manager class
public class BattleManagerHelper
{
    public void InitializeBattleData(GameObject objectPrefab, bool isPlayer, BattleScriptableObject battleScriptable, CharacterStats characterStats)
    {
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
            objectPrefab.GetComponent<ScriptableObjectProperties>().intelligence = characterStats.intelligence;
            objectPrefab.GetComponent<ScriptableObjectProperties>().dexterity = characterStats.dexterity;
            objectPrefab.GetComponent<ScriptableObjectProperties>().luck = characterStats.luck;
            objectPrefab.GetComponent<ScriptableObjectProperties>().hasDied = characterStats.hasDied;
            objectPrefab.GetComponent<ScriptableObjectProperties>().movesAvailable = battleScriptable.movesAvailable;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectSpriteRenderer = objectPrefab.GetComponent<SpriteRenderer>();
            objectPrefab.GetComponent<ScriptableObjectProperties>().aliveSprite = battleScriptable.aliveSprite;
            objectPrefab.GetComponent<ScriptableObjectProperties>().deadSprite = battleScriptable.deadSprite;
            objectPrefab.GetComponent<ScriptableObjectProperties>().equippedWeapon = characterStats.equippedWeapon;
            objectPrefab.GetComponent<ScriptableObjectProperties>().equippedArmor = characterStats.equippedArmor;
            objectPrefab.GetComponent<ScriptableObjectProperties>().equippedShield = characterStats.equippedShield;
            objectPrefab.GetComponent<ScriptableObjectProperties>().equippedGloves = characterStats.equippedGloves;
            objectPrefab.GetComponent<ScriptableObjectProperties>().equippedBoots = characterStats.equippedBoots;
            objectPrefab.GetComponent<ScriptableObjectProperties>().equippedAccessoryLeft = characterStats.equippedAccessoryLeft;
            objectPrefab.GetComponent<ScriptableObjectProperties>().equippedAccessoryRight = characterStats.equippedAccessoryRight;
            objectPrefab.GetComponent<ScriptableObjectProperties>().isPlayer = true;
            objectPrefab.GetComponent<SpriteRenderer>().sortingOrder = 1;

            // Set AnimatorController if a controller exists
            var animatorResource = Resources.Load<RuntimeAnimatorController>("Battle Animations/" + characterStats.charName + "/" + characterStats.charName + "Controller");
            if (animatorResource)
            {
                objectPrefab.GetComponent<Animator>().runtimeAnimatorController = animatorResource;
            }
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
            objectPrefab.GetComponent<ScriptableObjectProperties>().intelligence = battleScriptable.intelligence;
            objectPrefab.GetComponent<ScriptableObjectProperties>().dexterity = battleScriptable.dexterity;
            objectPrefab.GetComponent<ScriptableObjectProperties>().luck = battleScriptable.luck;
            objectPrefab.GetComponent<ScriptableObjectProperties>().hasDied = battleScriptable.hasDied;
            objectPrefab.GetComponent<ScriptableObjectProperties>().movesAvailable = battleScriptable.movesAvailable;
            objectPrefab.GetComponent<ScriptableObjectProperties>().objectSpriteRenderer = objectPrefab.GetComponent<SpriteRenderer>();
            objectPrefab.GetComponent<ScriptableObjectProperties>().itemsToDrop = battleScriptable.itemsToDrop;
            objectPrefab.GetComponent<ScriptableObjectProperties>().gilDropAmount = Random.Range(battleScriptable.gilMinDropAmount, battleScriptable.gilMaxDropAmount);
            objectPrefab.GetComponent<ScriptableObjectProperties>().expPoints = battleScriptable.expPoints;
            objectPrefab.GetComponent<ScriptableObjectProperties>().isPlayer = false;

            // Flip the sprite
            if (battleScriptable.flipSpriteHorizontally)
            {
                objectPrefab.GetComponent<SpriteRenderer>().flipX = true;
            }

            // Set AnimatorController if a controller exists
            var animatorResource = Resources.Load<RuntimeAnimatorController>("Battle Animations/Enemy Animations/" + battleScriptable.objectName + "/" + battleScriptable.objectName + "Controller");
            if (animatorResource)
            {
                objectPrefab.GetComponent<Animator>().runtimeAnimatorController = animatorResource;
            }
        }

        // Add to the combat list
        BattleManager.instance.activeCombatants.Add(objectPrefab);
    }

    // Function to initialize the magic data prefab
    public void InitializeSkillData(GameObject skillObject, SkillScriptable skillScriptable, GameObject currentUser, GameObject targetUser)
    {
        // Set sorting layers
        skillObject.GetComponent<SpriteRenderer>().sortingLayerName = "Battle Characters";
        skillObject.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // Set appropriate skill properties
        skillObject.GetComponent<SkillProperties>().skillName = skillScriptable.skillName;
        skillObject.GetComponent<SkillProperties>().skillDescription = skillScriptable.skillDescription;
        skillObject.GetComponent<SkillProperties>().skillCost = skillScriptable.skillCost;
        skillObject.GetComponent<SkillProperties>().skillDamage = skillScriptable.skillDamage;
        skillObject.GetComponent<SkillProperties>().skillSprite = skillScriptable.skillSprite;
        skillObject.GetComponent<SkillProperties>().effectLength = skillScriptable.effectLength;
        skillObject.GetComponent<SkillProperties>().soundEffect = skillScriptable.soundEffect;
        skillObject.GetComponent<SkillProperties>().skillAnimation = skillScriptable.skillAnimation;
        skillObject.GetComponent<Animator>().runtimeAnimatorController = skillScriptable.skillAnimation;

        // Set current and target user
        skillObject.GetComponent<SkillProperties>().currentUser = currentUser;
        skillObject.GetComponent<SkillProperties>().targetUser = targetUser;
    }
}
