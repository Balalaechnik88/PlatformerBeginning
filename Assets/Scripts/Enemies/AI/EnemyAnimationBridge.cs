using UnityEngine;

public class EnemyAnimationBridge : MonoBehaviour
{
    [SerializeField] private EnemyBrain _brain;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private TargetChaser _chaser;
    [SerializeField] private WaypointPatroller _patroller;

    private bool _wasAttacking;

    private void Awake()
    {
        if (_brain == null) 
            Debug.LogError($"[{nameof(EnemyAnimationBridge)}] Не назначен EnemyBrain.", this);
        if (_animator == null) 
            Debug.LogError($"[{nameof(EnemyAnimationBridge)}] Не назначен EnemyAnimator.", this);
    }

    private void Update()
    {
        if (_brain == null || _animator == null)
            return;

        float speedX = 0f;

        if (_chaser != null && _brain.IsAttacking == false)
            speedX = _chaser.CurrentSpeedX;
        else if (_patroller != null)
            speedX = _patroller.CurrentSpeedX;

        _animator.SetSpeed(Mathf.Abs(speedX));

        bool isAttackingNow = _brain.IsAttacking;

        if (isAttackingNow && _wasAttacking == false)
        {
            _animator.PlayAttack();
        }

        _wasAttacking = isAttackingNow;
    }
}
