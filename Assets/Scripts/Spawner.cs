using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject ObstaclePrefab;
    [SerializeField] private GameObject BuffPowerPrefab;
    [SerializeField] private GameObject DebuffPowerPrefab;
    [SerializeField] private GameObject FinishPrefab;

    [Header("Spawn settings")]
    [SerializeField] private float SpawnY = 0.5f;
    [SerializeField] private float SpawnZ = 25f;
    [SerializeField] private float Interval = 0.9f;

    private float _timer;
    private readonly float[] _lanes = { -2f, 0f, 2f };

    public Transform CurrentBossTargetPos { get; private set; }
    
    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.State != GameState.Running)
            return;

        _timer -= Time.deltaTime;

        if (_timer > 0f)
            return;

        SpawnRow();
        _timer = Interval;
    }

    private void SpawnRow()
    {
        var obstacleLane = Random.Range(0, 3);

        int buffLane;
        do
        {
            buffLane = Random.Range(0, 3);
        }
        while (buffLane == obstacleLane);

        var debuffLane = 3 - obstacleLane - buffLane;
        
        var obstaclePos = new Vector3(_lanes[obstacleLane], SpawnY, SpawnZ);
        Instantiate(ObstaclePrefab, obstaclePos, Quaternion.identity);

        var buffPos = new Vector3(_lanes[buffLane], SpawnY, SpawnZ);
        Instantiate(BuffPowerPrefab, buffPos, Quaternion.identity);
        
        var debuffPos = new Vector3(_lanes[debuffLane], SpawnY, SpawnZ);
        Instantiate(DebuffPowerPrefab, debuffPos, Quaternion.identity);

    }

    public void SpawnFinish()
    {
        var pos = new Vector3(0f, 0f, SpawnZ);
        var root = Instantiate(FinishPrefab, pos, Quaternion.identity);
        var targetMarker = root.GetComponentInChildren<BossTargetPos>();
        CurrentBossTargetPos = targetMarker != null ? targetMarker.transform : null;
    }
}