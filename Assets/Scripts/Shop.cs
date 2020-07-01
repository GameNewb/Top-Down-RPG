using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    [SerializeField] private Text gilText;
    
    public List<InventorySlots> itemsForSale = new List<InventorySlots>();

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    private InventoryHelper inventoryHelper;

    public InventorySlots selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    private Button parentButton;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        inventoryHelper = new InventoryHelper();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        this.OpenBuyMenu();

        GameManager.instance.dialogActive = true;

        gilText.text = GameManager.instance.currentGil.ToString() + "g";
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);

        GameManager.instance.dialogActive = false;
    }

    public void OpenBuyMenu()
    {
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        // Display all the items that the shopkeeper has
        inventoryHelper.PopulateInventoryButtons(itemsForSale, buyItemButtons);

        // Set the first item to showcase
        buyItemButtons[0].Press();
    }

    public void OpenSellMenu()
    {
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        // Display all the items that the player has
        this.RefreshUI();

        // Set the first item to showcase
        sellItemButtons[0].Press();
    }

    public void SelectBuyItem(InventorySlots itemToBuy, Button referencedButton)
    {
        selectedItem = itemToBuy;
        parentButton = referencedButton;
        this.ChangeBuyText(selectedItem.item.itemName, selectedItem.item.itemDescription, "Value: " + selectedItem.item.value + "g");
    }

    public void SelectSellItem(InventorySlots itemToSell)
    {
        selectedItem = itemToSell;
        this.ChangeSellText(selectedItem.item.itemName, selectedItem.item.itemDescription, "Value: " + Mathf.FloorToInt(selectedItem.item.value * .5f).ToString() + "g");
    }

    // TODO: Modify script to add/subtract from Shopkeeper amount
    public void BuyItem()
    {
        if (selectedItem.item != null)
        {
            var gmInstance = GameManager.instance;

            // If player has enough gil and shopkeeper still have stock
            if (gmInstance.currentGil >= selectedItem.item.value && selectedItem.amount > 0)
            {
                // Buy the item and subtract amount from shopkeeper
                gmInstance.currentGil -= selectedItem.item.value;
                gmInstance.AddItem(selectedItem.item, 1);
                selectedItem.amount--;

                gilText.text = gmInstance.currentGil.ToString() + "g";
                
                // Update UI to reflect correct number of available items
                inventoryHelper.PopulateInventoryButtons(itemsForSale, buyItemButtons);

                // Disable parent button when item is sold out
                if (parentButton != null && selectedItem.amount <= 0)
                {
                    parentButton.interactable = false;
                }
            }
        }
    }
    
    // Function to sell items when "Sell" button is clicked
    public void SellItem()
    {
        if (selectedItem.item != null && selectedItem.amount > 0)
        {
            var gmInstance = GameManager.instance;
            
            gmInstance.currentGil += Mathf.FloorToInt(selectedItem.item.value * .5f);
            gmInstance.RemoveItem(selectedItem.item, 1);

            gilText.text = gmInstance.currentGil.ToString() + "g";
            
            this.RefreshUI();
        }
    }

    // Refreshes the Sell panel to display accurate info
    private void RefreshUI()
    {
        // Sort the items again and repopulate the UI so that there are no empty slots in the inventory
        GameManager.instance.SortItems();
        inventoryHelper.PopulateInventoryButtons(GameManager.instance.playerInventory, sellItemButtons);

        // Set appropriate text depending on selected item to sell
        if (selectedItem.item != null)
        {
            this.ChangeSellText(selectedItem.item.itemName, selectedItem.item.itemDescription, "Value: " + Mathf.FloorToInt(selectedItem.item.value * .5f).ToString() + "g");
        }
        else if (selectedItem.item == null)
        {
            this.ChangeSellText("", "", "");
        }
    }

    // Function to change the text of Buy related UI
    private void ChangeBuyText(string itemNameText, string itemDescriptionText, string itemValueText)
    {
        buyItemName.text = itemNameText;
        buyItemDescription.text = itemDescriptionText;
        buyItemValue.text = itemValueText;
    }

    // Function to change the text of Sell related UI
    private void ChangeSellText(string itemNameText, string itemDescriptionText, string itemValueText)
    {
        sellItemName.text = itemNameText;
        sellItemDescription.text = itemDescriptionText;
        sellItemValue.text = itemValueText;
    }
}
