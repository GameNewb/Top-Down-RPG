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
    public GameObject buyInBulkPanel;
    public GameObject sellInBulkPanel;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    private Button parentButton;

    private GameObject buyAmountFieldObj;
    private GameObject sellAmountFieldObj;
    private InputField buyInputField;
    private InputField sellInputField;
    private int buyAmountText;
    private int sellAmountText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        inventoryHelper = new InventoryHelper();

        // Set Listener for buy amount text field
        if (buyInBulkPanel != null)
        {
            var eventListener = new InputField.SubmitEvent();
            buyAmountFieldObj = buyInBulkPanel.transform.GetChild(1).gameObject;
            buyInputField = buyAmountFieldObj.GetComponent<InputField>();
            eventListener.AddListener(SetBuyAmount);
            buyInputField.onEndEdit = eventListener;
        }

        // Set Listener for sell amount text field
        if (sellInBulkPanel != null)
        {
            var eventListener = new InputField.SubmitEvent();
            sellAmountFieldObj = sellInBulkPanel.transform.GetChild(1).gameObject;
            sellInputField = sellAmountFieldObj.GetComponent<InputField>();
            eventListener.AddListener(SetSellAmount);
            sellInputField.onEndEdit = eventListener;
        }
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

    // Function to buy items when "Buy" button is clicked
    public void BuyItem()
    {
        if (selectedItem.item != null)
        {
            var gmInstance = GameManager.instance;

            // If player has enough gil and shopkeeper still have stock
            if (gmInstance.currentGil >= selectedItem.item.value && selectedItem.amount > 0)
            {
                // If there's more than 1 amount in the shopkeeper, allow player to specify number they want to buy
                if (selectedItem.amount > 1)
                {
                    // Display panel and allow player to buy more items
                    buyInBulkPanel.SetActive(true);
                }
                else
                {
                    // Buy the item and subtract amount from shopkeeper
                    gmInstance.currentGil -= selectedItem.item.value;
                    gmInstance.AddItem(selectedItem.item, 1);
                    selectedItem.amount--;
                    gilText.text = gmInstance.currentGil.ToString() + "g";
                }
                
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
            
            if (selectedItem.amount > 1)
            {
                sellInBulkPanel.SetActive(true);
            } 
            else
            {
                gmInstance.currentGil += Mathf.FloorToInt(selectedItem.item.value * .5f);
                gmInstance.RemoveItem(selectedItem.item, 1);
                gilText.text = gmInstance.currentGil.ToString() + "g";

                this.RefreshUI();
            }
        }
    }

    // Function used by the "Confirm" button in the "Buy" panel
    public void BuyInBulk()
    {
        var gmInstance = GameManager.instance;

        // If player inputs a number thats more than what the shopkeeper has, set the value to what the shopkeeper has
        if (buyAmountText > selectedItem.amount)
        {
            buyAmountText = selectedItem.amount;
        }

        int totalCost = buyAmountText * selectedItem.item.value;

        // If player has enough gil, allow the purchase
        if (gmInstance.currentGil >= totalCost)
        {
            gmInstance.AddItem(selectedItem.item, buyAmountText);
            gmInstance.currentGil -= totalCost;
            selectedItem.amount -= buyAmountText;

            // Update UI
            gilText.text = gmInstance.currentGil.ToString() + "g";
            inventoryHelper.PopulateInventoryButtons(itemsForSale, buyItemButtons);

            // Disable parent button when item is sold out
            if (parentButton != null && selectedItem.amount <= 0)
            {
                parentButton.interactable = false;
            }
        }

        buyInBulkPanel.SetActive(false);
    }

    // Function used by the "Confirm" button in the "Buy" panel
    public void SellInBulk()
    {
        var gmInstance = GameManager.instance;
        
        // If player inputs a number thats more than what they have in the inventory, set the value to what they have
        if (sellAmountText > selectedItem.amount)
        {
            sellAmountText = selectedItem.amount;
        }

        int totalGil = sellAmountText * Mathf.FloorToInt(selectedItem.item.value * 0.5f);

        // Remove item from player inventory and increase gil
        gmInstance.RemoveItem(selectedItem.item, sellAmountText);
        gmInstance.currentGil += totalGil;

        // Update UI
        gilText.text = gmInstance.currentGil.ToString() + "g";
        inventoryHelper.PopulateInventoryButtons(gmInstance.playerInventory, sellItemButtons);
        sellInBulkPanel.SetActive(false);
    }

    // Helper function to set the amount for EventListener
    private void SetBuyAmount(string amount)
    {
        if (amount != "")
        {
            buyAmountText = int.Parse(amount);
        }
    }

    // Helper function to set the amount for EventListener
    private void SetSellAmount(string amount)
    {
        if (amount != "")
        {
            sellAmountText = int.Parse(amount);
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

    public void ClosePanel()
    {
        buyInBulkPanel.SetActive(false);
        sellInBulkPanel.SetActive(false);
    }
}
