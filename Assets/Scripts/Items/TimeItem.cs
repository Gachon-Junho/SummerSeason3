using UnityEngine;

public class TimeItem : Item
{
    [SerializeField]
    private float amount;
    
    protected override void AdjustGameState()
    {
        GameDirector.UsedTime -= amount;
    }
}
