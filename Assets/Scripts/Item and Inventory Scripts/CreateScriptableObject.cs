using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScriptableObject : MonoBehaviour
{
    public Item itemToCreate;

    public void Awake()
    {
        // Set the correct sprite for the item
        GetComponent<SpriteRenderer>().sprite = itemToCreate.itemSprite;
    }
}
