using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreCard;

    [SerializeField] 
    private GameObject scrollContent;

    [SerializeField] 
    private TMP_Text recentScore;

    [SerializeField] 
    private Toggle timeAttack;

    [SerializeField]
    private AudioSource mainLoop;
    
    private void Start()
    {
        timeAttack.isOn = GameSettingsCache.TimeAttack;
        GameSettingsCache.Ranking = CSVStorage.Read<PlayerScore>(CSVStorage.SCORES_FILE_NAME).ToList();

        if (GameSettingsCache.RecentScore != null)
            recentScore.text =
                $"Recent Score\nTime: {GameSettingsCache.RecentScore.UsedTime:F2}\nScore: {GameSettingsCache.RecentScore.Score:000000}\nDifficulty: {(Difficulty)GameSettingsCache.RecentScore.Difficulty}";
        
        createItem();
    }

    private void Update()
    {
        mainLoop.volume = GameSettingsCache.MusicVolume;
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