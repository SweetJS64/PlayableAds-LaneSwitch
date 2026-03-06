using UnityEngine;

public class Spawner : MonoBehaviour
{
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
    private readonly float[] _lanes = { -2f, 0f, 2f };
    private int _lastObstacleLane = -1;

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
        int obstacleLane;

        if (_lastObstacleLane == -1)
        {
            obstacleLane = Random.Range(0, 3);
        }
        else
        {
            obstacleLane = (_lastObstacleLane + Random.Range(1, 3)) % 3;
        }

        _lastObstacleLane = obstacleLane;

        var buffLane = (obstacleLane + Random.Range(1, 3)) % 3;
        var debuffLane = 3 - obstacleLane - buffLane;

        var obstacle = ObstaclePool.Get();
        obstacle.transform.position = new Vector3(_lanes[obstacleLane], SpawnY, SpawnZ);
        obstacle.transform.rotation = Quaternion.identity;

        var buff = BuffPowerPool.Get();
        buff.transform.position = new Vector3(_lanes[buffLane], SpawnY, SpawnZ);
        buff.transform.rotation = Quaternion.identity;

        var debuff = DebuffPowerPool.Get();
        debuff.transform.position = new Vector3(_lanes[debuffLane], SpawnY, SpawnZ);
        debuff.transform.rotation = Quaternion.identity;
    }

    public void SpawnFinish()
    {
        var pos = new Vector3(0f, 0f, SpawnZ);
        var root = Instantiate(FinishPrefab, pos, Quaternion.identity);
        var targetMarker = root.GetComponentInChildren<BossTargetPos>();
        CurrentBossTargetPos = targetMarker != null ? targetMarker.transform : null;
    }
}