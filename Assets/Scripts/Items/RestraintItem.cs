using System.Collections;
using UnityEngine;

public class RestraintItem : Item
{
    [SerializeField]
    private float duration;
    
    protected override void AdjustGameState()
    {
        StartCoroutine(restraintTemporary());
    }
    
    IEnumerator restraintTemporary()
    {
        Player.Movable = false;

        yield return new WaitForSeconds(duration);

        Player.Movable = true;
    }
}
