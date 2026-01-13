using UnityEngine;

public interface ICollectable
{
    bool CanBeCollected(GameObject collector);
    void Collect(GameObject collector);
}
