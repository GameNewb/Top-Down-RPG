using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    public static GameMenu instance;

    private bool disableControls = false;

    // Start is called before the first frame update
    void Start()
    {
        if (theMenu == null)
        {
            // Get child menu component
            theMenu = gameObject.transform.Find("Menu").gameObject;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape) && disableControls == false)
        {
            if (theMenu.activeInHierarchy)
            {
                ControlMenu(false);
            }
            else
            {
                ControlMenu(true);
            }
            
        } 
    }

    // Control the menu when different actions are performed 
    public void ControlMenu(bool control)
    {
        theMenu.SetActive(control);
        GameManager.instance.gameMenuOpen = control;
    }

    // Inactivate the menu when transitioning/loading between scenes
    public void DisableControl(bool control)
    {
        disableControls = control;
    }
}
