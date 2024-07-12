using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingController : MonoBehaviour
{
    private void Start()
    {
        GameSettingsCache.SizeX = 50;
        GameSettingsCache.SizeY = 50;
        GameSettingsCache.Difficulty = Difficulty.Easy;
        GameSettingsCache.MusicVolume = 1;
        GameSettingsCache.EffectVolume = 1;
        GameSettingsCache.TimeAttack = false;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void TimeAttackToggled(Toggle t)
    {
        GameSettingsCache.TimeAttack = t.isOn;
    }

    public void WidthChanged(TMP_InputField i)
    {
        if (string.IsNullOrEmpty(i.text))
        {
            GameSettingsCache.SizeX = 50;
            return;
        }
        
        GameSettingsCache.SizeX = int.Parse(i.text);
    }

    public void HeightChanged(TMP_InputField i)
    {
        if (string.IsNullOrEmpty(i.text))
        {
            GameSettingsCache.SizeY = 50;
            return;
        }
        
        GameSettingsCache.SizeY = int.Parse(i.text);
    }

    public void DifficultyChanged(TMP_Dropdown d)
    {
        switch (d.value)
        {
            case 0:
                GameSettingsCache.Difficulty = Difficulty.Easy;
                break;
            
            case 1:
                GameSettingsCache.Difficulty = Difficulty.Normal;
                break;
            
            case 2:
                GameSettingsCache.Difficulty = Difficulty.Hard;
                break;
        }
    }

    public void MusicVolumeChanged(Slider s)
    {
        GameSettingsCache.MusicVolume = s.value;
    }

    public void EffectVolumeChanged(Slider s)
    {
        GameSettingsCache.EffectVolume = s.value;
    }
}