using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private MovementConfigSO _movementConfig;

    [Header("Prefabs")]
    [SerializeField] private ObjectPool _obstaclePool;
    [SerializeField] private ObjectPool _buffPowerPool;
    [SerializeField] private ObjectPool _debuffPowerPool;
    [SerializeField] private GameObject _finishPrefab;

    [Header("Spawn settings")]
    [SerializeField] private float _spawnY = 0.5f;
    [SerializeField] private float _spawnZ = 25f;
    [SerializeField] private float _interval = 0.9f;

    private float _timer;
    private float[] _lanes;
    private int _lastBuffLane = -1;

    public Transform CurrentBossTargetPos { get; private set; }
    public BossAnimationController BossAnimationController { get; private set; }

    private void Awake()
    {
        var offset = _movementConfig.LaneOffset;
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
        
        _timer = _interval;
        SpawnRow();
    }

    public void PreSpawnRows(int count)
    {
        var rowSpacing = _movementConfig.MoveSpeed * _interval;
        for (var i = 1; i <= count; i++)
            SpawnRowAt(_spawnZ - rowSpacing * i);
    }

    private void SpawnRow() => SpawnRowAt(_spawnZ);

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

        SpawnFromPool(_obstaclePool, obstacleLane, z);
        SpawnFromPool(_buffPowerPool, buffLane, z);
        SpawnFromPool(_debuffPowerPool, debuffLane, z);
    }

    private void SpawnFromPool(ObjectPool pool, int laneIndex, float z)
    {
        var obj = pool.Get();
        obj.transform.position = new Vector3(_lanes[laneIndex], _spawnY, z);
        obj.transform.rotation = Quaternion.identity;
    }
    
    public void SpawnFinish()
    {
        var pos = new Vector3(0f, 0f, _spawnZ);
        var root = Instantiate(_finishPrefab, pos, Quaternion.identity);
        var targetMarker = root.GetComponentInChildren<BossTargetPos>();
        CurrentBossTargetPos = targetMarker != null ? targetMarker.transform : null;
        BossAnimationController = root.GetComponentInChildren<BossAnimationController>();
    }
}