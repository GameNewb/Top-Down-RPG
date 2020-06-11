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
    [SerializeField] public string[] itemsHeld;
    [SerializeField] public int[] numberOfItems;
    [SerializeField] public Item[] referenceItems;

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
}
