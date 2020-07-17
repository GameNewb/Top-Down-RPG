using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScriptableObject : MonoBehaviour
{
    public Item itemToCreate;
    public BattleScriptableObject objectToCreate;

    public void Awake()
    {
        // Set the correct sprite for the item
        if (itemToCreate != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemToCreate.itemSprite;
        }

        if (objectToCreate != null)
        {
            GetComponent<SpriteRenderer>().sprite = objectToCreate.objectSprite;
        }
    }
}
