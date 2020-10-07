using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelection : MonoBehaviour
{
    public string spellName;
    public int spellCost;
    public bool targetsPlayer;
    public Image image;
    public Text nameText;
    public Text costText;

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
        var bmInstance = BattleManager.instance;
        var currentMP = bmInstance.activeCombatants[bmInstance.currentTurn].GetComponent<ScriptableObjectProperties>().currentMP;

        // Check MP before allowing to use of magic
        if (currentMP >= spellCost)
        {
            bmInstance.magicMenu.SetActive(false);
            bmInstance.OpenTargetMenu(spellName, targetsPlayer);
            bmInstance.activeCombatants[bmInstance.currentTurn].GetComponent<ScriptableObjectProperties>().currentMP -= spellCost;
        }
        else
        {
            // Let player know there isn't enough MP
            bmInstance.battleNotice.theText.text = "Not Enough MP!";
            bmInstance.battleNotice.Activate();
            bmInstance.magicMenu.SetActive(false);
        }
    }
}
