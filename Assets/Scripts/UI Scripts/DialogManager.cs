using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;

    public string[] dialogLines;

    public int currentLine;
    private bool justStarted;

    public static DialogManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Active in scene
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if (!justStarted)
                {
                    currentLine++;

                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                    else
                    {
                        // Update text
                        dialogText.text = dialogLines[currentLine];
                    }
                } 
                else
                {
                    justStarted = false;
                }

            }
        }
    }

    public void ShowDialog(string[] newLines)
    {
        // Set line number
        dialogLines = newLines;

        currentLine = 0;

        // Update text
        dialogText.text = dialogLines[0];
        dialogBox.SetActive(true);


        justStarted = true;
    }
}
