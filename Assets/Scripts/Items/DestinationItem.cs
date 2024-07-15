using System;
using System.Collections;
using UnityEngine;

public class DestinationItem : Item
{
    protected override IEnumerator AdjustGameState()
    {
        GameDirector.GameOver(true);
        
        yield break;
    }
}
