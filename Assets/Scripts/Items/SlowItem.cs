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
        var original = Player.InitialSpeed;
        Player.CurrentSpeed *= percentage;

        yield return new WaitForSeconds(duration);

        Player.CurrentSpeed = original;
    }
}