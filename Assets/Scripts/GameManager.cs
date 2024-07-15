using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreCard;

    [SerializeField] 
    private GameObject scrollContent;
    
    private void Awake()
    {
        GameSettingsCache.Ranking ??= CSVStorage.Read<PlayerScore>(CSVStorage.SCORES_FILE_NAME).ToList();
        
        createItem();
    }

    private void createItem()
    {
        foreach (var score in GameSettingsCache.Ranking.OrderBy(p => p.UsedTime).ThenBy(p => p.Score))
        {
            var newScore = Instantiate(scoreCard, scrollContent.transform, true);
            newScore.GetComponent<ScoreCard>().SetData(score);
        }
    }
}