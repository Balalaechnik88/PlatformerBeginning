using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHorizontalMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rigidbody2D;

    public float CurrentSpeedX => _rigidbody2D.velocity.x;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyHorizontal(float horizontal)
    {
        Vector2 velocity = _rigidbody2D.velocity;
        velocity.x = horizontal * _moveSpeed;
        _rigidbody2D.velocity = velocity;
    }
}
