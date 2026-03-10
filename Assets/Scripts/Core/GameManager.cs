using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _runDuration = 30f;
    [SerializeField] private int _bossHP = 300;

    [Header("UI")]
    [SerializeField] private TMP_Text _powerText;
    [SerializeField] private GameObject _winOverlay;
    [SerializeField] private GameObject _loseOverlay;
    [SerializeField] private GameObject _tapToStartText;

    [Header("References")]
    [SerializeField] private Spawner _spawner;
    [SerializeField] private PlayerBossFight _playerBossFight;
    [SerializeField] private PlayerAnimationController _playerAnimationController;
    
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }
    public int BossHealth { get; private set; }

    private int _power;
    private float _timeLeft;
    private BossAnimationController BossAnim => _spawner?.BossAnimationController;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        State = GameState.WaitingForTap;
        _timeLeft = _runDuration;
        BossHealth = _bossHP;
        AddPower(0);
    }

    private void Start()
    {
        _spawner?.PreSpawnRows(3);
    }

    private void Update()
    {
        if (State == GameState.Win || State == GameState.Lose)
            return;

        if (TryStartRun())
            return;

        UpdateRunTimer();
    }
    
    private void UpdateRunTimer()
    {
        if (State != GameState.Running)
            return;

        _timeLeft -= Time.deltaTime;

        if (_timeLeft > 0f)
            return;

        _timeLeft = 0f;
        State = GameState.Finishing;
        _spawner?.SpawnFinish();
    }
    
    private bool TryStartRun()
    {
        if (State != GameState.WaitingForTap)
            return false;

        if (InputHelper.WasTap())
        {
            State = GameState.Running;
            _playerAnimationController.PlayRun();
            _tapToStartText?.SetActive(false);
        }
        
        return true;
    }
    
    public void AddPower(int amount)
    {
        _power += amount;
        if (_powerText != null)
            _powerText.text = $"POWER: <color=blue>{_power}</color>";
    }
    
    public void StartBossFight()
    {
        if (State != GameState.Finishing)
            return;
        
        if (_spawner.CurrentBossTargetPos == null)
            return;

        State = GameState.BossFight;
        _playerBossFight.BeginFight(_spawner.CurrentBossTargetPos);
    }

    public void ResolveBossFight()
    {
        if (State != GameState.BossFight)
            return;

        if (_power >= _bossHP)
            WinSequence();
        else
            LoseToBoss();
    }

    public void LoseToObstacle()
    {
        if (State == GameState.Win || State == GameState.Lose)
            return;

        State = GameState.Lose;
        _playerAnimationController.PlayLose();
        BossAnim?.PlayDance();

        if (_loseOverlay != null)
            _loseOverlay.SetActive(true);
    }

    private void LoseToBoss()
    {
        State = GameState.Lose;
        _playerAnimationController.PlayIdle();

        if (BossAnim != null)
            BossAnim.PlayAttack(onHit: ShowLose);
        else
            ShowLose();
    }

    private void ShowLose()
    {
        _playerAnimationController.PlayLose();

        if (_loseOverlay != null)
            _loseOverlay.SetActive(true);
    }

    private void WinSequence()
    {
        State = GameState.Win;

        _playerAnimationController.PlayAttack(onComplete: () =>
        {
            BossHealth = 0;
            _winOverlay?.SetActive(true);
            BossAnim?.PlayLose();
            _playerAnimationController.PlayDance();
        });
    }
    
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}