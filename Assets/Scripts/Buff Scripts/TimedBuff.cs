using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedBuff
{
    protected float duration;
    protected int effectStacks;
    protected readonly GameObject buffObj;
    public bool isFinished;

    public Buff ScriptedBuff { get; }
    
    public TimedBuff(Buff buff, GameObject obj)
    {
        ScriptedBuff = buff;
        buffObj = obj;
    }

    public void Tick(float delta)
    {
        duration -= delta;

        if (duration <= 0)
        {
            this.End();
            isFinished = true;
        }
    }

    public void Activate()
    {
        if (ScriptedBuff.isEffectStacked || duration <= 0)
        {
            this.ApplyEffect();
            effectStacks++;
        }

        if (ScriptedBuff.isDurationStacked || duration <= 0)
        {
            duration += ScriptedBuff.duration;
        }
    }

    protected abstract void ApplyEffect();
    public abstract void End();
}

