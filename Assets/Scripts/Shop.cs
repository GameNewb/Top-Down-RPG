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

    public Item selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;
    
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
        //GameManager.instance.SortItems();
        //inventoryHelper.PopulateInventoryButtons(GameManager.instance.playerInventory, sellItemButtons);
        this.RefreshUI();

        // Set the first item to showcase
        sellItemButtons[0].Press();
    }

    public void SelectBuyItem(Item itemToBuy)
    {
        selectedItem = itemToBuy;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.itemDescription;
        buyItemValue.text = "Value: " + selectedItem.value + "g";
    }

    public void SelectSellItem(Item itemToSell)
    {
        selectedItem = itemToSell;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.itemDescription;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + "g";
    }

    // TODO: Modify script to add/subtract from Shopkeeper amount
    public void BuyItem()
    {
        if (selectedItem != null)
        {
            var gmInstance = GameManager.instance;

            if (gmInstance.currentGil >= selectedItem.value)
            {
                gmInstance.currentGil -= selectedItem.value;
                gmInstance.AddItem(selectedItem, 1);

                gilText.text = gmInstance.currentGil.ToString() + "g";
            }
        }
    }
    
    // TODO: Fix bug where after amount becomes < 0, player is still able to sell the item (use item amount instead to fix it)
    public void SellItem()
    {
        if (selectedItem != null)
        {
            var gmInstance = GameManager.instance;
            
            gmInstance.currentGil += Mathf.FloorToInt(selectedItem.value * .5f);
            gmInstance.RemoveItem(selectedItem, 1);

            gilText.text = gmInstance.currentGil.ToString() + "g";

            this.RefreshUI();
        }
    }

    private void RefreshUI()
    {
        GameManager.instance.SortItems();
        inventoryHelper.PopulateInventoryButtons(GameManager.instance.playerInventory, sellItemButtons);
    }
}
