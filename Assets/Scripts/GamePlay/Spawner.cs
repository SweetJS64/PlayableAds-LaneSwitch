using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private MovementConfigSO MovementConfig;

    [Header("Prefabs")]
    [SerializeField] private ObjectPool ObstaclePool;
    [SerializeField] private ObjectPool BuffPowerPool;
    [SerializeField] private ObjectPool DebuffPowerPool;
    [SerializeField] private GameObject FinishPrefab;

    [Header("Spawn settings")]
    [SerializeField] private float SpawnY = 0.5f;
    [SerializeField] private float SpawnZ = 25f;
    [SerializeField] private float Interval = 0.9f;

    private float _timer;
    private float[] _lanes;
    private int _lastBuffLane = -1;

    public Transform CurrentBossTargetPos { get; private set; }
    public BossAnimationController BossAnimationController { get; private set; }

    private void Awake()
    {
        var offset = MovementConfig.LaneOffset;
        _lanes = new[] { -offset, 0f, offset };
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.State != GameState.Running)
            return;

        _timer -= Time.deltaTime;

        if (_timer > 0f)
            return;
        
        _timer = Interval;
        SpawnRow();
    }

    public void PreSpawnRows(int count)
    {
        var rowSpacing = MovementConfig.MoveSpeed * Interval;
        for (var i = 1; i <= count; i++)
            SpawnRowAt(SpawnZ - rowSpacing * i);
    }

    private void SpawnRow() => SpawnRowAt(SpawnZ);

    private void SpawnRowAt(float z)
    {
        int buffLane;

        if (_lastBuffLane == -1)
            buffLane = Random.Range(0, 3);
        else
            buffLane = (_lastBuffLane + Random.Range(1, 3)) % 3;

        _lastBuffLane = buffLane;

        var obstacleLane = (buffLane + Random.Range(1, 3)) % 3;
        var debuffLane = 3 - buffLane - obstacleLane;

        SpawnFromPool(ObstaclePool, obstacleLane, z);
        SpawnFromPool(BuffPowerPool, buffLane, z);
        SpawnFromPool(DebuffPowerPool, debuffLane, z);
    }

    private void SpawnFromPool(ObjectPool pool, int laneIndex, float z)
    {
        var obj = pool.Get();
        obj.transform.position = new Vector3(_lanes[laneIndex], SpawnY, z);
        obj.transform.rotation = Quaternion.identity;
    }
    
    public void SpawnFinish()
    {
        var pos = new Vector3(0f, 0f, SpawnZ);
        var root = Instantiate(FinishPrefab, pos, Quaternion.identity);
        var targetMarker = root.GetComponentInChildren<BossTargetPos>();
        CurrentBossTargetPos = targetMarker != null ? targetMarker.transform : null;
        BossAnimationController = root.GetComponentInChildren<BossAnimationController>();
    }
}