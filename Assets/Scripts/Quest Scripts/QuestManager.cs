using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> questMarkers = new List<Quest>();

    public static QuestManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MarkQuestComplete("Quest Test");
            MarkQuestIncomplete("Demons!");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            this.SaveQuestData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            this.LoadQuestData();
        }
    }

    // Function to retrieve the index of the quest we're looking for
    public int GetQuest(string questToFind)
    {
        for (int i = 0; i < questMarkers.Count; i++)
        {
            if (questMarkers[i].questName == questToFind)
            {
                return i;
            }
        }

        return -1;
    }

    // Function to check the status of the quest
    public bool CheckIfComplete(string questToCheck)
    {
        int questNumber = this.GetQuest(questToCheck);
        
        if (questNumber != -1)
        {
            return questMarkers[questNumber].isComplete;
        }

        return false;
    }

    // Function to mark a quest complete
    public void MarkQuestComplete(string questToMark)
    {
        questMarkers[this.GetQuest(questToMark)].isComplete = true;
        
        this.UpdateLocalQuestObjects();
    }

    // Function to mark a quest incomplete
    public void MarkQuestIncomplete(string questToMark)
    {
        questMarkers[this.GetQuest(questToMark)].isComplete = false;

        this.UpdateLocalQuestObjects();
    }

    // Function to update quest objectsp
    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if (questObjects.Length > 0)
        {
            for (int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

    public void SaveQuestData()
    {
        for (int i = 0; i < questMarkers.Count; i++)
        {
            // Save the values
            PlayerPrefs.SetInt("QuestMarker_" + questMarkers[i].questName, questMarkers[i].isComplete ? 1 : 0);
        }
    }

    public void LoadQuestData()
    {
        for (int i = 0; i < questMarkers.Count; i++)
        {
            // False value if quest gets added into the list after saving
            int valueToSet = 0;
            string questSavedData = "QuestMarker_" + questMarkers[i].questName;

            if (PlayerPrefs.HasKey(questSavedData))
            {
                // Get the values
                valueToSet = PlayerPrefs.GetInt(questSavedData);
            }

            // Set the value accordingly
            questMarkers[i].isComplete = valueToSet != 0;
        }
    }
}
