using UnityEngine;

public class PlayerBossFight : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _reachDistance = 0.1f;

    private Transform _target;
    private bool _isActive;

    public void BeginFight(Transform target)
    {
        _target = target;
        _isActive = _target != null;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        if (GameManager.Instance.State != GameState.BossFight)
            return;

        var direction = _target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            _moveSpeed * Time.deltaTime
        );
        
        if (Vector3.Distance(transform.position, _target.position) <= _reachDistance)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _isActive = false;
            GameManager.Instance.ResolveBossFight();
        }
    }
}