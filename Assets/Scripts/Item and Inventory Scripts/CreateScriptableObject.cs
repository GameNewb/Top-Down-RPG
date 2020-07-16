using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScriptableObject : MonoBehaviour
{
    public Item itemToCreate;
    public EnemyScriptableObject enemyToCreate;

    public void Awake()
    {
        // Set the correct sprite for the item
        if (itemToCreate != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemToCreate.itemSprite;
        }

        if (enemyToCreate != null)
        {
            GetComponent<SpriteRenderer>().sprite = enemyToCreate.enemySprite;
        }
    }
}
