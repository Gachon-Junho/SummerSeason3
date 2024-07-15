using System.Collections;
using UnityEngine;

public class ScoreItem : Item
{
    [SerializeField] 
    private int amount = 100;
    
    protected override IEnumerator AdjustGameState()
    {
        GameDirector.Score += amount;
        
        yield break;
    }
}