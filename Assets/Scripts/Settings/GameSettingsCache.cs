using System;
using System.Collections.Generic;

public static class GameSettingsCache
{
    public static bool TimeAttack;

    public static int SizeX
    {
        get => sizeX;
        set
        {
            sizeX = Math.Clamp(value % 2 == 0 ? value + 1 : value, 11, 101);
        }
    }

    public static int SizeY
    {
        get => sizeY;
        set
        {
            sizeY = Math.Clamp(value % 2 == 0 ? value + 1 : value, 11, 101);
        }
    }

    public static float RemainingTime => (sizeX + sizeY) * 3f / (int)Difficulty;

    public static float MusicVolume = 0.6f;
    public static float EffectVolume = 0.6f;
    public static Difficulty Difficulty = Difficulty.Easy;
    public static List<PlayerScore> Ranking = new List<PlayerScore>();

    private static int sizeX = 51;
    private static int sizeY = 51;

    public static PlayerScore RecentScore;

    public static void SubmitScore(int score, float usedTime)
    {
        var newScore = new PlayerScore
        {
            Score = score,
            UsedTime = usedTime,
            RemainingTime = TimeAttack ? RemainingTime - usedTime : -1,
            Difficulty = (int)Difficulty,
            SizeX = SizeX,
            SizeY = SizeY,
        };
        
        RecentScore = newScore;
        Ranking.Add(newScore);
        CSVStorage.Write(Ranking, CSVStorage.SCORES_FILE_NAME);
    }
}
