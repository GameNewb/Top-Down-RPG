using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;
    public Item buttonItem;

    private Button parentObject;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Press()
    {
        var gameManagerInstance = GameManager.instance;

        // Player opens up the menu, display appropriate item info
        if (GameMenu.instance.theMenu.activeInHierarchy)
        {
            // Item is not empty, update the text
            if (gameManagerInstance.playerInventory[buttonValue].item != null)
            {
                GameMenu.instance.SelectItem(gameManagerInstance.GetItemDetails(gameManagerInstance.playerInventory[buttonValue].item));
            }

            // Close the Character Selection Panel when selecting a new item from inventory
            GameObject characterSelectionPanel = gameObject.transform.parent.gameObject.transform.parent.Find("Character Selection Panel").gameObject;
            if (characterSelectionPanel.activeInHierarchy)
            {
                characterSelectionPanel.SetActive(false);
            }
        }
        
        // Player opens up the shop
        if (Shop.instance.shopMenu.activeInHierarchy)
        {
            var shopInstance = Shop.instance;
            
            // Buying items, update item name/description
            if (shopInstance.buyMenu.activeInHierarchy)
            {
                if (shopInstance.itemsForSale[buttonValue].item != null)
                {
                    shopInstance.SelectBuyItem(shopInstance.itemsForSale[buttonValue], parentObject);
                }
            }

            // Selling items, update item name/description
            if (shopInstance.sellMenu.activeInHierarchy)
            {
                if (gameManagerInstance.playerInventory[buttonValue].item != null)
                {
                    shopInstance.SelectSellItem(gameManagerInstance.playerInventory[buttonValue]);
                }
            }
            
        }
    }

}
