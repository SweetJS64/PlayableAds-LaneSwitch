using UnityEngine;
using TMPro;

public class BossHealthText : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;

    [SerializeField] private float CountdownSpeed = 150f;

    private float _displayedHealth;
    private bool _initialized;

    private void Awake()
    {
        if (Text == null)
            Text = GetComponent<TMP_Text>();
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

        _displayedHealth = Mathf.MoveTowards(_displayedHealth, target, CountdownSpeed * Time.deltaTime);
        UpdateText();
    }

    private void UpdateText()
    {
        if (Text == null)
            return;

        Text.text = $"HEALTH: {Mathf.RoundToInt(_displayedHealth)}";
    }

    public void Refresh()
    {
        if (GameManager.Instance == null)
            return;

        _displayedHealth = GameManager.Instance.BossHealth;
        UpdateText();
    }
}