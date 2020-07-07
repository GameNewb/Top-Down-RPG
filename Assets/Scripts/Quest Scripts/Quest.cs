using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public bool isComplete;

    public Quest() { }

    // Constructor
    public Quest(string qName, bool qComplete)
    {
        questName = qName;
        isComplete = qComplete;
    }
}
