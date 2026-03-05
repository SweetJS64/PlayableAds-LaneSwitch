using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject ObstaclePrefab;

    [Header("Spawn settings")]
    [SerializeField] private float SpawnZ = 25f;
    [SerializeField] private float SpawnY = 0.5f;
    [SerializeField] private float LaneOffset = 2f;
    [SerializeField] private float Interval = 0.9f;

    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        SpawnObstacle();
        _timer = Interval;
    }

    private void SpawnObstacle()
    {
        var lane = Random.Range(0, 3);
        var x = (lane - 1) * LaneOffset;
        
        Vector3 pos = new Vector3(x, SpawnY, SpawnZ);
        Instantiate(ObstaclePrefab, pos, Quaternion.identity);
    }
}