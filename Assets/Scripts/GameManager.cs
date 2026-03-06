using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    WaitingForTap,
    Running,
    Finishing,
    BossFight,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private float RunDuration = 30f;
    [SerializeField] private TMP_Text PowerText;
    [SerializeField] private GameObject WinOverlay;
    [SerializeField] private GameObject LoseOverlay;
    [SerializeField] private Spawner Spawner;
    [SerializeField] private PlayerBossFight PlayerBossFight;
    [SerializeField] private int BossHP = 300;
    
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }
    
    public int BossHealth => BossHP;
    
    private int _power;
    private float _timeLeft;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        State = GameState.WaitingForTap;
        _timeLeft = RunDuration;
        AddPower(0);
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
        Spawner?.SpawnFinish();
    }
    
    private bool TryStartRun()
    {
        if (State != GameState.WaitingForTap)
            return false;

        if (WasTap())
            State = GameState.Running;

        return true;
    }
    
    public void AddPower(int amount)
    {
        _power += amount;
        if (PowerText != null)
            PowerText.text = $"POWER: <color=blue>{_power}</color>";
    }
    
    public void StartBossFight()
    {
        if (State != GameState.Finishing)
            return;

        if (Spawner.CurrentBossTargetPos == null)
            return;

        State = GameState.BossFight;
        PlayerBossFight.BeginFight(Spawner.CurrentBossTargetPos);
    }

    public void ResolveBossFight()
    {
        if (State != GameState.BossFight)
            return;

        if (_power >= BossHP)
            Win();
        else
            Lose();
    }
    
    private void Win()
    {
        if (State == GameState.Win || State == GameState.Lose)
            return;
        
        State = GameState.Win;

        if (WinOverlay != null)
            WinOverlay.SetActive(true);
    }
    
    public void Lose()
    {
        if (State == GameState.Win || State == GameState.Lose)
            return;

        State = GameState.Lose;
        
        if (LoseOverlay != null)
            LoseOverlay.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool WasTap()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;
        
        return false;
    }
}