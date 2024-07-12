using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] 
    private MazeGenerator mazeGenerator;

    [SerializeField]
    private GameObject[] positiveItemPrefab;
    
    [SerializeField]
    private GameObject[] negativeItemPrefab;

    public void GenerateItems()
    {
        var count = (GameSettingsCache.SizeX + GameSettingsCache.SizeY) / 4;
        
        switch (GameSettingsCache.Difficulty)
        {
            case Difficulty.Easy:
                createItem(count, 1);
                break;
            
            case Difficulty.Normal:
                createItem(count, 0.5f);
                break;
            
            case Difficulty.Hard:
                createItem(count, 0.3f);
                break;
        }
    }

    private void createItem(int count, float positiveRange)
    {
        var roads = mazeGenerator.GetRoadIndexes();
        
        for (int i = 0; i < count; i++)
        {
            var range = Random.Range(0f, 1f);
            GameObject item;

            var index = roads[Random.Range(0, roads.Count)];
            var pos = new Vector3(index.x, index.y, 0) - new Vector3(mazeGenerator.mazeSize.x, mazeGenerator.mazeSize.y, 0) / 2 + mazeGenerator.transform.position;

            if (range <= positiveRange)
                item = positiveItemPrefab[Random.Range(0, positiveItemPrefab.Length)];
            else
                item = negativeItemPrefab[Random.Range(0, negativeItemPrefab.Length)];
            
            Instantiate(item, pos, Quaternion.identity, transform);
        }
    }
}