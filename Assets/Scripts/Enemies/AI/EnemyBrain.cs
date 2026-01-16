using System;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private const float MinDistanceEpsilon = 0.0001f;

    [Header("References")]
    [SerializeField] private PlayerDetector _detector;
    [SerializeField] private WaypointPatroller _patroller;
    [SerializeField] private TargetChaser _chaser;
    [SerializeField] private MeleeAttack _attack;
    [SerializeField] private SpriteDirectionRotator _rotator;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyAnimationEventReceiver _eventReceiver;

    [Header("Attack")]
    [SerializeField] private float _attackStartDistance = 2f;

    private Transform _currentTarget;
    private bool _isAttacking;
    private float _attackStartDistanceSquared;
    private IEnemyStrategy _patrolStrategy;

    public event Action AttackStarted;

    public bool IsAttacking => _isAttacking;

    private void Awake()
    {
        _attackStartDistanceSquared = _attackStartDistance * _attackStartDistance;

        if (_detector == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен PlayerDetector.", this);
        if (_patroller == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен WaypointPatroller.", this);
        if (_chaser == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен TargetChaser.", this);
        if (_attack == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен MeleeAttack.", this);
        if (_rotator == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен SpriteDirectionRotator.", this);
        if (_animator == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен EnemyAnimator.", this);
        if (_eventReceiver == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен EnemyAnimationEventReceiver.", this);

        if (_patroller != null && _chaser != null)
            _patrolStrategy = new EnemyPatrolStrategy(_patroller, _chaser);
    }

    private void OnEnable()
    {
        if (_eventReceiver == null)
            return;

        _eventReceiver.HitEvent += OnHitEvent;
        _eventReceiver.AttackFinishedEvent += OnAttackFinishedEvent;
    }

    private void OnDisable()
    {
        if (_eventReceiver == null)
            return;

        _eventReceiver.HitEvent -= OnHitEvent;
        _eventReceiver.AttackFinishedEvent -= OnAttackFinishedEvent;
    }

    private void FixedUpdate()
    {
        UpdateTarget();
        TryStartAttackIfInRange();
        TickMovementAndAnimation();
    }

    private void UpdateTarget()
    {
        if (_detector == null)
        {
            _currentTarget = null;
            return;
        }

        _currentTarget = _detector.FindPlayer();
    }

    private void TryStartAttackIfInRange()
    {
        if (_isAttacking)
            return;

        if (_currentTarget == null || _attack == null)
            return;

        Vector2 vectorToTarget = (Vector2)_currentTarget.position - (Vector2)transform.position;
        float distanceToTargetSquared = vectorToTarget.sqrMagnitude;

        if (distanceToTargetSquared < MinDistanceEpsilon)
            return;

        if (distanceToTargetSquared > _attackStartDistanceSquared)
            return;

        if (_attack.TryStartAttack())
        {
            _isAttacking = true;
            _animator?.PlayAttack();
            AttackStarted?.Invoke();
        }
    }

    private void TickMovementAndAnimation()
    {
        if (_isAttacking)
        {
            StopMovement();
            ApplyFacingAndSpeed(0f);
            return;
        }

        if (_currentTarget == null)
        {
            TickPatrol();
            return;
        }

        TickChase(_currentTarget);
    }

    private void TickPatrol()
    {
        if (_patrolStrategy == null)
            return;

        _patrolStrategy.Tick();

        float patrolSpeedX = _patroller != null ? _patroller.CurrentSpeedX : 0f;
        ApplyFacingAndSpeed(patrolSpeedX);
    }

    private void TickChase(Transform target)
    {
        _patroller?.Stop();
        _chaser?.TickChase(target);

        float chaseSpeedX = _chaser != null ? _chaser.CurrentSpeedX : 0f;
        ApplyFacingAndSpeed(chaseSpeedX);
    }

    private void StopMovement()
    {
        _patroller?.Stop();
        _chaser?.Stop();
    }

    private void ApplyFacingAndSpeed(float speedX)
    {
        _rotator?.SetFacingDirection(speedX);
        _animator?.SetSpeed(Mathf.Abs(speedX));
    }

    private void OnHitEvent()
    {
        _attack?.Hit();
    }

    private void OnAttackFinishedEvent()
    {
        _attack?.AttackFinished();
        _isAttacking = false;
    }
}
