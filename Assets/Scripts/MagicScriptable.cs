using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Magic/Create Magic")]
public class MagicScriptable : ScriptableObject
{
    [Header("Magic Information")]
    public string magicName;
    public string magicDescription;
    public int magicCost;
    public int magicDamage;
    public Sprite magicSprite;

    [Header("Magic Effects Properties")]
    public RuntimeAnimatorController magicAnimation;
    public float effectLength;
    public int soundEffect;

    [Header("Magic Buff/Debuff Properties")]
    public string buffEffect;

}
