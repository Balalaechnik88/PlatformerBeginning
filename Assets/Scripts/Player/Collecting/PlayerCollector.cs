using System;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public event Action<int> CoinsChanged;

    public int Coins { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();

        if (collectable == null)
            return;

        if (collectable.CanBeCollected(gameObject) == false)
            return;

        collectable.Collect(gameObject);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        CoinsChanged?.Invoke(Coins);
    }
}
