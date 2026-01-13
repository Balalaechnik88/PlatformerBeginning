using UnityEngine;

public class SpriteDirectionRotator : MonoBehaviour
{
    private const float MinAbsoluteDirection = 0.01f;

    public void SetFacingDirection(float directionX)
    {
        if (Mathf.Abs(directionX) < MinAbsoluteDirection)
            return;

        float yRotation = directionX > 0f ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
