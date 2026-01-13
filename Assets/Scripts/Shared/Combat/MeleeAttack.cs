using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _attackPoint;

    [Header("Attack Settings")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackRadius = 0.8f;
    [SerializeField] private float _cooldownSeconds = 0.6f;
    [SerializeField] private LayerMask _targetLayerMask;

    [Header("Target Selection")]
    [SerializeField] private float _minTargetDistance = 0.01f;

    private float _cooldownRemainingSeconds;
    private bool _isAttacking;

    public bool IsAttacking => _isAttacking;
    public bool CanStartAttack => _isAttacking == false && _cooldownRemainingSeconds <= 0f;
    public float AttackRadius => _attackRadius;

    private void Update()
    {
        UpdateCooldownTimer();
    }

    public bool TryStartAttack()
    {
        if (CanStartAttack == false)
        {
            return false;
        }

        _isAttacking = true;
        return true;
    }

    public void Hit()
    {
        if (_isAttacking == false)
        {
            return;
        }

        Vector2 attackOrigin = GetAttackOrigin();
        Health nearestTargetHealth = FindNearestTargetHealth(attackOrigin);

        if (nearestTargetHealth == null)
        {
            return;
        }

        nearestTargetHealth.TakeDamage(_damage);
    }

    public void AttackFinished()
    {
        if (_isAttacking == false)
        {
            return;
        }

        _isAttacking = false;
        _cooldownRemainingSeconds = _cooldownSeconds;
    }

    private void UpdateCooldownTimer()
    {
        if (_cooldownRemainingSeconds <= 0f)
        {
            return;
        }

        _cooldownRemainingSeconds -= Time.deltaTime;

        if (_cooldownRemainingSeconds < 0f)
        {
            _cooldownRemainingSeconds = 0f;
        }
    }

    private Vector2 GetAttackOrigin()
    {
        if (_attackPoint != null)
        {
            return _attackPoint.position;
        }

        return transform.position;
    }

    private Health FindNearestTargetHealth(Vector2 attackOrigin)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackOrigin, _attackRadius, _targetLayerMask);

        if (hitColliders == null || hitColliders.Length == 0)
        {
            return null;
        }

        Health nearestHealth = null;

        float minTargetDistanceSquared = _minTargetDistance * _minTargetDistance;
        float nearestDistanceSquared = float.MaxValue;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider2D hitCollider = hitColliders[i];

            if (hitCollider == null)
            {
                continue;
            }

            Health targetHealth =
                hitCollider.GetComponent<Health>() ??
                hitCollider.GetComponentInParent<Health>() ??
                hitCollider.GetComponentInChildren<Health>();

            if (targetHealth == null)
            {
                continue;
            }

            Vector2 deltaToTarget = (Vector2)targetHealth.transform.position - attackOrigin;
            float distanceToTargetSquared = deltaToTarget.sqrMagnitude;

            if (distanceToTargetSquared < minTargetDistanceSquared)
            {
                continue;
            }

            if (distanceToTargetSquared < nearestDistanceSquared)
            {
                nearestDistanceSquared = distanceToTargetSquared;
                nearestHealth = targetHealth;
            }
        }

        return nearestHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 attackOrigin = Application.isPlaying
            ? GetAttackOrigin()
            : (_attackPoint != null ? (Vector2)_attackPoint.position : (Vector2)transform.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackOrigin, _attackRadius);
    }
}
