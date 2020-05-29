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
    public float textDelay = 0.02f;
    public bool isDialogRunning;
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
                // Ensure that we don't skip text since we're using button up instead of down
                if (!justStarted)
                {
                    // Check if dialogue is already scrolling
                    if (!isDialogRunning)
                    {
                        currentLine++;

                        // End the dialog after it reached the length
                        if (currentLine >= dialogLines.Length)
                        {
                            // Disabled the dialog box
                            dialogBox.SetActive(false);

                            // Re-enable player movement
                            PlayerController.instance.canMove = true;
                        }
                        else
                        {
                            StartCoroutine(ScrollingText());
                        }
                    } 
                    else
                    {
                        // Stop the Coroutine to prevent multiple coroutines
                        StopCoroutine(ScrollingText());
                        isDialogRunning = false;
                        
                        // Show and update the full text
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

        // Disable player movement
        PlayerController.instance.canMove = false;
    }

    public IEnumerator ScrollingText()
    {
        // Coroutine check
        isDialogRunning = true;

        // Clear the text so that we scroll through it
        dialogText.text = "";

        // Iterate through string characters to scroll through
        foreach (char letter in dialogLines[currentLine].ToCharArray())
        {
            // Slowly add a letter
            dialogText.text += letter;
            yield return new WaitForSeconds(textDelay);
        }
    }
}
