using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private int _value = 1;

    public bool CanBeCollected(GameObject collector) => true;

    public void Collect(GameObject collector)
    {
        PlayerCollector collector2D = collector.GetComponent<PlayerCollector>();

        if (collector2D != null)
        {
            collector2D.AddCoins(_value);
        }

        Destroy(gameObject);
    }
}
