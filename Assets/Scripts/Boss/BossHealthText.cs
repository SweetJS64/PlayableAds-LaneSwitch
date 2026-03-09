using UnityEngine;
using TMPro;

public class BossHealthText : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;

    [SerializeField] private float _countdownSpeed = 150f;

    private float _displayedHealth;
    private bool _initialized;

    private void Awake()
    {
        if (_healthText == null)
            _healthText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (!_initialized)
        {
            _displayedHealth = GameManager.Instance.BossHealth;
            _initialized = true;
            UpdateText();
            return;
        }

        var target = GameManager.Instance.BossHealth;
        if (Mathf.Approximately(_displayedHealth, target))
            return;

        _displayedHealth = Mathf.MoveTowards(_displayedHealth, target, _countdownSpeed * Time.deltaTime);
        UpdateText();
    }

    private void UpdateText()
    {
        if (_healthText == null)
            return;

        _healthText.text = $"HEALTH: {Mathf.RoundToInt(_displayedHealth)}";
    }

}