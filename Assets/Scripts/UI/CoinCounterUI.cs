using UnityEngine;
using TMPro;

public class CoinCounterUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private TMP_Text _coinsText;

    private void Awake()
    {
        if (_playerCollector == null)
        {
            Debug.LogError(
                $"[{nameof(CoinCounterUI)}] Не назначен PlayerCollector2D.",
                this);
            return;
        }

        if (_coinsText == null)
        {
            Debug.LogError(
                $"[{nameof(CoinCounterUI)}] Не назначен TMP_Text.",
                this);
            return;
        }
    }

    private void OnEnable()
    {
        if (_playerCollector != null)
        {
            _playerCollector.CoinsChanged += UpdateCoinsText;
            UpdateCoinsText(_playerCollector.Coins);
        }
    }

    private void OnDisable()
    {
        if (_playerCollector != null)
        {
            _playerCollector.CoinsChanged -= UpdateCoinsText;
        }
    }

    private void UpdateCoinsText(int coins)
    {
        _coinsText.text = coins.ToString();
    }
}