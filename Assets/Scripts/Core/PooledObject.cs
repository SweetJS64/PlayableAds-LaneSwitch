using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool _pool;
    private bool _isInPool;

    public void SetPool(ObjectPool pool)
    {
        _pool = pool;
        _isInPool = true;
    }

    public void OnTakenFromPool()
    {
        _isInPool = false;
    }

    public void ReturnToPool()
    {
        if (_isInPool)
            return;

        _isInPool = true;

        if (_pool == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _pool.ReturnToPool(this);
    }
}