using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private const float MinDistanceSquared = 0.0001f;

    [SerializeField] private float _viewRadius = 6f;
    [SerializeField] private LayerMask _playerLayerMask;

    public Transform DetectedPlayer { get; private set; }

    private void Update()
    {
        DetectedPlayer = FindPlayer();
    }

    private Transform FindPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _viewRadius, _playerLayerMask);

        if (playerCollider == null)
            return null;

        Vector2 delta = (Vector2)playerCollider.transform.position - (Vector2)transform.position;
        float distanceSquared = delta.sqrMagnitude;

        if (distanceSquared <= MinDistanceSquared)
            return playerCollider.transform;

        return playerCollider.transform;
    }
}
