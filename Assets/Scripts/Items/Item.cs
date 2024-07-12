using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected GameDirector GameDirector;
    protected Player Player;

    private void Update()
    {
        GameDirector ??= GameObject.Find("GameDirector").GetComponent<GameDirector>();
        Player ??= GameObject.Find("Player(Clone)").GetComponent<Player>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D player)
    {
        AdjustGameState();
        Destroy(gameObject);
    }

    protected abstract void AdjustGameState();
}
