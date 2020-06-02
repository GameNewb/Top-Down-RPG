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

    // HP/MP Stats
    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;

    // Attribute stats
    public int strength;
    public int vitality;
    public int wpnPwr;
    public int armrPwr;

    // Gear 
    public string equippedWeapon;
    public string equippredArmor;
    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        this.CalculateExpRequirement();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            AddExp(100);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        // Level up player
        if (currentEXP > expToNextLevel[playerLevel])
        {
            // Reset EXP
            currentEXP -= expToNextLevel[playerLevel];
            playerLevel++;
        }
    }

    private void CalculateExpRequirement()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = expToNextLevel[i-1] + (int) (i * Mathf.Sqrt(expToNextLevel[i - 1]) * 0.15f);
        }
    }
}
