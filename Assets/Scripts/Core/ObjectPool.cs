using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _initialSize = 10;

    private readonly Queue<PooledObject> _pool = new();

    private void Awake()
    {
        for (var i = 0; i < _initialSize; i++)
        {
            CreateObject();
        }
    }

    private PooledObject CreateObject()
    {
        var instance = Instantiate(_prefab, transform);
        instance.SetActive(false);

        var pooledObject = instance.GetComponent<PooledObject>();
        if (pooledObject == null)
        {
            pooledObject = instance.AddComponent<PooledObject>();
        }

        pooledObject.SetPool(this);
        _pool.Enqueue(pooledObject);

        return pooledObject;
    }

    public GameObject Get()
    {
        if (_pool.Count == 0)
        {
            CreateObject();
        }

        var pooledObject = _pool.Dequeue();
        pooledObject.OnTakenFromPool();
        pooledObject.gameObject.SetActive(true);

        return pooledObject.gameObject;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.SetParent(transform);
        pooledObject.transform.localPosition = Vector3.zero;
        pooledObject.transform.localRotation = Quaternion.identity;

        _pool.Enqueue(pooledObject);
    }
}