using System;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected GameDirector GameDirector => gameDirector ??= GameObject.Find("GameDirector").GetComponent<GameDirector>();
    protected Player Player => player ??= GameObject.Find("Player(Clone)").GetComponent<Player>();
    
    private GameDirector gameDirector;
    private Player player;

    private void Update()
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D player)
    {
        player.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(AdjustGameState());
        Destroy(gameObject);
    }

    protected abstract IEnumerator AdjustGameState();
}
