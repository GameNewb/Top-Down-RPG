using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    [SerializeField] private GameObject buyMenu;
    [SerializeField] private GameObject sellMenu;

    [SerializeField] private Text gilText;
    
    public List<InventorySlots> itemsForSale = new List<InventorySlots>();

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    private InventoryHelper inventoryHelper;

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
    }

    public void OpenSellMenu()
    {
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        // Display all the items that the player has
        GameManager.instance.SortItems();
        inventoryHelper.PopulateInventoryButtons(GameManager.instance.playerInventory, sellItemButtons);
    }
}
