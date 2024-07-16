using TMPro;
using UnityEngine;

public class ScoreCard : MonoBehaviour
{
    public PlayerScore PlayerScore;
        
    [SerializeField] 
    private TMP_Text difficulty;
        
    [SerializeField] 
    private TMP_Text score;
        
    [SerializeField] 
    private TMP_Text time;

    public void SetData(PlayerScore playerScore)
    {
        PlayerScore = playerScore;
            
        time.text = $"Time: {playerScore.UsedTime:F2}";
        score.text = $"{playerScore.Score:000000}";
        difficulty.text = $"Difficulty: {(Difficulty)playerScore.Difficulty}";
    }
}