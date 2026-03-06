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
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private GameObject WinOverlay;
    [SerializeField] private GameObject LoseOverlay;
    [SerializeField] private Spawner Spawner;
    [SerializeField] private PlayerBossFight PlayerBossFight;
   
    
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }

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
    }

    private void Update()
    {
        if (State == GameState.Win || State == GameState.Lose)
            return;

        if (State == GameState.WaitingForTap)
        {
            if (WasTap())
            {
                State = GameState.Running;
                Debug.Log("START");
            }
            return;
        }
        
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f && State == GameState.Running)
        {
            _timeLeft = 0f;
            State = GameState.Finishing;
            Spawner?.SpawnFinish();
            
            Debug.Log("FINISH REQUESTED");
        }

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

        Win();
    }
    
    private void Win()
    {
        if (State == GameState.Win || State == GameState.Lose)
            return;
        
        State = GameState.Win;

        if (WinOverlay != null)
            WinOverlay.SetActive(true);

        Debug.Log("WIN");
    }


    
    public void Lose()
    {
        if (State == GameState.Win || State == GameState.Lose)
            return;

        State = GameState.Lose;
        
        if (LoseOverlay != null)
            LoseOverlay.SetActive(true);
        
        Debug.Log("LOSE");
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