using ScriptableObjects;
using UnityEngine;

public class TimedPoisonBuff : TimedBuff
{
    public TimedPoisonBuff(Buff buff, GameObject obj) : base(buff, obj)
    {
    }

    protected override void ApplyEffect()
    {
        //Add speed increase to MovementComponent
        ScriptablePoisonBuff poisonBuff = (ScriptablePoisonBuff)ScriptedBuff;
    }

    public override void End()
    {
        //Revert speed increase
        ScriptablePoisonBuff poisonBuff = (ScriptablePoisonBuff)ScriptedBuff;
        effectStacks = 0;
    }
}
