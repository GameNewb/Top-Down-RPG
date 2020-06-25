using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject buyMenu;
    [SerializeField] private GameObject sellMenu;

    [SerializeField] private Text gilText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && !shopMenu.activeInHierarchy)
        {
            this.OpenShop();
            this.OpenBuyMenu();
        }
        else if (Input.GetButtonDown("Fire2") && shopMenu.activeInHierarchy)
        {
            this.CloseShop();
        }
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);

        GameManager.instance.dialogActive = true;

        gilText.text = GameManager.instance.currentGil.ToString() + "g";
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        buyMenu.SetActive(false);
        sellMenu.SetActive(false);

        GameManager.instance.dialogActive = false;
    }

    public void OpenBuyMenu()
    {
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);
    }

    public void OpenSellMenu()
    {
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
    }
}
