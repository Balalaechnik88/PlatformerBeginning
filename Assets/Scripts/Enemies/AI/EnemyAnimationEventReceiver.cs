using System;
using UnityEngine;

public class EnemyAnimationEventReceiver : MonoBehaviour
{
    public event Action HitEvent;
    public event Action AttackFinishedEvent;

    public void EnemyHitEvent()
    {
        HitEvent?.Invoke();
    }

    public void EnemyAttackFinishedEvent()
    {
        AttackFinishedEvent?.Invoke();
    }
}
