using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager instance;

    public CharacterStats[] playerStats;

    // Control the player controls when any of this is active/true
    public bool gameMenuOpen;
    public bool dialogActive;

    // Item variables
    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerController = PlayerController.instance;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Player can only move if nothing is opened
        // TODO - Optimize
        if (gameMenuOpen || dialogActive)
        {
            playerController.canMove = false;
        } 
        else
        {
            playerController.canMove = true;
        }
    }

    public Item GetItemDetails(string itemToGet)
    {
        // Get the item we're looking for
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if(referenceItems[i].itemName == itemToGet)
            {
                return referenceItems[i];
            }
        }

        // No item found
        return null;
    }

    public void SortItems()
    {
        // Variable to keep track of what's the next empty slot
        int emptySlot = 0;

        // Iterate through each inventory slot that we have
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            // If we see an available item in the inventory, move it to an empty slot
            if (itemsHeld[i] != "")
            {
                itemsHeld[emptySlot] = itemsHeld[i];
                numberOfItems[emptySlot] = numberOfItems[i];
                emptySlot++;
            }
        }

        // Populate inventory with "empty" items
        // We start at the next empty slot, and just iterate through the remaining slots left
        for (int i = emptySlot; i < itemsHeld.Length; i++)
        {
            // Reset the values
            itemsHeld[i] = "";
            numberOfItems[i] = 0;
        }
    }
}
