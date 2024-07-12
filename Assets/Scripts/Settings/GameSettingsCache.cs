using System;
using System.Collections.Generic;
using System.Drawing;

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

    public static float RemainingTime
    {
        get => (sizeX + sizeY) * (int)Difficulty % 3f;
    }
    
    public static float MusicVolume;
    public static float EffectVolume;
    public static Difficulty Difficulty;
    public static List<PlayerScore> Ranking;

    private static int sizeX;
    private static int sizeY;
}
