using System.Collections;
using UnityEngine;

public class TimeItem : Item
{
    [SerializeField]
    private float amount;
    
    protected override IEnumerator AdjustGameState()
    {
        GameDirector.UsedTime -= amount;
        
        yield break;
    }
}
