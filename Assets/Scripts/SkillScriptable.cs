using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Create Skill")]
public class SkillScriptable : ScriptableObject
{
    [Header("Skill Information")]
    public string skillName;
    public string skillDescription;
    public int skillCost;
    public int skillDamage;
    public Sprite skillSprite;

    [Header("Skill Effects Properties")]
    public RuntimeAnimatorController skillAnimation;
    public float effectLength;
    public int soundEffect;

    [Header("Skill Buff/Debuff Properties")]
    public Buff buffEffect;

}
