using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private MeleeAttack _attack;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private PlayerAnimationEventReceiver _eventReceiver;

    private void Awake()
    {
        if (_input == null)
            Debug.LogError($"[{nameof(PlayerAttacker)}] InputReader2D не назначен.", this);
        if (_attack == null)
            Debug.LogError($"[{nameof(PlayerAttacker)}] MeleeAttack2D не назначен.", this);
        if (_animator == null)
            Debug.LogError($"[{nameof(PlayerAttacker)}] PlayerAnimator не назначен.", this);
        if (_eventReceiver == null)
            Debug.LogError($"[{nameof(PlayerAttacker)}] PlayerAnimationEventReceiver не назначен.", this);

        _eventReceiver.HitEvent += OnHitEvent;
        _eventReceiver.AttackFinishedEvent += OnAttackFinishedEvent;
    }

    private void Update()
    {
        bool isAttackPressed = _input.ConsumeAttackPressed();

        if (isAttackPressed == false)
            return;

        bool didStartAttack = _attack.TryStartAttack();

        if (didStartAttack)
        {
            _animator.TriggerAttack();
        }
    }

    private void OnHitEvent()
    {
        _attack.Hit();
    }

    private void OnAttackFinishedEvent()
    {
        _attack.AttackFinished();
    }
}
