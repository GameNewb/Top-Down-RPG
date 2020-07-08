using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private GameManager gameMngr;
    [SerializeField] string namePlaceHolder;
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

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameMngr = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Active in scene
        if (dialogBox.activeInHierarchy && Input.GetButtonUp("Fire1"))
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

                        // Re-enable player movement when dialog is inactive
                        GameManager.instance.dialogActive = false;

                        if (shouldMarkQuest)
                        {
                            shouldMarkQuest = false;

                            if (markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark);
                            }
                            else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                    }
                    else
                    {
                        // Change the name box for each speaker
                        CheckNameLabel();

                        // Start scrolling the dialog
                        StartCoroutine("ScrollingText");
                    }
                } 
                else
                {
                    // Stop the Coroutine to prevent multiple coroutines
                    StopCoroutine("ScrollingText");
                    isDialogRunning = false;

                    // Show and update the full text, clear it out first to prevent coroutine
                    dialogText.text = dialogLines[currentLine];
                }
                    
            } 
            else
            {
                justStarted = false;
            }
        }
    }

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        // Set line number
        dialogLines = newLines;
        currentLine = 0;

        // First iteration, replace the name
        CheckNameLabel();

        // Update text
        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;

        // Enable/disable for people/signs
        nameBox.SetActive(isPerson);

        // Disable player movement when dialog is active
        GameManager.instance.dialogActive = true;
    }

    private void CheckNameLabel()
    {
        if (dialogLines[currentLine].StartsWith(namePlaceHolder))
        {
            // Set name text and move to the next line
            nameText.text = dialogLines[currentLine].Replace(namePlaceHolder, "");
            currentLine++;
        }
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

    public void QuestActivation(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
