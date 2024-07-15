using System.Collections;
using UnityEngine;

public class SlowItem : Item
{
    [SerializeField]
    private float percentage;
    
    [SerializeField]
    private float duration;
    
    protected override IEnumerator AdjustGameState()
    {
        var original = Player.Speed;
        Player.Speed *= percentage;

        yield return new WaitForSeconds(duration);

        Player.Speed = original;
    }
}