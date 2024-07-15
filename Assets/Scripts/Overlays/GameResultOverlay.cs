using System;
using TMPro;
using UnityEngine;

public class GameResultOverlay : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text score;
    
    private void Start()
    {
        time.text = GameSettingsCache.TimeAttack
            ? $"Remaining time: {GameSettingsCache.RemainingTime - gameDirector.UsedTime:F2}"
            : $"Time: {gameDirector.UsedTime:F2}";
        score.text = $"Score: {gameDirector.Score:000000}";
    }
}