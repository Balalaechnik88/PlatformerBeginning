using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const string SpeedParameterName = "Speed";
    private const string AttackTriggerName = "Attack";

    [SerializeField] private Animator _animator;

    private int _speedId;
    private int _attackId;

    private bool _hasSpeed;
    private bool _hasAttack;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _speedId = Animator.StringToHash(SpeedParameterName);
        _attackId = Animator.StringToHash(AttackTriggerName);

        _hasSpeed = HasParameter(_animator, _speedId);
        _hasAttack = HasParameter(_animator, _attackId);
    }

    public void SetSpeed(float speed)
    {
        if (_animator == null || _hasSpeed == false)
            return;

        _animator.SetFloat(_speedId, speed);
    }

    public void PlayAttack()
    {
        if (_animator == null || _hasAttack == false)
            return;

        _animator.SetTrigger(_attackId);
    }

    private static bool HasParameter(Animator animator, int id)
    {
        if (animator == null)
            return false;

        var parameters = animator.parameters;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].nameHash == id)
                return true;
        }

        return false;
    }
}
