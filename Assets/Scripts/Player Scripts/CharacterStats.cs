using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Player properties
    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 100;
    public bool hasDied = false;

    // HP/MP Stats
    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLVLBonus;

    // Attribute stats
    public int statPoints = 0;
    public int strength;
    public int vitality;
    public int intelligence;
    public int dexterity;
    public int luck;
    public int wpnPwr;
    public int armrPwr;

    // Gear 
    public Item equippedWeapon;
    public Item equippedShield;
    public Item equippedArmor;
    public Item equippedGloves;
    public Item equippedBoots;
    public Item equippedAccessoryLeft;
    public Item equippedAccessoryRight;
    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        this.CalculateExpRequirement();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Function to add exp
    // TODO: change LVL up logic
    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        // Level up player
        if (playerLevel < maxLevel)
        {
            if(currentEXP >= expToNextLevel[playerLevel])
            {
                // Reset EXP
                currentEXP -= expToNextLevel[playerLevel];
                playerLevel++;
                statPoints += 3;

                // Determine whether to add to str or vitality
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    vitality++;
                }

                // Increase and reset HP
                maxHP = Mathf.FloorToInt(maxHP * 1.08f);
                currentHP = maxHP;

                maxMP += mpLVLBonus[playerLevel-1];
                currentMP = maxMP;
            }
        }

        // Do not let player exceed max level
        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }

    // TODO: Fix exp leak
    private void CalculateExpRequirement()
    {
        expToNextLevel = new int[maxLevel];
        mpLVLBonus = new int[maxLevel];
        expToNextLevel[1] = baseEXP;
        mpLVLBonus[1] = maxMP;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = expToNextLevel[i - 1] + (int) (i * Mathf.Sqrt(expToNextLevel[i - 1]) * 0.15f);
            mpLVLBonus[i] = mpLVLBonus[i - 1] + Mathf.FloorToInt(i * 0.25f);
        }
    }
}
