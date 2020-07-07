using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shopkeeper : MonoBehaviour
{
    private bool canOpen;
    
    public List<InventorySlots> shopkeeperInventory = new List<InventorySlots>();
    public static Shopkeeper instance;

    // Scene variable where the Shopkeeper is supposed to be in
    [SerializeField] private string sceneLocation;

    // Start is called before the first frame update
    void Awake()
    {
        // Keep only 1 instance of the object
        if (instance == null)
        {
            instance = this;

            // Allow shopkeeper to persist
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Make sure we're deleting a "copy" and not a newly loaded "Shopkeeper" object 
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
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

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene = SceneManager.GetActiveScene();

        if (gameObject)
        {
            // Activate / Deactive the Shopkeeper object when we're transition scenes
            gameObject.SetActive(scene.name == sceneLocation);
        }
    }
    
}
