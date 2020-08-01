using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableItemProperties : MonoBehaviour
{
    public Item itemToCreate;

    // Start is called before the first frame update
    void Start()
    {
        // Set the correct sprite for the item
        if (itemToCreate != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemToCreate.itemSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
