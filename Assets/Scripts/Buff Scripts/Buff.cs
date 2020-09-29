using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    [Header("Buff Properties")]
    public float duration;
    public bool isDurationStacked;
    public bool isEffectStacked;

    public abstract TimedBuff InitializeBuff(GameObject obj);
}
