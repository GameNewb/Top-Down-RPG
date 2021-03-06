﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string[] lines;

    private bool canActivate;
    public bool isPerson = true;

    public string questToMark;
    public bool shouldActivateQuest;
    public bool markComplete;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Show the dialog box when player clicks on NPC
        if (canActivate && Input.GetButtonUp("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy && DialogManager.instance.isDialogRunning == false)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
            DialogManager.instance.QuestActivation(questToMark, markComplete);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
