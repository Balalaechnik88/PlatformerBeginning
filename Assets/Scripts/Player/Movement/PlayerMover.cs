using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GroundSensor _ground;
    [SerializeField] private PlayerHorizontalMover _horizontalMover;
    [SerializeField] private PlayerJumper _jumper;
    [SerializeField] private SpriteDirectionRotator _rotator;

    public bool IsGrounded => _ground.IsGrounded;
    public float CurrentSpeedX => _horizontalMover.CurrentSpeedX;

    private void Awake()
    {
        if (_input == null)
            Debug.LogError($"[{nameof(PlayerMover)}] InputReader2D не назначен.", this);
        if (_ground == null) 
            Debug.LogError($"[{nameof(PlayerMover)}] GroundSensor2D не назначен.", this);
        if (_horizontalMover == null) 
            Debug.LogError($"[{nameof(PlayerMover)}] PlayerHorizontalMover2D не назначен.", this);
        if (_jumper == null) 
            Debug.LogError($"[{nameof(PlayerMover)}] PlayerJumper2D не назначен.", this);
        if (_rotator == null)
            Debug.LogError($"[{nameof(PlayerMover)}] SpriteDirectionRotator2D не назначен.", this);
    }

    private void Update()
    {
        float horizontal = _input.Horizontal;
        _rotator.SetFacingDirection(horizontal);

        bool isJumpPressed = _input.ConsumeJumpPressed();
        _jumper.TryJump(IsGrounded, isJumpPressed);
    }

    private void FixedUpdate()
    {
        _horizontalMover.ApplyHorizontal(_input.Horizontal);
    }
}
