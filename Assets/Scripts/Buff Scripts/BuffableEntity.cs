using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffableEntity : MonoBehaviour
{
    private readonly Dictionary<Buff, TimedBuff> buffList = new Dictionary<Buff, TimedBuff>();
    
    // Update is called once per frame
    void Update()
    {
        foreach (var buff in buffList.Values.ToList())
        {
            buff.Tick(1);

            if (buff.isFinished)
            {
                buffList.Remove(buff.ScriptedBuff);
            }
        }
    }

    public void AddBuff(TimedBuff buff)
    {
        if (buffList.ContainsKey(buff.ScriptedBuff))
        {
            buffList[buff.ScriptedBuff].Activate();
        }
        else
        {
            buffList.Add(buff.ScriptedBuff, buff);
            buff.Activate();
        }
    }
}
