using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _checkRadius = 0.15f;
    [SerializeField] private LayerMask _groundMask;

    public bool IsGrounded { get; private set; }

    private void Update()
    {
        UpdateGroundedState();
    }

    private void UpdateGroundedState()
    {
        if (_checkPoint == null)
        {
            IsGrounded = false;
            return;
        }

        IsGrounded = Physics2D.OverlapCircle(_checkPoint.position, _checkRadius, _groundMask);
    }
}
