using System.Collections;
using UnityEngine;

public class ProxyAudioSource : MonoBehaviour
{
    [SerializeField] 
    private AudioSource audio;

    public void Play(AudioClip clip = null)
    {
        if (clip != null)
            audio.clip = clip;
        else
            return;

        audio.volume = GameSettingsCache.EffectVolume;
        audio.Play();
        
        StartCoroutine(scheduleDestroy(clip.length));
    }

    private IEnumerator scheduleDestroy(float timeUntilPerform)
    {
        yield return new WaitForSeconds(timeUntilPerform);
        
        Destroy(gameObject);
    }
}