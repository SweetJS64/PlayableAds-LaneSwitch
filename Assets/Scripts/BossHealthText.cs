using UnityEngine;
using TMPro;

public class BossHealthText : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;

    private void Awake()
    {
        if (Text == null)
            Text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (GameManager.Instance == null || Text == null)
            return;

        Text.text = $"HEALTH: {GameManager.Instance.BossHealth}";
    }
}