using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingOverlay : MonoBehaviour
{
    [SerializeField]
    private Toggle timeAttack;
    
    [SerializeField]
    private TMP_InputField width;
    
    [SerializeField]
    private TMP_InputField height;
    
    [SerializeField]
    private TMP_Dropdown difficulty;
    
    [SerializeField]
    private Slider music;
    
    [SerializeField]
    private Slider effect;
    
    private void Start()
    {
        timeAttack.isOn = GameSettingsCache.TimeAttack;
        width.text = GameSettingsCache.SizeX.ToString();
        height.text = GameSettingsCache.SizeY.ToString();
        difficulty.value = (int)GameSettingsCache.Difficulty - 1;
        music.value = GameSettingsCache.MusicVolume;
        effect.value = GameSettingsCache.EffectVolume;
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