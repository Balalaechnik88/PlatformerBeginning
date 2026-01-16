using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GroundSensor _ground;
    [SerializeField] private PlayerHorizontalMover _horizontalMover;
    [SerializeField] private PlayerJumper _jumper;
    [SerializeField] private SpriteDirectionRotator _rotator;

    public bool IsGrounded => _ground != null && _ground.IsGrounded;
    public float CurrentSpeedX => _horizontalMover != null ? _horizontalMover.CurrentSpeedX : 0f;

    private void Awake()
    {
        if (_input == null)
            Debug.LogError($"[{nameof(PlayerMover)}] InputReader не назначен.", this);
        if (_ground == null)
            Debug.LogError($"[{nameof(PlayerMover)}] GroundSensor не назначен.", this);
        if (_horizontalMover == null)
            Debug.LogError($"[{nameof(PlayerMover)}] PlayerHorizontalMover не назначен.", this);
        if (_jumper == null)
            Debug.LogError($"[{nameof(PlayerMover)}] PlayerJumper не назначен.", this);
        if (_rotator == null)
            Debug.LogError($"[{nameof(PlayerMover)}] SpriteDirectionRotator не назначен.", this);
    }

    private void Update()
    {
        if (_input == null)
            return;

        float horizontalInput = _input.Horizontal;
        _rotator?.SetFacingDirection(horizontalInput);

        bool jumpPressed = _input.ConsumeJumpPressed();
        _jumper?.TryJump(IsGrounded, jumpPressed);
    }

    private void FixedUpdate()
    {
        if (_input == null || _horizontalMover == null)
            return;

        _horizontalMover.ApplyHorizontal(_input.Horizontal);
    }
}
