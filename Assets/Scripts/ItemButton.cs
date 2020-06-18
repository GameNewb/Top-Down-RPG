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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        var gameManagerInstance = GameManager.instance;
        // Item is not empty, update the text
        if (gameManagerInstance.itemsHeld[buttonValue])
        {
            GameMenu.instance.SelectItem(gameManagerInstance.GetItemDetails(gameManagerInstance.itemsHeld[buttonValue]));
        }
    }

}
