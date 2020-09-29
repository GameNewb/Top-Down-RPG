using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Buffs/Poison Debuff")]
    public class ScriptablePoisonBuff : Buff
    {
        public int damage;

        public override TimedBuff InitializeBuff(GameObject obj)
        {
            return new TimedPoisonBuff(this, obj);
        }
    }
}
