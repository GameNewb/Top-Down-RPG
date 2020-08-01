using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleType[] potentialBattles;

    public bool activateOnEnter, activateOnStay, activateOnExit;
    public float timeBetweenBattles;
    public float betweenBattleEncounter;
    public bool deactivateAfterStarting; // For boss?

    private bool inArea;

    // Start is called before the first frame update
    void Start()
    {
        betweenBattleEncounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow battle if player can move and there's no active battle going on
        if (inArea && PlayerController.instance.canMove)
        {
            // Check if player is moving
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                betweenBattleEncounter -= Time.deltaTime;
            }

            if (betweenBattleEncounter <= 0)
            {
                betweenBattleEncounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);

                StartCoroutine(StartBattle());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activateOnEnter)
            {
                StartCoroutine(StartBattle());
            }
            else
            {
                inArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activateOnExit)
            {
                StartCoroutine(StartBattle());
            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattle()
    {
        UIFade.instance.FadeToBlack();
        GameManager.instance.activeBattle = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);
        
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.StartBattle(potentialBattles[selectedBattle].enemies);
        UIFade.instance.FadeFromBlack();

        if (deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }
    }
}
