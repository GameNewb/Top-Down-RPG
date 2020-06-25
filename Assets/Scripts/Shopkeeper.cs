using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    private bool canOpen;
    
    public List<InventorySlots> shopkeeperInventory = new List<InventorySlots>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var shop = Shop.instance;
        if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !shop.shopMenu.activeInHierarchy)
        {
            shop.itemsForSale = shopkeeperInventory;
            shop.OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
