using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
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

    private const float MinDistanceEpsilon = 0.0001f;

    private Transform _currentTarget;
    private bool _isAttacking;

    private float _attackStartDistanceSqr;

    public bool IsAttacking => _isAttacking; 

    private void Awake()
    {
        _attackStartDistanceSqr = _attackStartDistance * _attackStartDistance;

        if (_detector == null) 
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен PlayerDetector2D.", this);
        if (_patroller == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен WaypointPatroller2D.", this);
        if (_chaser == null) 
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен TargetChaser2D.", this);
        if (_attack == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен MeleeAttack2D.", this);
        if (_rotator == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен SpriteDirectionRotator2D.", this);
        if (_animator == null) 
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен EnemyAnimator.", this);
        if (_eventReceiver == null)
            Debug.LogError($"[{nameof(EnemyBrain)}] Не назначен EnemyAnimationEventReceiver.", this);

        if (_eventReceiver != null)
        {
            _eventReceiver.HitEvent += OnHitEvent;
            _eventReceiver.AttackFinishedEvent += OnAttackFinishedEvent;
        }
    }

    private void OnDestroy()
    {
        if (_eventReceiver != null)
        {
            _eventReceiver.HitEvent -= OnHitEvent;
            _eventReceiver.AttackFinishedEvent -= OnAttackFinishedEvent;
        }
    }

    private void Update()
    {
        if (_detector == null)
            return;

        _currentTarget = _detector.DetectedPlayer;

        
        if (_isAttacking)
            return;

        if (_currentTarget == null || _attack == null)
            return;

        Vector2 toTarget = (Vector2)_currentTarget.position - (Vector2)transform.position;
        float distanceToTargetSqr = toTarget.sqrMagnitude;

        if (distanceToTargetSqr < MinDistanceEpsilon)
            return;

        if (distanceToTargetSqr <= _attackStartDistanceSqr)
        {
            bool didStartAttack = _attack.TryStartAttack();

            if (didStartAttack)
            {
                _isAttacking = true;
                _animator?.PlayAttack();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isAttacking)
        {
            _patroller?.Stop();
            _chaser?.Stop();
            _animator?.SetSpeed(0f);
            return;
        }

        if (_currentTarget == null)
        {
            _chaser?.Stop();
            _patroller?.TickPatrol();

            float patrolSpeedX = _patroller != null ? _patroller.CurrentSpeedX : 0f;
            _rotator?.SetFacingDirection(patrolSpeedX);
            _animator?.SetSpeed(Mathf.Abs(patrolSpeedX));
            return;
        }

        _patroller?.Stop();
        _chaser?.TickChase(_currentTarget);

        float chaseSpeedX = _chaser != null ? _chaser.CurrentSpeedX : 0f;
        _rotator?.SetFacingDirection(chaseSpeedX);
        _animator?.SetSpeed(Mathf.Abs(chaseSpeedX));
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
