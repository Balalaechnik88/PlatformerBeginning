using System;
using UnityEngine;

public class PlayerAnimationEventReceiver : MonoBehaviour
{
    public event Action HitEvent;
    public event Action AttackFinishedEvent;

    public void PlayerHitEvent()
    {
        HitEvent?.Invoke();
    }

    public void PlayerAttackFinishedEvent()
    {
        AttackFinishedEvent?.Invoke();
    }
}
