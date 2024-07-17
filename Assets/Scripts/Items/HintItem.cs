using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HintItem : Item
{
    [SerializeField] 
    private float timeUntilHide;
    
    [SerializeField] 
    private GameObject hintCellPrefab;
    
    protected override IEnumerator AdjustGameState()
    {
        List<GameObject> hints = new List<GameObject>();
        var indexes = MazeGenerator.GetRoadIndexes();

        var offset = - new Vector3(MazeGenerator.mazeSize.x, MazeGenerator.mazeSize.y, 0) / 2 + MazeGenerator.transform.position;
        var start = indexes.First(i => Vector3.Distance(Player.transform.position, new Vector3(i.x, i.y, 0) + offset) < 0.6f);

        var poses = MazeGenerator.FindShortestPath(start, new Vector2Int(MazeGenerator.mazeSize.x - 2, 1));

        foreach (var pos in poses)
        {
            hints.Add(Instantiate(hintCellPrefab, new Vector3(pos.x, pos.y, 0) + offset, Quaternion.identity));
        }
        
        yield return new WaitForSeconds(timeUntilHide);

        foreach (var cell in hints)
        {
            Destroy(cell);
        }
    }
}