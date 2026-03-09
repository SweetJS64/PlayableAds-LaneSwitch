using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private MovementConfigSO _movementConfig;

    private PooledObject _pooledObject;
    
    private void Awake()
    {
        _pooledObject = GetComponent<PooledObject>();
    }
    
    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.State != GameState.Running &&
            GameManager.Instance.State != GameState.Finishing)
            return;
        
        transform.Translate(Vector3.back * _movementConfig.MoveSpeed * Time.deltaTime, 
            Space.World);

        if (transform.position.z <= _movementConfig.DespawnZ)
            _pooledObject.ReturnToPool();
    }
}