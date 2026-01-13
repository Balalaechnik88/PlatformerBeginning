using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (_coinPrefab == null || _spawnPoints == null)
            return;

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            Transform point = _spawnPoints[i];

            if (point == null)
                continue;

            Instantiate(_coinPrefab, point.position, Quaternion.identity);
        }
    }
}
