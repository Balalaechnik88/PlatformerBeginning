using UnityEngine;

public class Medkit : MonoBehaviour, ICollectable
{
    [SerializeField] private int _healAmount = 2;

    public bool CanBeCollected(GameObject collector)
    {
        Health health = collector.GetComponent<Health>();
        return health != null && health.CurrentHealth > 0;
    }

    public void Collect(GameObject collector)
    {
        Health health = collector.GetComponent<Health>();

        if (health != null)
        {
            health.Heal(_healAmount);
        }

        Destroy(gameObject);
    }
}
