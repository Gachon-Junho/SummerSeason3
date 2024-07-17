using System;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] 
    private AudioClip clip;

    [SerializeField] 
    private GameObject proxy;
    
    protected GameDirector GameDirector => gameDirector ??= GameObject.Find("GameDirector").GetComponent<GameDirector>();
    protected Player Player => player ??= GameObject.Find("Player(Clone)").GetComponent<Player>();

    protected MazeGenerator MazeGenerator => mazeGenerator ??= GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>();
    
    private GameDirector gameDirector;
    private Player player;
    private MazeGenerator mazeGenerator;

    private void Update()
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D player)
    {
        player.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(AdjustGameState());

        var audio = Instantiate(proxy).GetComponent<ProxyAudioSource>();
        audio.Play(clip);
        
        Destroy(gameObject);
    }

    protected abstract IEnumerator AdjustGameState();
}
