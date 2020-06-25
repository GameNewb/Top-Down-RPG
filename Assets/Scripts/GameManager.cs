﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager instance;

    public CharacterStats[] playerStats;

    // Control the player controls when any of this is active/true
    public bool gameMenuOpen;
    public bool dialogActive;

    // Item variables
    public Item[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;
    
    public List<InventorySlots> playerInventory = new List<InventorySlots>();

    public Item itemToAddLater;

    private int nextEmptySlot;

    public int currentGil;
    public int maxGil;
    
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

        if (Input.GetKeyDown(KeyCode.J))
        {
            this.AddItem(itemToAddLater, 1);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            this.RemoveItem(itemToAddLater, 1);
        }
    }

    public Item GetItemDetails(Item itemToGet)
    {
        // Get the item we're looking for
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if(referenceItems[i] == itemToGet)
            {
                return referenceItems[i];
            }
        }

        // No item found
        return null;
    }

    // Sorts the player inventory using two-point algorithm
    public void SortItems()
    {
        // Variable to keep track of what's the next empty slot
        int emptySlot = 0;

        // Iterate through each inventory slot that we have
        for (int i = 0; i < playerInventory.Count; i++)
        {
            // If we see an available item in the inventory, move it to an empty slot
            if (playerInventory[i].item != null)
            {
                playerInventory[emptySlot].item = playerInventory[i].item;
                playerInventory[emptySlot].amount = playerInventory[i].amount;
                emptySlot++;
            }
        }

        // Global variable that keeps track of our next empty slot
        nextEmptySlot = emptySlot;

        // Populate inventory with "empty" items
        // We start at the next empty slot, and just iterate through the remaining slots left
        for (int i = emptySlot; i < playerInventory.Count; i++)
        {
            // Reset the values
            playerInventory[i].item = null;
            playerInventory[i].amount = 0;
        }
    }

    public void AddItem(Item itemToAdd, int amount)
    {
        bool hasItem = false;

        // Sort Items first to easily distinguish the next empty slot
        this.SortItems();

        // Iterate through the current Player inventory to see if we have the item
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (playerInventory[i].item == itemToAdd)
            {
                playerInventory[i].amount += amount;
                hasItem = true;
                break;
            }
        }

        // If player doesn't have item, add it to next empty slot
        if (!hasItem)
        {
            playerInventory[nextEmptySlot].item = itemToAdd;
            playerInventory[nextEmptySlot].amount = amount;
        }
        
        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(Item itemToRemove, int amount)
    {
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (playerInventory[i].item == itemToRemove)
            {
                playerInventory[i].amount -= amount;
                
                // Remove the item from our inventory if the amount is less than 0
                if (playerInventory[i].amount <= 0)
                {
                    playerInventory[i].item = null;
                    playerInventory[i].amount = 0;
                }

                break;
            }
        }

        GameMenu.instance.ShowItems();
    }
}


/* OLD CODE */
/*
 * 
    public void AddItem(Item itemToAdd, int amount)
    {
        bool hasItem = false;

        // Sort Items first to easily distinguish the next empty slot
        this.SortItems();

        // Iterate through the current Player inventory to see if we have the item
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToAdd)
            {
                numberOfItems[i] += amount;
                hasItem = true;
                break;
            }
        }

        // If player doesn't have item, add it to next empty slot
        if (!hasItem)
        {
            itemsHeld[nextEmptySlot] = itemToAdd;
            numberOfItems[nextEmptySlot] += amount;
        }

        GameMenu.instance.ShowItems();
    }
 * 
 * 

    public void SortItems()
    {
        // Variable to keep track of what's the next empty slot
        int emptySlot = 0;

        // Iterate through each inventory slot that we have
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            // If we see an available item in the inventory, move it to an empty slot
            if (itemsHeld[i])
            {
                itemsHeld[emptySlot] = itemsHeld[i];
                numberOfItems[emptySlot] = numberOfItems[i];
                emptySlot++;
            }
        }

        // Global variable that keeps track of our next empty slot
        nextEmptySlot = emptySlot;

        // Populate inventory with "empty" items
        // We start at the next empty slot, and just iterate through the remaining slots left
        for (int i = emptySlot; i < itemsHeld.Length; i++)
        {
            // Reset the values
            itemsHeld[i] = null;
            numberOfItems[i] = 0;
        }
    }
 *
 * 
 */
