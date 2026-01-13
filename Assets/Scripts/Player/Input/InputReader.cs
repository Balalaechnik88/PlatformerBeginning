using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private string _horizontalAxisName = "Horizontal";
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _attackKey = KeyCode.F;

    public float Horizontal { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsAttackPressed { get; private set; }

    private void Update()
    {
        Horizontal = Input.GetAxisRaw(_horizontalAxisName);

        if (Input.GetKeyDown(_jumpKey))
            IsJumpPressed = true;

        if (Input.GetKeyDown(_attackKey))
            IsAttackPressed = true;
    }

    public bool ConsumeJumpPressed()
    {
        bool wasPressed = IsJumpPressed;
        IsJumpPressed = false;
        return wasPressed;
    }

    public bool ConsumeAttackPressed()
    {
        bool wasPressed = IsAttackPressed;
        IsAttackPressed = false;
        return wasPressed;
    }
}
