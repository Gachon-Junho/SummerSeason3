using System;
using System.Collections;
using UnityEngine;

public class TimeItem : Item
{
    [SerializeField]
    private float amount;
    
    protected override IEnumerator AdjustGameState()
    {
        GameDirector.UsedTime = Math.Clamp(GameDirector.UsedTime - amount, 0, float.MaxValue);
        
        yield break;
    }
}
